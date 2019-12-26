using CommLiby.Cyhk.Models;
using Simple.Common.SocketLib;
using System.Collections.Generic;

namespace Simple.Common.Cyhk
{
    public partial class CyhkTcpClient
    {
        /// <summary>
        /// 发送消息(最大896字节)
        /// </summary>
        /// <param name="type">消息类型 0-其他 1-路况信息 2-通知 3-协查信息</param>
        /// <param name="car"></param>
        public void Send_Msg(ICar car, ushort type, string msg)
        {
            if (car == null || msg == null) return;

            int len = -1;
            byte[] bs = NetTools.HostToNetworkOrderToBytes(msg, -1);
            len = bs.Length;
            SendMessage(CommandID.SND_MSG, car, new object[] { car.MAC.ToSendString(16), type, (ushort)len, bs });
        }

        /// <summary>
        /// 查询车辆位置
        /// </summary>
        /// <param name="car"></param>
        public void Retr(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.RETR, car, new object[] { car.MAC.ToSendString(16) });
        }

        /// <summary>
        /// 设置车辆回传间隔
        /// </summary>
        /// <param name="callType">1-定时 2-定距</param>
        /// <param name="car"></param>
        public void Call_Mobil(ICar car, ushort callType, ushort callPara, ushort number = 0xffff)
        {
            if (car == null) return;
            SendMessage(CommandID.CALL_MOBIL, car, new object[] { car.MAC.ToSendString(16), callType, callPara, number });
        }

