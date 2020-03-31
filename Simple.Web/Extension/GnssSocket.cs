using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Simple.Common;
using Simple.Common.Cyhk;
using Simple.Common.SocketLib;
using Simple.ExEntity.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Other
{
    /// <summary>
    /// 搁置，暂时不用
    /// </summary>
    public class GnssSocket
    {
        private IConfiguration configuration;
        private readonly CyhkTcpClient tcpClient;
        private readonly IHubContext<MapHub> hubContext;
        public GnssSocket(IConfiguration configuration, IHubContext<MapHub> hubContext)
        {
            this.configuration = configuration;
            this.hubContext = hubContext;
            tcpClient = new CyhkTcpClient();
            tcpClient.InitTcpClient(new TCPClient());
            tcpClient.GpsDataReceived += TcpClient_GpsDataReceived;
            tcpClient.LoginSuccessed += TcpClient_LoginSuccessed;
            tcpClient.InitCompleted += TcpClient_InitCompleted;
        }

        public IClientProxy ClientProxy
        {
            get
            {
                return hubContext.Clients.User(tcpClient.LoginUserID.ToString());
            }
        }

        private void TcpClient_LoginSuccessed(bool suc)
        {
            if (!suc)
            {
                ClientProxy.SendAsync("LoginOut");
            }
        }

        public void GpsDataUpdated(NEWTRACK data)
        {
            CarStatus carStatus = new CarStatus();
            carStatus.RefreshStatus(data.STATUS, data.STOPTIME);
            data.StatusShow = carStatus.ToString();
            ClientProxy.SendAsync("ReceiveGpsData", data);
        }

        private void TcpClient_GpsDataReceived(string mac, Fields fds)
        {
            if (string.IsNullOrWhiteSpace(mac) || fds == null) return;

            var data = new NEWTRACK() { MAC = mac };

            ushort heading = fds.GetFieldValue<ushort>("Direct");
            heading = (ushort)Math.Round(heading / 10.0);
            uint status = fds.GetFieldValue<uint>("Status");
            uint statusEx = (uint)fds.GetFieldValue("StatusEx");
            uint alarmStatus = (uint)fds.GetFieldValue("AlarmStatus");
            uint alarmStatusEx = (uint)fds.GetFieldValue("AlarmStatusEx");
            byte locate = (byte)fds.GetFieldValue("IsLocate");
            byte locatemode = (byte)fds.GetFieldValue("locatemode");
            string timeStr = (string)fds.GetFieldValue("GpsTime");
            DateTime gnsstime = DateTimeHelper.StringToDateTime(timeStr, "yyMMddHHmmss");
            uint ulo = (uint)fds.GetFieldValue("Longitude");
            double lo = ulo / 1000000.0;
            uint ula = (uint)fds.GetFieldValue("Latitude");
            double la = ula / 1000000.0;
            ushort altitude = fds.GetFieldValue<ushort>("High");
            ushort uspeed = (ushort)fds.GetFieldValue("Speed");
            float speed = (float)Math.Round(uspeed / 10.0, 1);
            uint mileage = (uint)fds.GetFieldValue("Mileage");
            byte haveoil = (byte)fds.GetFieldValue("Haveoil");
            ushort oil1 = (ushort)fds.GetFieldValue("Oil1");
            ushort oil2 = (ushort)fds.GetFieldValue("Oil2");
            ushort oil3 = (ushort)fds.GetFieldValue("Oil3");
            ushort oil4 = (ushort)fds.GetFieldValue("Oil4");

            ushort additemcount = (ushort)fds.GetFieldValue("AdditionItem");
            byte[] msgBytes = (byte[])fds.GetFieldValue("AdditionBytes");
            int messageIndex = 0;

            try
            {
                #region 附加信息处理

                #region 附加信息变量
                string position = string.Empty;
                short loadjust = 0, laadjust = 0;
                ushort sensor1 = 0, sensor2 = 0, sensor3 = 0, sensor4 = 0;
                ushort stoptime = 0;
                short tmp = 0;
                int areaid = 0;

                short firestate; //消防状态
                ushort menalarmid; //需要人工确认的报警事件ID
                byte overspeedtype; //超速 位置类型 0：无特定位置； 1：圆形区域； 2：矩形区域； 3：多边形区域； 4：路段 
                int overspeedareaid; //超速 区域或路段ID
                byte linealarmtype; //进出区域/路线报警 位置类型 1：圆型区域； 2：矩形区域； 3：多边形区域； 4：路线 
                int linealarmareaid; //进出区域/路线报警 区域或线路ID
                byte linealarmdirect; //进出区域/路线报警 0：进； 1：出
                int roadid; //路线行驶时间不足/过长报警  路段ID 
                ushort roadruntime; //路线行驶时间不足/过长报警 路段行驶时间 秒
                byte roadruntype; //路线行驶时间不足/过长报警 0：不足；1：过长
                byte singallevel; //无线通信信号强度
                byte satellitenum; //定位卫星个数
                byte oilalarmtype; //油量异常报警 0 加油 1 漏油
                short oilalarmvalue; //油量异常报警 加油或漏油升数
                byte onoffvalue; //开关量 每位代表一个开关量 0 为开 1为合

                #endregion

                byte addlength = 0; // 附属信息长度字节数
                for (ushort i = 0; i < additemcount; i++)
                {
                    byte id = msgBytes[messageIndex += addlength];
                    addlength = msgBytes[messageIndex += 1];
                    messageIndex += 1;
                    switch (id)
                    {
                        case 0x01: //位置描述信息
                            position = NetTools.NetworkToHostOrderFromBytesToString(msgBytes, messageIndex,
                                addlength);
                            break;
                        case 0x02: //经纬度偏移量
                            loadjust = NetTools.NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex);
                            laadjust = NetTools.NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex + 2);
                            break;
                        case 0x03: //消防状态
                            firestate = NetTools.NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex);
                            break;
                        case 0x04: //需要人工确认报警事件的ID，WORD，从1开始计数
                            menalarmid = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex);
                            break;
                        case 0x11: //超速报警附加信息见附表6
                            overspeedtype = msgBytes[messageIndex];
                            if (overspeedtype != 0)
                                overspeedareaid = NetTools.NetworkToHostOrderFromBytesToInt(msgBytes,
                                    messageIndex + 1);
                            break;
                        case 0x12: //进出区域/路线报警附加信息见附表7
                            linealarmtype = msgBytes[messageIndex];
                            linealarmareaid = NetTools.NetworkToHostOrderFromBytesToInt(msgBytes,
                                messageIndex + 1);
                            linealarmdirect = msgBytes[messageIndex + 5];
                            break;
                        case 0x13: //路段行驶时间不足/过长报警附加信息见附表8 
                            roadid = NetTools.NetworkToHostOrderFromBytesToInt(msgBytes, messageIndex);
                            roadruntime = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex + 4);
                            roadruntype = msgBytes[messageIndex + 6];
                            break;
                        case 0x2B: //模拟量
                            sensor1 = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex);
                            sensor2 = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex + 2);
                            sensor3 = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex + 4);
                            sensor4 = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex + 6);
                            break;
                        case 0x30: //BYTE，无线通信网络信号强度
                            singallevel = msgBytes[messageIndex];
                            break;
                        case 0x31: //BYTE，GNSS定位卫星数
                            satellitenum = msgBytes[messageIndex];
                            break;
                        case 0x32: //停车时间
                            stoptime = NetTools.NetworkToHostOrderFromBytesToUshort(msgBytes, messageIndex);
                            break;
                        case 0x33: //温度
                            tmp = NetTools.NetworkToHostOrderFromBytesToShort(msgBytes, messageIndex);
                            break;
                        case 0x34: //区域ID
                            areaid = NetTools.NetworkToHostOrderFromBytesToInt(msgBytes, messageIndex);
                            break;
                        case 0x35: //油量异常报警 见附表9
                            oilalarmtype = msgBytes[messageIndex];
                            oilalarmvalue = NetTools.NetworkToHostOrderFromBytesToShort(msgBytes,
                                messageIndex + 1);
                            break;
                        case 0x36: //开关量 每位代表一个开关量 0 为开 1为合
                            onoffvalue = msgBytes[messageIndex];
                            break;
                    }
                }

                #endregion

                data.UpdateData(heading, (int)status, (int)statusEx, (int)alarmStatus, (int)alarmStatusEx, locate,
                    locatemode, gnsstime, lo, la, altitude, speed, (int)mileage, haveoil, oil1, oil2,
                    oil3, oil4, sensor1, sensor2, sensor3, sensor4, position, loadjust, laadjust,
                    stoptime, tmp, areaid);

                GpsDataUpdated(data);

            }
            catch (Exception)
            {

            }
        }

        private void TcpClient_InitCompleted(bool obj)
        {
            if (obj)
            {
                tcpClient.StartService();//启动服务
            }
        }

        public void GnssLogin(string uid, string pwd)
        {
            tcpClient.Init(configuration["ServerIp"], int.Parse(configuration["ServerPort"]), uid, pwd);
        }

    }
}
