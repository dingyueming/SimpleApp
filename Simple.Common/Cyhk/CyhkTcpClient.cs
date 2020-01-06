using CommLiby.Cyhk;
using CommLiby.Cyhk.Models;
using Newtonsoft.Json;
using Simple.Common.SocketLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Common.Cyhk
{
    public partial class CyhkTcpClient : SingletonBase<CyhkTcpClient>
    {
        #region 字段 
        private bool? inited = null;
        private bool? initedTcp = null;
        private string _ip = null;
        private int _port = -1;

        private uint _userID;   //当前登录用户ID
        private string _userName; //当前登录用户用户名
        private string _userPwd; //当前登录用户密码
        private bool _isMobile = false; //是否移动用户登录
        private string _mac;    //移动设备SN

        private Timer _tm_tcpclient = null;   //连接控制任务
        //服务是否运行中
        private bool _serviceRuning => _tm_tcpclient != null;
        private bool _serviceClosed = false; //服务停止

        private bool _loginSucces = false; //登录成功

        private ITCPClient _tcpClient;
        readonly List<Command> _cmdSendList = new List<Command>(); //执行的命令列表

        private Timer _tm_dealrecdata = null; //接收到的数据处理任务
        private Task _tk_dealrecdata = null;
        private Timer _tm_sendcheck = null;   //发送命令检查任务

        private ushort _heartErrCount;  //心跳包失败次数
        private readonly StateObject _state;   //异步参数对象

        private bool connBusy = false;
        #endregion

        #region 事件
        /// <summary>
        /// 初始化完成事件
        /// </summary>
        public event Action<bool> InitCompleted;
        /// <summary>
        /// 登录完成事件
        /// </summary> 
        public event Action<bool> LoginSuccessed;
        /// <summary>
        /// 命令反馈事件
        /// </summary>
        public event CommandAckHandler CommandAck;
        /// <summary>
        /// 连接状态改变
        /// </summary>
        public event Action<ushort, bool> LinkStateChanged;

        /// <summary>
        /// GPS数据收到事件
        /// </summary>
        public event Action<string, Fields> GpsDataReceived;
        /// <summary>
        /// 锁控GPS数据收到事件
        /// </summary>
        public event Action<string, Fields> LockGpsDataReceived;
        /// <summary>
        /// 接收到车机的文字消息
        /// </summary>
        public event Action<string, Fields> MessageRecived;
        /// <summary>
        /// 应急接收到车机的文字消息
        /// </summary>
        public event Action<string, Fields> MessageRecived_YJGS;
        /// <summary>
        /// 接收到车机上来的图片
        /// </summary>
        public event Action<string, Fields> ImageReceived;
        /// <summary>
        /// 锁操作事件
        /// </summary>
        public event Action<string, Fields, byte> LockAction;
        /// <summary>
        /// 锁控报警事件
        /// </summary>
        public event Action<string, Fields> LockAlarmed;
        /// <summary>
        /// 接收到读取的分锁号
        /// </summary>
        public event Action<string, bool, byte, string> LockNoReceived;

        /// <summary>
        /// 接收到巡检信息反馈
        /// </summary>
        public event Action<string, Fields> InspectionReceived;
        /// <summary>
        /// 接收到终端参数信息
        /// </summary>
        public event Action<string, Fields> ParametersReceived;
        #endregion

        #region 属性
        public bool _connSuccess { get { return _tcpClient.ConnSuccess; } }
        public uint LoginUserID { get { return _userID; } }
        public bool LoginSuccess { get { return _loginSucces; } }
        public bool ServiceStrated { get { return _serviceRuning; } }
        #endregion

        #region 初始化

        public CyhkTcpClient()
        {
            _state = new StateObject();
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <returns></returns>
        public void Init(string ip, int port, string username, string userpwd, string mac = null)
        {
            _ip = ip;
            _port = port;
            _userName = username;
            _userPwd = userpwd;
            _mac = mac;
            string strCmdInfo = string.Empty;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Other/JsonFileFolder/CmdInfo.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    strCmdInfo = sr.ReadToEnd().ToString();
                }
            }
            Command.cmdInfoList = JsonConvert.DeserializeObject<List<DICT_CMD>>(strCmdInfo);

            string strCmdStatus = string.Empty;
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Other/JsonFileFolder/CmdStatus.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    strCmdStatus = sr.ReadToEnd().ToString();
                }
            }
            Command.cmdStatusList = JsonConvert.DeserializeObject<List<DICT_ERROR>>(strCmdStatus);
            InitCompleted?.Invoke(initedTcp == true);
        }

        /// <summary>
        /// 初始化TCPClient
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="isMobile"></param>
        public void InitTcpClient(ITCPClient tcpClient, bool isMobile = false)
        {
            _isMobile = isMobile;
            if (tcpClient != null)
            {
                _tcpClient = tcpClient;
                _tcpClient.ConnectSuccess += _tcpClient_ConnectSuccess;
                _tcpClient.SocketClosed += _tcpClient_SocketClosed;
                _tcpClient.SocketDisconnected += _tcpClient_SocketDisconnected;
                initedTcp = true;
            }
            else
            {
                initedTcp = false;
            }

            if (inited != null && initedTcp != null)
            {
                InitCompleted?.Invoke(inited == true && initedTcp == true);
            }
        }
        #endregion

        #region 启动服务
        private void ConnectMethod(object state)
        {
            lock (this) { if (connBusy) return; }

            if (!_serviceClosed)
            {
                lock (this)
                {
                    connBusy = true;
                }

                if (_connSuccess) //连接成功
                {
                    if (_loginSucces) //发送心跳包
                    {
                        bool suc = SendHeartBeat();
                        if (suc)
                            _tm_tcpclient.Change(new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 20));   //更改为20秒间隔
                        else
                            _tm_tcpclient.Change(new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 10));   //更改为10秒间隔
                    }
                    //else //登录和连接请求是紧密的 不行就重新连接
                    //connSuccess = false;
                }
                else
                {
                    //_state = new StateObject();
                    //未连接成功
                    _tcpClient.ConnectServer(_ip, _port, _state);
                }
            }
            else
            {
                _tm_tcpclient.Dispose();
                _tm_tcpclient = null;
            }

            lock (this)
            {
                connBusy = false;
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <returns></returns>
        public bool StartService()
        {
            if (string.IsNullOrWhiteSpace(_ip) || _port == -1) return false;
            if (_serviceRuning) return true;
            _serviceClosed = false;
            //_connSuccess = false;
            _loginSucces = false;


            if (_tm_tcpclient == null)
                _tm_tcpclient = new Timer(ConnectMethod, null, new TimeSpan(0, 0, 3), new TimeSpan(0, 0, 10));

            //处理数据线程
            /*if (_tm_dealrecdata == null)
                _tm_dealrecdata = new Timer(DealReceiveData, null, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 200));*/
            if (_tk_dealrecdata == null)
            {
                _tk_dealrecdata = new Task(new Action<object>(DealReceiveData), TaskCreationOptions.LongRunning);
            }
            if (_tk_dealrecdata.Status != TaskStatus.Running)
                _tk_dealrecdata.Start();


            if (_tm_sendcheck == null)
                _tm_sendcheck = new Timer(SendBSCheck, null, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 0, 0, 200));

            return true;
        }

        private void _tcpClient_ConnectSuccess()
        {
            SendConnectRequest(); //发送连接请求
        }

        private void _tcpClient_SocketClosed()
        {
            _loginSucces = false;
            if (_serviceClosed)
                _cmdSendList.Clear();
        }

        private void _tcpClient_SocketDisconnected()
        {
            InvokeCommandAckHandler(null);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public bool StopService()
        {
            if (!_serviceRuning) return true;
            if (_serviceClosed) return true;
            _serviceClosed = true;
            CloseConnection();  //发送退出命令

            _tcpClient.CloseSocket();

            GC.Collect();
            return true;
        }


        #endregion

        #region 发送指令
        /// <summary>
        /// 发送心跳包
        /// </summary>
        /// <returns></returns>
        public bool SendHeartBeat() => SendMessage(CommandID.KEEP_ALIVE, 0, _userID, 0);  //链路测试 

        /// <summary>
        /// 发送连接请求
        /// </summary>
        public bool SendConnectRequest()
        {
            byte[] msg = new byte[18];
            byte[] type_bs = NetTools.HostToNetworkOrderToBytes((ushort)1);
            Array.Copy(type_bs, 0, msg, 0, 2);
            return SendMessage(CommandID.CONNECT, 0, _userID, 0, msg);
        }

        /// <summary>
        /// 发送登录请求
        /// </summary>
        public bool SendLoginRequest()
        {
            byte[] msg = new byte[32];
            byte[] userNameBytes = NetTools.HostToNetworkOrderToBytes(_userName, 16);
            Array.Copy(userNameBytes, 0, msg, 0, 16);
            Array.Copy(NetTools.HostToNetworkOrderToBytes(_userPwd, 16), 0, msg, 16, 16);
            return SendMessage(CommandID.USER_LOGIN, 0, _userID, 0, msg);
        }

        /// <summary>
        /// 发送移动用户登录请求
        /// </summary>
        /// <returns></returns>
        public bool SendMobileLoginRequest()
        {
            byte[] msg = new byte[48];
            Array.Copy(NetTools.HostToNetworkOrderToBytes(_userName, 16), 0, msg, 0, 16);
            Array.Copy(NetTools.HostToNetworkOrderToBytes(_userPwd, 16), 0, msg, 16, 16);
            Array.Copy(NetTools.HostToNetworkOrderToBytes(_mac, 16), 0, msg, 32, 16);
            return SendMessage(CommandID.USER_LOGIN_MOBILE, 0, _userID, 0, msg);
        }

        /// <summary>
        /// 请求关闭与服务器的连接
        /// </summary>
        public bool CloseConnection() => SendMessage(CommandID.USER_LOGOUT, 0, _userID, 0);

        /// <summary>
        /// 向服务器发送消息
        /// </summary>
        /// <param name="cmd_id"></param>
        /// <param name="car"></param>
        /// <param name="message"></param>
        public bool SendMessage(ushort cmd_id, ICar car, byte[] message)
        {
            ushort mobileType = 0;
            ushort ciserverNo = 0;
            if (car != null)
            {
                mobileType = (ushort)car.MTYPE;
                ciserverNo = (ushort)car.CTYPE;
            }
            return SendMessage(cmd_id, mobileType, _userID, ciserverNo, message, 0, car);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cmd_id"></param>
        /// <param name="car"></param>
        /// <param name="messages"></param>
        public bool SendMessage(ushort cmd_id, ICar car, params object[] messages)
        {
            if (messages == null) return false;
            List<byte> list = new List<byte>();
            foreach (object obj in messages)
            {
                list.AddRange(ConvertObjectToBytes(obj));
            }
            return SendMessage(cmd_id, car, list.ToArray());
        }

        /// <summary>
        /// 向服务器发送消息
        /// </summary>
        /// <param name="cmd_id">命令号</param>
        /// <returns></returns>
        public bool SendMessage(ushort cmd_id, ushort mobileType, uint userID, ushort ciServerNo, byte[] message = null, ushort agentNo = 0, ICar car = null)
        {
            Command cmd = Command.Next();
            cmd.CmdId = cmd_id;
            cmd.Car = car; //记录发送命令的车辆

            try
            {
                int messageLength = message?.Length ?? 0;

                //转换网络字节序
                byte[] msg_l_bs = NetTools.HostToNetworkOrderToBytes((ushort)(messageLength + 16));
                byte[] cmd_id_bs = NetTools.HostToNetworkOrderToBytes(cmd_id);
                byte[] sn_bs = NetTools.HostToNetworkOrderToBytes(cmd.SequenceNumber);
                byte[] mobileType_bs = NetTools.HostToNetworkOrderToBytes(mobileType);
                byte[] userID_bs = NetTools.HostToNetworkOrderToBytes(userID);
                byte[] ciServerNo_bs = NetTools.HostToNetworkOrderToBytes(ciServerNo);
                byte[] agentNo_bs = NetTools.HostToNetworkOrderToBytes(agentNo);

                byte[] sendBytes = new byte[19 + messageLength];
                sendBytes[0] = (byte)'{';
                Array.Copy(msg_l_bs, 0, sendBytes, 1, 2);
                Array.Copy(cmd_id_bs, 0, sendBytes, 3, 2);
                Array.Copy(sn_bs, 0, sendBytes, 5, 2);
                Array.Copy(mobileType_bs, 0, sendBytes, 7, 2);
                Array.Copy(userID_bs, 0, sendBytes, 9, 4);
                Array.Copy(ciServerNo_bs, 0, sendBytes, 13, 2);
                Array.Copy(agentNo_bs, 0, sendBytes, 15, 2);
                if (message != null && message.Length > 0)
                    Array.Copy(message, 0, sendBytes, 17, messageLength);

                sendBytes[sendBytes.Length - 2] = CalcSum(sendBytes);
                sendBytes[sendBytes.Length - 1] = (byte)'}';

                if (!_loginSucces && cmd.Car != null)
                {
                    cmd.SendBytes = sendBytes;
                    return false;
                }
                else
                {
                    return SendBS(sendBytes, cmd);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (cmd_id != 2) //退出命令无需加入列表
                    _cmdSendList.Add(cmd); //加入队列
            }
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 向服务器发送字节
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public bool SendBS(byte[] bs, Command userToken = null) => SendBS(bs, 0, bs.Length, userToken);

        /// <summary>
        /// 发送数据，按照以下格式发送数据：
        /// |数据长度n|需要发送的数据
        ///    4字节        n字节
        /// </summary>
        /// <param name="data">需要发送的数据</param>
        /// <param name="offset">需要发送数据的偏移量</param>
        /// <param name="size">需要发送数据的长度</param>
        public bool SendBS(Byte[] data, int offset, int size, Command userToken = null)
        {
            if (!_connSuccess || _tcpClient == null)
                return false;

            if (data == null || data.Length == 0)
                return false;

            try
            {

                ISocketAsyncEventArgs sendArgs = _tcpClient.GetISocketAsyncEventArgs();
                sendArgs.SocketProtocol = SocketProtocol.Tcp;
                sendArgs.Completed += SendArgs_Completed;
                sendArgs.SetBuffer(data, offset, size);
                sendArgs.UserToken = userToken;

                bool suc = _tcpClient.SendAsync(sendArgs);
                if (userToken != null)
                {
                    userToken.SendStatus = CmdStatus.Unknown;
                    userToken.SendTime = DateTimeHelper.Now;
                }
                if (!suc) {
                    SendArgs_Completed(null, sendArgs);
                }
                return sendArgs.SocketError== SocketError.Success;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void SendArgs_Completed(object sender, ISocketAsyncEventArgs e)
        {
            try
            {
                e.SetBuffer(null, 0, 0);
                Command userToken = e.UserToken as Command;
                //如果传输的数据量为0，则表示链接已经断开
                if (e.SocketError != SocketError.Success)
                {
                    //发送失败
                    if (userToken != null)
                    {
                        userToken.SendStatus = CmdStatus.Failed;
                        userToken.AckCode = 6000; //发送失败
                        InvokeCommandAckHandler(userToken);
                        _cmdSendList.Remove(userToken); //移除命令对象
                        userToken = null;
                    }
                }
                else
                {
                    //发送成功
                    if (userToken != null)
                    {
                        userToken.SendStatus = CmdStatus.Success;                        
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                //通知数据发送完毕
                //sendResetEvent.Set();
            }
        }

        #endregion

        #region 接收服务器发送过来的数据
        DateTime lastGctime = DateTime.MaxValue;
        /// <summary>
        /// 处理收到的数据
        /// </summary>
        void DealReceiveData(object stat)
        {
            if (!_serviceClosed)
            {
                while (!_serviceClosed)
                {
                    _state.BufferQueue.EnqueueEvent.WaitOne(1000);
                    while (true)
                    {
                        try
                        {
                            //if (_state == null) break;
                            if (_state.BufferQueue.Count == 0) break; //队列为空不判断

                            try
                            {
                                byte[] msgBytes = FindMessage();
                                if (msgBytes == null) break;

                                #region 处理消息内容

                                //调试用
                                //string bytestr = "";
                                //foreach (byte msgByte in msgBytes)
                                //{
                                //    bytestr += msgByte + " ";
                                //}
                                //调试用 -结束

                                //0-16位是消息头
                                ushort recCmdId = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, 3);
                                ushort seqNum = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, 5);
                                int messageIndex = 17; //从17位开始是消息内容

                                #region 命令反馈

                                if (recCmdId == CommandID.CMD_ACK) //命令反馈
                                {
                                    ushort cmdId = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex);
                                    if (cmdId == CommandID.USER_LOGOUT) //用户在别处登录序列号
                                    {
                                        if (!_serviceClosed) //非主动关闭服务
                                        {
                                            StopService(); //停止服务
                                            LoginSuccessed?.Invoke(false); //登录退出
                                        }
                                    }
                                    else
                                    {
                                        if (cmdId == CommandID.USER_LOGIN ||
                                            cmdId == CommandID.USER_LOGIN_MOBILE) //用户登录
                                        {
                                            _userID = NetTools.NetworkToHostOrderFromBytesToUint(msgBytes, 9);
                                        }

                                        Command cmd =
                                            _cmdSendList.FirstOrDefault(x => x.SequenceNumber == seqNum); //存在消息对象
                                        if (cmd != null)
                                        {
                                            string mac =
                                                NetTools.NetworkToHostOrderFromBytesToString(msgBytes,
                                                    messageIndex += 2, 16);
                                            ushort status =
                                                NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes,
                                                    messageIndex += 16);
                                            ushort extraLength =
                                                NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes,
                                                    messageIndex += 2);
                                            byte[] extraData = null;
                                            if (extraLength > 0)
                                            {
                                                extraData = new byte[extraLength];
                                                Array.Copy(msgBytes, messageIndex += 2, extraData, 0, extraLength);
                                            }

                                            cmd.AckCode = status;
                                            cmd.Mac = mac;
                                            InvokeCommandAckHandler(cmd, extraData);
                                            _cmdSendList.Remove(cmd); //已经接收移除命令
                                            cmd = null;
                                        }
                                    }
                                }

                                #endregion

                                #region GPS位置信息

                                else if (recCmdId == CommandID.GPS_PACKET) //GPS位置信息
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);

                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "Mac"),
                                        new Field(new Range(33, 2u), typeof(short), "Version"),
                                        new Field(new Range(35, 2u), typeof(ushort), "Direct"),
                                        new Field(new Range(37, 4u), typeof(uint), "Status"),
                                        new Field(new Range(41, 4u), typeof(uint), "StatusEx"),
                                        new Field(new Range(45, 4u), typeof(uint), "AlarmStatus"),
                                        new Field(new Range(49, 4u), typeof(uint), "AlarmStatusEx"),
                                        new Field(new Range(53, 1u), typeof(byte), "IsLocate"),
                                        new Field(new Range(54, 1u), typeof(byte), "locatemode"),
                                        new Field(new Range(55, 12u), typeof(string), "GpsTime"),
                                        new Field(new Range(67, 4u), typeof(uint), "Longitude"),
                                        new Field(new Range(71, 4u), typeof(uint), "Latitude"),
                                        new Field(new Range(75, 2u), typeof(short), "High"),
                                        new Field(new Range(77, 2u), typeof(ushort), "Speed"),
                                        new Field(new Range(79, 4u), typeof(uint), "Mileage"),
                                        new Field(new Range(83, 1u), typeof(byte), "Haveoil"),
                                        new Field(new Range(84, 2u), typeof(ushort), "Oil1"),
                                        new Field(new Range(86, 2u), typeof(ushort), "Oil2"),
                                        new Field(new Range(88, 2u), typeof(ushort), "Oil3"),
                                        new Field(new Range(90, 2u), typeof(ushort), "Oil4"),
                                        new Field(new Range(92, 2u), typeof(ushort), "AdditionItem"),
                                        new Field(new Range(94, msgBytes.Length - 1), typeof(byte[]), "AdditionBytes")
                                    }, msgBytes);

                                    if (GpsDataReceived != null) //调用事件
                                    {
                                        GpsDataReceived.Invoke(mac, fds);
                                    }
                                }

                                #endregion

                                #region 锁状态信息

                                else if (recCmdId == CommandID.LOCKGPS_PACKET) //锁状态信息
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "mac"),
                                        new Field(new Range(33, 12u), typeof(string), "Time"),
                                        new Field(new Range(45, 1u), typeof(byte), "主锁状态"),
                                        new Field(new Range(46, 12u), typeof(string), "分锁状态"),
                                        new Field(new Range(58, 2u), typeof(short), "区域")
                                    }, msgBytes);

                                    if (LockGpsDataReceived != null) //调用事件
                                    {
                                        LockGpsDataReceived.Invoke(mac, fds);
                                    }
                                }

                                #endregion

                                #region 报警消息 废弃

                                else if (recCmdId == CommandID.AlarmInfo) //报警消息 废弃
                                {
                                    //string mac = NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    //ushort direct = (ushort)(NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex += 16) / 10);
                                    //ushort alramStatus = NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex += 2);
                                    //ushort isLocate = NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex += 2);
                                    //string timestr = NetworkToHostOrderFromBytesToString(msgBytes, messageIndex += 2, 12);
                                    //double lo = NetworkToHostOrderFromBytesToUint(msgBytes, messageIndex += 12) / 1000000.0;
                                    //double la = NetworkToHostOrderFromBytesToUint(msgBytes, messageIndex += 4) / 1000000.0;
                                    //short reserverd = NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex += 4);
                                    //ushort speed = NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex += 2);
                                    //uint mileage = NetworkToHostOrderFromBytesToUint(msgBytes, messageIndex += 2);
                                }

                                #endregion

                                #region 上发消息

                                else if (recCmdId == CommandID.UP_MSG) //上发消息
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "Mac"),
                                        new Field(new Range(33, 2u), typeof(short), "MsgType"),
                                        new Field(new Range(35, 2u), typeof(ushort), "MsgLen"),
                                        new Field(new Range(37, msgBytes.Length - 1), typeof(byte[]), "Message"),
                                    }, msgBytes);

                                    if (MessageRecived != null) //调用事件
                                    {
                                        MessageRecived.Invoke(mac, fds);
                                    }
                                }
                                else if (recCmdId == CommandID.UP_MSG_GSYJ)
                                {
                                    string mac = NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "Mac"),
                                        new Field(new Range(33, 1u), typeof(byte), "NetType"),
                                        new Field(new Range(34, 4u), typeof(int), "MsgLen"),
                                        new Field(new Range(38, msgBytes.Length - 1), typeof(byte[]), "Message"),
                                    }, msgBytes);

                                    if (MessageRecived_YJGS != null) //调用事件
                                    {
                                        MessageRecived_YJGS.Invoke(mac, fds);
                                    }
                                }

                                #endregion

                                #region 上传图像数据通知

                                else if (recCmdId == CommandID.ImageInfo) //图像消息传输
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "mac"),
                                        new Field(new Range(33, 4u), typeof(int), "Seq"),
                                    }, msgBytes);

                                    ImageReceived?.Invoke(mac, fds);
                                }

                                #endregion

                                #region 注册卡刷卡及中心解锁/封锁操作结果

                                else if (recCmdId == CommandID.LCV_ACTION)
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "mac"),
                                        new Field(new Range(33, 12u), typeof(string), "Time"),
                                        new Field(new Range(45, 8u), typeof(string), "注册卡号"),
                                        new Field(new Range(53, 1u), typeof(byte), "是否中心操作"),
                                        new Field(new Range(54, 1u), typeof(byte), "是否黑名单"),
                                        new Field(new Range(55, 1u), typeof(byte), "状态"),
                                        new Field(new Range(56, 1u), typeof(byte), "结果"),
                                        new Field(new Range(57, 2u), typeof(short), "IsLocate"),
                                        new Field(new Range(59, 4u), typeof(int), "Longitude"),
                                        new Field(new Range(63, 4u), typeof(int), "Latitude"),
                                        new Field(new Range(67, 2u), typeof(short), "Areaid"),
                                    }, msgBytes);

                                    if (LockAction != null)
                                    {
                                        LockAction.Invoke(mac, fds, 0);
                                    }
                                }

                                #endregion

                                #region 非注册卡刷卡操作

                                else if (recCmdId == CommandID.UnRegCard)
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "mac"),
                                        new Field(new Range(33, 12u), typeof(string), "Time"),
                                        new Field(new Range(45, 8u), typeof(string), "注册卡号"),
                                        new Field(new Range(53, 1u), typeof(byte), "状态"),
                                    }, msgBytes);

                                    if (LockAction != null)
                                    {
                                        LockAction.Invoke(mac, fds, 1);
                                    }
                                }

                                #endregion

                                #region 锁控报警

                                else if (recCmdId == CommandID.LCV_ALARM)
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "Mac"),
                                        new Field(new Range(33, 2u), typeof(ushort), "Direct"),
                                        new Field(new Range(35, 2u), typeof(short), "AlarmStatus"),
                                        new Field(new Range(37, 2u), typeof(short), "IsLocate"),
                                        new Field(new Range(39, 12u), typeof(string), "GpsTime"),
                                        new Field(new Range(51, 4u), typeof(int), "Longitude"),
                                        new Field(new Range(55, 4u), typeof(int), "Latitude"),
                                        new Field(new Range(59, 2u), typeof(short), "areaid"),
                                        new Field(new Range(61, 2u), typeof(ushort), "Speed"),
                                        new Field(new Range(63, 4u), typeof(uint), "milleage"),
                                        new Field(new Range(67, 8u), typeof(string), "卡号"),
                                        new Field(new Range(75, 1u), typeof(byte), "报警时主锁状态"),
                                        new Field(new Range(76, 12u), typeof(string), "报警时分锁状态"),
                                    }, msgBytes);

                                    if (LockAlarmed != null)
                                    {
                                        LockAlarmed?.Invoke(mac, fds);
                                    }
                                }

                                #endregion

                                #region 查询锁号上行

                                else if (recCmdId == CommandID.ReadLockNo)
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    byte result = msgBytes[messageIndex += 16];
                                    byte lockid = msgBytes[messageIndex += 1];
                                    string lockno =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex += 1, 6);
                                    if (LockNoReceived != null)
                                    {
                                        LockNoReceived(mac, result == 0, lockid, lockno);
                                    }
                                }

                                #endregion

                                #region 设备巡检

                                else if (recCmdId == CommandID.DevInspectionAck)
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "Mac"),
                                        new Field(new Range(33, 1u), typeof(byte), "CSQ"),
                                        new Field(new Range(34, 1u), typeof(byte), "可见卫星数"),
                                        new Field(new Range(35, 1u), typeof(byte), "可用卫星数"),
                                        new Field(new Range(36, 1u), typeof(byte), "卫星信噪比"),
                                        new Field(new Range(37, 1u), typeof(byte), "UART1外接设备类型"),
                                        new Field(new Range(38, 1u), typeof(byte), "UART1设备状态"),
                                        new Field(new Range(39, 1u), typeof(byte), "UART2外接设备类型"),
                                        new Field(new Range(40, 1u), typeof(byte), "UART2设备状态"),
                                    }, msgBytes);

                                    InspectionReceived.Invoke(mac, fds);
                                }

                                #endregion

                                #region 查询终端参数

                                else if (recCmdId == CommandID.DevParameterAck)
                                {
                                    string mac =
                                        NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    Fields fds = new Fields(new[]
                                    {
                                        new Field(new Range(17, 16u), typeof(string), "Mac"),
                                        new Field(new Range(33, 1u), typeof(byte), "跟踪方式"),
                                        new Field(new Range(34, 2u), typeof(short), "跟踪间隔"),
                                        new Field(new Range(36, msgBytes.Length - 1), typeof(byte[]), "版本"),
                                    }, msgBytes);

                                    ParametersReceived.Invoke(mac, fds);
                                }

                                #endregion

                                #region 终端上传VTR（透传数据）

                                else if (recCmdId == CommandID.UP_VTR_DATA)
                                {
                                    //string mac = NetworkToHostOrderFromBytesToString(msgBytes, messageIndex, 16);
                                    ////指令类型
                                    //short type = NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex += 16);
                                    //string vtrtime = NetworkToHostOrderFromBytesToString(msgBytes, messageIndex += 2, 12);
                                    //short sattus = NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex += 12);
                                    //uint longitude = NetworkToHostOrderFromBytesToUint(msgBytes, messageIndex += 2);
                                    //uint latitude = NetworkToHostOrderFromBytesToUint(msgBytes, messageIndex += 4);
                                    //short speed = NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex += 4);
                                    //short direct = NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex += 2);
                                    //short isLocate = NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex += 2);
                                    //ushort paraLen = NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex += 2);
                                    //string para = NetworkToHostOrderFromBytesToString(msgBytes, messageIndex += 2, paraLen);
                                }

                                #endregion

                                #endregion

                                msgBytes = null;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message != "队列已经空了")
                                {
                                    throw ex;
                                }
                                GC.Collect();
                            }
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                        finally
                        {
                            if (!lastGctime.IsValid())
                                lastGctime = DateTimeHelper.Now;
                            else if ((DateTimeHelper.Now - lastGctime).TotalMinutes > 10) //10分钟清理一次内存
                            {
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                lastGctime = DateTimeHelper.Now;
                            }
                        }
                    }
                }
            }
            else
            {
                _tm_dealrecdata?.Dispose();
                _tm_dealrecdata = null;
            }
        }

        /// <summary>
        /// 寻找完整消息
        /// </summary>
        byte[] FindMessage()
        {
            byte[] msgBytes = null;
            try
            {
                while (_state.BufferQueue.Peek() != '{') //扔掉无效数据
                {
                    byte bt = _state.BufferQueue.Dequeue();
                    if (_state.TempBufferQueue.Count > 0)
                        _state.TempBufferQueue.Enqueue(bt);
                }

                if (_state.TempBufferQueue.Count > 0) //有数据 且接下来是完整数据
                {
                    msgBytes = _state.TempBufferQueue.DequeueAll(); //产生消息数组
                    if (msgBytes.First() != '{' || msgBytes.Last() != '}' || !CheckSum(msgBytes)) //未校验成功
                        msgBytes = null;
                    else
                        return msgBytes;
                }

                while (_state.BufferQueue.Count > 0) //有数据
                {
                    ushort tmpLength = 0;
                    ushort msgLength = 0; //消息长度
                    byte[] lengthbs = new byte[2];
                    do
                    {
                        byte b = _state.BufferQueue.Dequeue(); //出队
                        _state.TempBufferQueue.Enqueue(b); //入临时队列
                        tmpLength++; //取一个消息长度加1

                        //获得消息长度
                        if (tmpLength == 2)
                            lengthbs[0] = b;
                        if (tmpLength == 3)
                        {
                            lengthbs[1] = b;
                            msgLength = NetTools.NetworkToHostOrderFromBytesToUshort(lengthbs, 0);
                        }

                        if (msgLength != 0 && tmpLength >= msgLength + 3) //读取结束
                        {
                            if (b != (byte)'}') //无效数据 丢弃
                            {
                                _state.TempBufferQueue.Clear();
                                break;
                            }
                            else
                            {
                                if (tmpLength < 19) //消息总长至少19位 过滤无效数据
                                {
                                    _state.TempBufferQueue.Clear();
                                    break;
                                }
                                else
                                {
                                    msgBytes = _state.TempBufferQueue.DequeueAll(); //产生消息数组
                                    if (!CheckSum(msgBytes)) //未校验成功
                                    {
                                        msgBytes = null;
                                        break;
                                    }
                                }
                            }
                        }
                    } while (msgBytes == null);

                    if (msgBytes != null) break;
                }
            }
            catch (Exception ex)
            {
                //未找到消息尾巴
                msgBytes = null;
            }
            return msgBytes;
        }


        #endregion

        #region 向服务器发送数据
        /// <summary>
        /// 发送锁，只有当前一个包中的数据全部发送完时，才允许发送下一个包
        /// </summary>
        //private readonly object m_lockobject = new object();
        //ManualResetEvent sendResetEvent = new ManualResetEvent(false);
        private void SendBSCheck(object state)
        {
            if (!_serviceClosed)
            {
                while (true)
                {
                    try
                    {
                        Command cmd = _cmdSendList.FirstOrDefault(x => x.SendStatus == CmdStatus.WaitForSend);
                        if (cmd != null && _loginSucces)
                        {
                            SendBS(cmd.SendBytes, cmd);
                        }
                        else
                        {
                            cmd = _cmdSendList.FirstOrDefault(x => x.AckStatus == CmdStatus.Unknown && (DateTimeHelper.Now - x.SendTime).TotalSeconds >= 15);
                            if (cmd != null)
                            {
                                cmd.AckCode = 60001;
                                InvokeCommandAckHandler(cmd);
                                _cmdSendList.Remove(cmd); //移除超时命令 
                                cmd = null;
                                GC.Collect();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                _tm_sendcheck.Dispose();
                _tm_sendcheck = null;
            }
        }

        #endregion

        #region 事件调用

        public const ushort maxlinklevel = 3;
        bool showlogin = false;

        /// <summary>
        /// 调用命令反馈事件
        /// </summary>
        private void InvokeCommandAckHandler(Command cmd, byte[] extraData = null)
        {
            //连接状态
            ushort linkLevel = maxlinklevel;

            if (cmd == null)
            {   //连接断开直接显示进度条
                _heartErrCount = 0;
                linkLevel = 0;
                _tcpClient.CloseSocket();
                //loginSucces = false;
                showlogin = true;
            }
            else
            {   //心跳失败 3次 重新登录
                if (_heartErrCount >= maxlinklevel)
                {
                    _heartErrCount = 0;
                    linkLevel = 0;
                    _tcpClient.CloseSocket();
                    //loginSucces = false;
                    showlogin = true;
                }
                else
                {
                    if (cmd.AckStatus == CmdStatus.Success) //命令成功
                    {
                        _heartErrCount = 0;
                        linkLevel = maxlinklevel;
                        showlogin = false;

                        if (cmd.CmdId == CommandID.CONNECT) //通信模块连接请求成功
                        {
                            if (!_isMobile)
                                SendLoginRequest(); //登录请求
                            else
                                SendMobileLoginRequest();   //移动用户登录请求
                        }

                        else if (cmd.CmdId == CommandID.USER_LOGIN || cmd.CmdId == CommandID.USER_LOGIN_MOBILE)//登录成功
                        {
                            _loginSucces = true;
                            SendHeartBeat();    //心跳包
                            LoginSuccessed?.Invoke(true);
                        }
                    }
                    else
                    {   //命令失败
                        if (cmd.CmdId == CommandID.KEEP_ALIVE) //链路测试反馈失败
                        {
                            //if (cmd.AckCode == 60000)  //发送失败
                            //{
                            //    heartErrCount = 0;
                            //    linkLevel = 0;
                            //    ClostSocket();
                            //    //loginSucces = false;
                            //    showlogin = true;
                            //}
                            //else
                            {
                                _heartErrCount++;
                                linkLevel = (ushort)(maxlinklevel - _heartErrCount);
                            }
                        }
                        else if (cmd.CmdId == CommandID.CONNECT)  //通信连接请求失败
                        {   //断开重新连接
                            _heartErrCount = 0;
                            linkLevel = 0;
                            _tcpClient.CloseSocket();
                            //loginSucces = false;
                            showlogin = true;
                        }
                    }
                }

                if (CommandAck != null && cmd.CmdId != CommandID.KEEP_ALIVE) //链路测试无需反馈
                    CommandAck(cmd, extraData);
            }

            LinkStateChanged?.Invoke(linkLevel, showlogin);
        }
        #endregion

        /// <summary>
        /// 把Object类型转换成数组
        /// </summary>
        /// <param name="obj"></param>
        byte[] ConvertObjectToBytes(object obj)
        {
            if (obj == null) return new byte[0];

            if (obj is byte)    //byte类型
            {
                return new byte[] { (byte)obj };
            }
            else if (obj is byte[]) //bytes数组
            {
                return (byte[])obj;
            }
            else if (obj is short)  //short类型
            {
                short _obj = (short)obj;
                return NetTools.HostToNetworkOrderToBytes(_obj);
            }
            else if (obj is ushort)  //ushort类型
            {
                ushort _obj = (ushort)obj;
                return NetTools.HostToNetworkOrderToBytes(_obj);
            }
            else if (obj is int)   //int类型
            {
                int _obj = (int)obj;
                return NetTools.HostToNetworkOrderToBytes(_obj);
            }
            else if (obj is uint)   //uint类型
            {
                uint _obj = (uint)obj;
                return NetTools.HostToNetworkOrderToBytes(_obj);
            }
            else if (obj is string) //string类型
            {
                string _obj = (string)obj;
                try
                {
                    string[] array = _obj.Split('_');
                    int _len;
                    if (array.Length > 1)
                    {
                        _obj = array[0];
                        _len = int.Parse(array[1]);
                    }
                    else
                    {
                        _obj = array[0];
                        _len = _obj.Length;
                    }
                    return NetTools.HostToNetworkOrderToBytes(_obj, _len);
                }
                catch { return new byte[0]; }
            }

            return new byte[0];
        }

        /// <summary>
        /// 计算校验值
        /// </summary>
        /// <returns></returns>
        public byte CalcSum(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 2) return 0;

            byte sum = 0;
            int len = bytes.Length;

            for (var i = 0; i < len; i++)
            {
                if (i == 0 || i >= len - 2) continue;
                sum += bytes[i];
            }

            return sum;
        }

        /// <summary>
        /// 检查校验值
        /// </summary>
        /// <returns></returns>
        public bool CheckSum(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 2) return false;
            bool pass = bytes[bytes.Length - 2] == CalcSum(bytes);
            return pass;
        }
    }
}