        /// <summary>
        /// 取消跟踪
        /// </summary>
        /// <param name="car"></param>
        public void Cancel_Call(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.CANCEL_CALL, car, car.MAC.ToSendString(16));
        }

        /// <summary>
        /// 取消报警
        /// </summary>
        /// <param name="car"></param>
        /// <param name="amarmType">报警类型 0-取消所有报警</param>
        public void Cancel_Alarm(ICar car, ushort alarmType)
        {
            if (car == null) return;
            SendMessage(CommandID.CANCEL_ALARM, car, car.MAC.ToSendString(16), alarmType);
        }

        /// <summary>
        /// 锁车操作
        /// </summary>
        /// <param name="car"></param>
        public void Lock_Mobil(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.LOCK_MOBIL, car, car.MAC.ToSendString(16));
        }

        /// <summary>
        /// 解锁操作
        /// </summary>
        /// <param name="car"></param>
        public void UnLock(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.UNLOCK, car, car.MAC.ToSendString(16));
        }

        /// <summary>
        /// 通油路
        /// </summary>
        /// <param name="car"></param>
        public void Open_GUN(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.OPEN_GUN, car, car.MAC.ToSendString(16));
        }
        /// <summary>
        /// 断油路
        /// </summary>
        /// <param name="car"></param>
        public void Close_GUN(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.CLOSE_GUN, car, car.MAC.ToSendString(16));
        }
        /// <summary>
        /// 设置报警电话或者视频监控
        /// </summary>
        /// <param name="car"></param>
        /// <param name="type">监控类型 0语音监控 1视频监控</param>
        public void SetMonitor(ICar car, string masterTel, string subTel = "", ushort type = 0)
        {
            if (car == null || string.IsNullOrWhiteSpace(masterTel)) return;
            SendMessage(CommandID.SET_MONITOR, car, car.MAC.ToSendString(16), (ushort)1, type, masterTel.ToSendString(16), subTel.ToSendString(10));
        }

        /// <summary>
        /// 设置电话本
        /// </summary>
        /// <param name="phonenums">电话本数组集合</param>
        public void SetPhone(ICar car, params string[] phonenums)
        {
            if (car == null || phonenums == null) return;
            ushort phoneCount = (ushort)phonenums.Length;
            List<byte> bs = new List<byte>();
            foreach (string phonenum in phonenums)
            {
                bs.AddRange(NetTools.HostToNetworkOrderToBytes(phonenum, 16));
            }
            SendMessage(CommandID.SET_PHONE, car, car.MAC.ToSendString(16), phoneCount, bs.ToArray());
        }

        /// <summary>
        /// 定时取图片
        /// </summary>
        /// <param name="car"></param>
        /// <param name="cnlID">通道号 0轮询取</param>
        /// <param name="imgSize">1-320*240 2-640*480</param>
        /// <param name="times">间隔时间</param>
        /// <param name="number">张数</param>
        public void Gather_Image_Timer(ICar car, ushort cnlID, ushort imgSize, ushort times, ushort number)
        {
            if (car == null) return;
            SendMessage(CommandID.GATHER_IMAGE_TIMER, car, car.MAC.ToSendString(16), cnlID, imgSize, times, number);
        }
        /// <summary>
        /// 业务数据请求
        /// </summary>
        /// <param name="car"></param>
        /// <param name="cmd">请求的业务数据类型</param>
        /// <param name="par">参数 0代表停止请求 0xffff代表一直请求 其他代表请求的数据包个数</param>
        public void Data_Request(ICar car, ushort cmd, ushort par)
        {
            if (car == null) return;
            SendMessage(CommandID.DATA_REQUEST, car, car.MAC.ToSendString(16), cmd, par);
        }
        /// <summary>
        /// 图片采集
        /// </summary>
        /// <param name="car"></param>
        /// <param name="imgType">图片质量 1-320*240 2-640*480</param>
        /// <param name="nCount">通道号</param>
        public void Gather_Image(ICar car, ushort imgType, ushort nCount)
        {
            if (car == null) return;
            SendMessage(CommandID.GATHER_IMAGE, car, car.MAC.ToSendString(16), imgType, nCount);
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="car"></param>
        /// <param name="type">0-无类型，直接传输</param>
        /// <param name="len"></param>
        /// <param name="content"></param>
        public void CarSet(ICar car, ushort type, string content)
        {
            if (car == null) return;
            int len = -1;
            if (!string.IsNullOrEmpty(content))
            {
                byte[] bs = NetTools.HostToNetworkOrderToBytes(content, -1);
                len = bs.Length;
                SendMessage(CommandID.CARSET, car, car.MAC.ToSendString(16), type, (ushort)len, bs);
            }
            else
            {
                SendMessage(CommandID.CARSET, car, car.MAC.ToSendString(16), type);
            }

        }
        /// <summary>
        /// 设置区域报警
        /// </summary>
        /// <param name="car"></param>
        /// <param name="areaid">区域ID</param>
        /// <param name="areatype">区域类型 0-矩形 1-多边形 2-圆形 3-线路</param>
        /// <param name="alarmtype">报警类型 0-入区域报警 1-出区域报警 2-出入报警 4-无</param>
        /// <param name="disable">区域状态 是否禁用</param>
        /// <param name="limitSpeed">区域内超速值限制</param>
        /// <param name="openlockNumber">解封卡解封次数</param>
        public void Set_Area_Alarm(ICar car, ushort areaid, ushort areatype, ushort alarmtype, bool disable,
            ushort limitSpeed, byte openlockNumber)
        {
            if (car == null) return;
            ushort areaflag = 0;
            ushort status = disable ? (ushort)0 : (ushort)1;
            ushort mobilareaid = 0;
            ushort pointnum = 0;
            SendMessage(CommandID.SET_AREA_ALARM, car, car.MAC.ToSendString(16), areaid, areaflag, areatype, alarmtype, status, mobilareaid, limitSpeed, pointnum, openlockNumber);
        }
        /// <summary>
        /// 设置线路报警
        /// </summary>
        /// <param name="car"></param>
        /// <param name="areaid">区域ID</param>
        /// <param name="areatype">区域类型 0-矩形 1-多边形 2-圆形 3-线路</param>
        /// <param name="alarmtype">报警类型 0-入区域报警 1-出区域报警 2-出入报警 4-无</param>
        /// <param name="disable">区域状态 是否禁用</param>
        /// <param name="limitSpeed">区域内超速值限制</param>
        /// <param name="openlockNumber">解封卡解封次数</param>
        public void Set_Line_Alarm(ICar car, ushort areaid, ushort areatype, ushort alarmtype, bool disable,
            ushort limitSpeed, byte openlockNumber)
        {
            if (car == null) return;
            ushort areaflag = 0;
            ushort status = disable ? (ushort)0 : (ushort)1;
            ushort mobilareaid = 0;
            ushort pointnum = 0;
            SendMessage(CommandID.SET_LINE_ALARM, car, car.MAC.ToSendString(16), areaid, areaflag, areatype, alarmtype, status, mobilareaid, limitSpeed, pointnum, openlockNumber);
        }
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="areaid">区域ID</param>
        public void Delete_Area(ushort areaid)
        {
            SendMessage(CommandID.DELETE_AREA, null, areaid);
        }
        /// <summary>
        /// 设置终端工作模式
        /// </summary>
        /// <param name="car"></param>
        /// <param name="mode">1-GPS 2-BD 3-混合</param>
        public void Set_Work_Mode(ICar car, byte mode)
        {
            if (car == null) return;
            SendMessage(CommandID.SET_WORK_MODE, car, car.MAC.ToSendString(16), mode);
        }

        /// <summary>
        /// 下发导航点
        /// </summary>
        /// <param name="car"></param>
        /// <param name="map">导航地图 0-东纳 1-美行</param>
        /// <param name="points">导航点个数 一般不超过16个 x-经度 y-纬度</param>
        /// <param name="no">本次导航编号 0-临时 其他需要存储</param>
        /// <param name="pathSelection">路径选择 0-任意 1-导航地图自定 2-最短路径 3-最少收费 4-高速优先</param>
        /// <param name="name">目的地名称 最大86字节</param>
        public void Set_Navi_Point(ICar car, byte map, int no, ushort pathSelection, string name, params Point[] points)
        {
            if (car == null || points == null) return;
            if (string.IsNullOrWhiteSpace(name)) name = "目的地";
            byte pointcount = (byte)points.Length;
            List<byte> pointBytes = new List<byte>();
            foreach (Point point in points)
            {
                pointBytes.AddRange(NetTools.HostToNetworkOrderToBytes((int)(point.X * 1000000)));
                pointBytes.AddRange(NetTools.HostToNetworkOrderToBytes((int)(point.Y * 1000000)));
            }
            SendMessage(CommandID.SET_NAVI_POINT, car, car.MAC.ToSendString(16), map, pointcount, pointBytes.ToArray(), no, pathSelection, name.ToSendString(86));
        }

        /// <summary>
        /// 油罐车绑定分锁命令
        /// </summary>
        /// <param name="car"></param>
        /// <param name="lockid">分锁ID 1-12</param>
        /// <param name="lockNo">锁号</param>
        public void Bind_Part_Lock(ICar car, byte lockid, string lockNo)
        {
            if (car == null || string.IsNullOrWhiteSpace(lockNo)) return;
            SendMessage(CommandID.BIND_PART_LOCK, car, car.MAC.ToSendString(16), lockid, lockNo.ToSendString(6));
        }

        /// <summary>
        /// 油罐车读取分锁命令
        /// </summary>
        /// <param name="car"></param>
        /// <param name="lockid">分锁id 1-12</param>
        public void Read_Part_Lock(ICar car, byte lockid)
        {
            if (car == null) return;
            SendMessage(CommandID.READ_PART_LOCK, car, car.MAC.ToSendString(16), lockid);
        }
        /// <summary>
        /// 油罐车取消绑定分锁命令
        /// </summary>
        /// <param name="car"></param>
        /// <param name="lockid"></param>
        public void Unbind_Part_Lock(ICar car, byte lockid)
        {
            if (car == null) return;
            SendMessage(CommandID.UNBIND_PART_LOCK, car, car.MAC.ToSendString(16), lockid);
        }

        /// <summary>
        /// 锁操作 施封/解封
        /// </summary>
        /// <param name="car"></param>
        /// <param name="cardNo">施解封卡号</param>
        /// <param name="islock">是否是施封操作</param>
        /// <param name="partlocks">操作的分锁号(长度12)</param>
        public void LockOperation(ICar car, string cardNo, bool islock, byte[] partlocks)
        {
            if (car == null) return;
            if (cardNo == null) cardNo = "";
            byte operation = islock ? (byte)0x01 : (byte)0x00;

            SendMessage(CommandID.LOCKOPERATION, car, car.MAC.ToSendString(16), cardNo.ToSendString(8), operation, partlocks);
        }

        /// <summary>
        /// 锁控工作模式设定
        /// </summary>
        /// <param name="car"></param>
        /// <param name="interval">锁状态回传间隔</param>
        /// <param name="samecardinterval">相同刷卡间隔</param>
        /// <param name="locktimeout">锁超时间隔</param>
        public void LockSetting(ICar car, short interval, short samecardinterval, short locktimeout)
        {
            if (car == null) return;
            SendMessage(CommandID.LOCKSETTING, car, car.MAC.ToSendString(16), interval, samecardinterval, locktimeout);
        }

        /// <summary>
        /// 施解封卡绑定操作
        /// </summary>
        /// <param name="car"></param>
        /// <param name="card">卡号</param>
        /// <param name="cardType">卡类型 0解封卡 1施封卡 2超级卡</param>
        /// <param name="isbind">true绑定 false解绑</param>
        public void LockCardBind(ICar car, string card, byte cardType, bool isbind)
        {
            if (car == null) return;

            byte id = 2;        // 卡相对中控的ID 0-4
            byte opter = 2;         //卡类型 1解封卡 2施封卡
            byte[] limits = new byte[12];   //分锁权限 0x30解封权限 0x31施封权限 0x00清除权限 0xff保持不变
            if (cardType == 0)
            {   //解封卡
                id = 0;
                opter = 1;
                if (isbind)
                    for (var i = 0; i < limits.Length; i++)
                    {
                        limits[i] = 0x30;
                    }
                else
                {
                    opter = 0;
                }
            }
            if (cardType == 1)
            {
                //施封卡 暂时无法解除
                id = 1;
                opter = 0;
                if (isbind)
                    for (var i = 0; i < limits.Length; i++)
                    {
                        limits[i] = 0x31;
                    }
                else
                {
                    opter = 1;
                }
            }
            if (cardType == 2)
            {
                //超级卡
                id = 4;
                opter = 1;
                if (isbind)
                    for (var i = 0; i < limits.Length; i++)
                    {
                        limits[i] = 0x30;
                    }
                else
                {
                    opter = 0;
                }
            }

            SendMessage(CommandID.LOCKCARDBIND, car, car.MAC.ToSendString(16), id, card.ToSendString(8), opter, limits);
        }

        /// <summary>
        /// 绑定油箱参数
        /// </summary>
        /// <param name="car"></param>
        /// <param name="oilID">油耗参数ID</param>
        /// <param name="oilboxId">油箱序列号 目前0-3</param>
        public void Bind_OilConsume(ICar car, uint oilID, byte oilboxId)
        {
            if (car == null)
            {
                return;
            }

            SendMessage(CommandID.Bind_OilConsume, car, car.MAC.ToSendString(16), oilID, oilboxId);
        }

        /// <summary>
        /// 绑定油箱参数 (旧版 只支持1个油箱)
        /// </summary>
        /// <param name="car"></param>
        /// <param name="oilID">油耗参数ID</param>
        public void Bind_OilConsume_old(ICar car, uint oilID)
        {
            if (car == null)
            {
                return;
            }

            SendMessage(CommandID.Bind_OilConsume_old, car, car.MAC.ToSendString(16), oilID);
        }

        /// <summary>
        /// 设置三急报警
        /// </summary>
        /// <param name="car"></param>
        /// <param name="x">X轴加速度阈值</param>
        /// <param name="y">Y轴加速度阈值</param>
        /// <param name="z">Z轴加速度阈值</param>
        public void Set3jiAlarm(ICar car, short x, short y, short z)
        {
            if (car == null) return;
            SendMessage(CommandID.Set3jiAlarm, car, car.MAC.ToSendString(16), x, y, z);
        }

        /// <summary>
        /// 终端巡检
        /// </summary>
        /// <param name="car"></param>
        public void DevInspection(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.DevInspection, car, car.MAC.ToSendString(16));
        }
        /// <summary>
        /// 查询终端参数
        /// </summary>
        /// <param name="car"></param>
        public void DevParameter(ICar car)
        {
            if (car == null) return;
            SendMessage(CommandID.DevParameter, car, car.MAC.ToSendString(16));
        }

        /// <summary>
        /// 发送供水应急消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="netType"></param>
        public void SendGSYJMsg(ICar car, string msg, string userName, byte netType = 0)
        {
            /// (car == null || msg == null) return;


            if (car == null || msg == null) return;
            string head = "S01" + "01" + "01" + "!";
            string content = head + userName + '&' + msg + "###";
            int length = GbUnicode.GetGB2312Array(content).Length;
            if (length > 896) length = 896;

            SendMessage(CommandID.DOWN_MSG_GSYJ, car, new object[] { car.MAC.ToSendString(16), (ushort)length, content.ToSendString(length) });

            //string head = netType + "01" + "01" + "01" + "!";
            //string content = head + userName + '&' + msg + "###";

            //int len = -1;
            //byte[] bs = NetTools.HostToNetworkOrderToBytes(content, -1);
            //len = bs.Length;
            //SendMessage(CommandID.DOWN_MSG_GSYJ, car, new object[] { car.MAC.ToSendString(16), (short)len, bs });
        }
    }

    public static class ExtensionsClass
    {
        /// <summary>
        /// 转换为网络发送字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToSendString(this string str, int length)
        {
            return string.Format("{0}_{1}", str, length);
        }
    }
}
