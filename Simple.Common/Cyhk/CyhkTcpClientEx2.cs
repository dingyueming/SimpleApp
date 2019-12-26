using CommLiby.Cyhk.Models;

namespace Simple.Common.Cyhk
{
    public static class CyhkTcpClientEx2
    {
        /// <summary>
        /// 发送消息(最大896字节)
        /// </summary>
        /// <param name="type">消息类型 0-其他 1-路况信息 2-通知 3-协查信息</param>
        /// <param name="car"></param>
        public static void Send_Msg(this ICar car, ushort type, string msg)
        {
            CyhkTcpClient.Instance.Send_Msg(car, type, msg);
        }

        /// <summary>
        /// 查询车辆位置
        /// </summary>
        /// <param name="car"></param>
        public static void Retr(this ICar car)
        {
            CyhkTcpClient.Instance.Retr(car);
        }

        /// <summary>
        /// 设置车辆回传间隔
        /// </summary>
        /// <param name="callType">1-定时 2-定距</param>
        /// <param name="car"></param>
        public static void Call_Mobil(this ICar car, ushort callType, ushort callPara, ushort number = 0xffff)
        {
            CyhkTcpClient.Instance.Call_Mobil(car, callType, callPara, number);
        }

        /// <summary>
        /// 取消跟踪
        /// </summary>
        /// <param name="car"></param>
        public static void Cancel_Call(this ICar car)
        {
            CyhkTcpClient.Instance.Cancel_Call(car);
        }

        /// <summary>
        /// 取消报警
        /// </summary>
        /// <param name="car"></param>
        /// <param name="amarmType">报警类型 0-取消所有报警</param>
        public static void Cancel_Alarm(this ICar car, ushort alarmType)
        {
            CyhkTcpClient.Instance.Cancel_Alarm(car, alarmType);
        }

        /// <summary>
        /// 锁车操作
        /// </summary>
        /// <param name="car"></param>
        public static void Lock_Mobil(this ICar car)
        {
            CyhkTcpClient.Instance.Lock_Mobil(car);
        }

        /// <summary>
        /// 解锁操作
        /// </summary>
        /// <param name="car"></param>
        public static void UnLock(this ICar car)
        {
            CyhkTcpClient.Instance.UnLock(car);
        }

        /// <summary>
        /// 通油路
        /// </summary>
        /// <param name="car"></param>
        public static void Open_GUN(this ICar car)
        {
            CyhkTcpClient.Instance.Open_GUN(car);
        }
        /// <summary>
        /// 断油路
        /// </summary>
        /// <param name="car"></param>
        public static void Close_GUN(this ICar car)
        {
            CyhkTcpClient.Instance.Close_GUN(car);
        }
        /// <summary>
        /// 设置报警电话或者视频监控
        /// </summary>
        /// <param name="car"></param>
        /// <param name="type">监控类型 0语音监控 1视频监控</param>
        public static void SetMonitor(this ICar car, string masterTel, string subTel = "", ushort type = 0)
        {
            CyhkTcpClient.Instance.SetMonitor(car, masterTel, subTel, type);
        }

        /// <summary>
        /// 设置电话本
        /// </summary>
        /// <param name="phonenums">电话本数组集合</param>
        public static void SetPhone(this ICar car, params string[] phonenums)
        {
            CyhkTcpClient.Instance.SetPhone(car, phonenums);
        }
        /// <summary>
        /// 定时取图片
        /// </summary>
        /// <param name="car"></param>
        /// <param name="cnlID">通道号 0轮询取</param>
        /// <param name="imgSize">1-320*240 2-640*480</param>
        /// <param name="times">间隔时间</param>
        /// <param name="number">张数</param>
        public static void Gather_Image_Timer(this ICar car, ushort cnlID, ushort imgSize, ushort times, ushort number)
        {
            CyhkTcpClient.Instance.Gather_Image_Timer(car, cnlID, imgSize, times, number);
        }
        /// <summary>
        /// 业务数据请求
        /// </summary>
        /// <param name="car"></param>
        /// <param name="cmd">请求的业务数据类型</param>
        /// <param name="par">参数 0代表停止请求 0xffff代表一直请求 其他代表请求的数据包个数</param>
        public static void Data_Request(this ICar car, ushort cmd, ushort par)
        {
            CyhkTcpClient.Instance.Data_Request(car, cmd, par);
        }
        /// <summary>
        /// 图片采集
        /// </summary>
        /// <param name="car"></param>
        /// <param name="imgType">图片质量 1-320*240 2-640*480</param>
        /// <param name="nCount">通道号</param>
        public static void Gather_Image(this ICar car, ushort imgType, ushort nCount)
        {
            CyhkTcpClient.Instance.Gather_Image(car, imgType, nCount);
        }
        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="car"></param>
        /// <param name="type">0-无类型，直接传输</param>
        /// <param name="len"></param>
        /// <param name="content"></param>
        public static void CarSet(this ICar car, ushort type, string content)
        {
            CyhkTcpClient.Instance.CarSet(car, type, content);
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
        public static void Set_Area_Alarm(this ICar car, ushort areaid, ushort areatype, ushort alarmtype, bool disable,
            ushort limitSpeed, byte openlockNumber)
        {
            CyhkTcpClient.Instance.Set_Area_Alarm(car, areaid, areatype, alarmtype, disable, limitSpeed, openlockNumber);
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
        public static void Set_Line_Alarm(this ICar car, ushort areaid, ushort areatype, ushort alarmtype, bool disable,
            ushort limitSpeed, byte openlockNumber)
        {
            CyhkTcpClient.Instance.Set_Line_Alarm(car, areaid, areatype, alarmtype, disable, limitSpeed, openlockNumber);
        }
        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="areaid">区域ID</param>
        public static void Delete_Area(ushort areaid)
        {
            CyhkTcpClient.Instance.Delete_Area(areaid);
        }
        /// <summary>
        /// 设置终端工作模式
        /// </summary>
        /// <param name="car"></param>
        /// <param name="mode">1-GPS 2-BD 3-混合</param>
        public static void Set_Work_Mode(this ICar car, byte mode)
        {
            CyhkTcpClient.Instance.Set_Work_Mode(car, mode);
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
        public static void Set_Navi_Point(this ICar car, byte map, int no, ushort pathSelection, string name, params Point[] points)
        {
            CyhkTcpClient.Instance.Set_Navi_Point(car, map, no, pathSelection, name, points);
        }

        /// <summary>
        /// 油罐车绑定分锁命令
        /// </summary>
        /// <param name="car"></param>
        /// <param name="lockid">分锁ID 1-12</param>
        /// <param name="lockNo">锁号</param>
        public static void Bind_Part_Lock(this ICar car, byte lockid, string lockNo)
        {
            CyhkTcpClient.Instance.Bind_Part_Lock(car, lockid, lockNo);
        }

        /// <summary>
        /// 油罐车读取分锁命令
        /// </summary>
        /// <param name="car"></param>
        /// <param name="lockid">分锁id 1-12</param>
        public static void Read_Part_Lock(this ICar car, byte lockid)
        {
            CyhkTcpClient.Instance.Read_Part_Lock(car, lockid);
        }
        /// <summary>
        /// 油罐车取消绑定分锁命令
        /// </summary>
        /// <param name="car"></param>
        /// <param name="lockid"></param>
        public static void Unbind_Part_Lock(this ICar car, byte lockid)
        {
            CyhkTcpClient.Instance.Unbind_Part_Lock(car, lockid);
        }

        /// <summary>
        /// 锁操作 施封/解封
        /// </summary>
        /// <param name="car"></param>
        /// <param name="cardNo">施解封卡号</param>
        /// <param name="islock">是否是施封操作</param>
        /// <param name="partlockno">操作的分锁号</param>
        public static void LockOperation(this ICar car, string cardNo, bool islock, params byte[] partlockno)
        {
            CyhkTcpClient.Instance.LockOperation(car, cardNo, islock, partlockno);
        }

        /// <summary>
        /// 锁控工作模式设定
        /// </summary>
        /// <param name="car"></param>
        /// <param name="interval">锁状态回传间隔</param>
        /// <param name="samecardinterval">相同刷卡间隔</param>
        /// <param name="locktimeout">锁超时间隔</param>
        public static void LockSetting(this ICar car, short interval, short samecardinterval, short locktimeout)
        {
            CyhkTcpClient.Instance.LockSetting(car, interval, samecardinterval, locktimeout);
        }

        /// <summary>
        /// 施解封卡绑定操作
        /// </summary>
        /// <param name="car"></param>
        /// <param name="card">卡号</param>
        /// <param name="cardType">卡类型 0解封卡 1施封卡 2超级卡</param>
        /// <param name="isbind">true绑定 false解绑</param>
        public static void LockCardBind(this ICar car, string card, byte cardType, bool isbind)
        {
            CyhkTcpClient.Instance.LockCardBind(car, card, cardType, isbind);
        }

        /// <summary>
        /// 绑定油箱参数
        /// </summary>
        /// <param name="car"></param>
        /// <param name="oilID">油耗参数ID</param>
        /// <param name="oilboxId">油箱序列号 目前0-3</param>
        public static void Bind_OilConsume(this ICar car, uint oilID, byte oilboxId)
        {
            CyhkTcpClient.Instance.Bind_OilConsume(car, oilID, oilboxId);
        }

        /// <summary>
        /// 绑定油箱参数 (旧版 只支持1个油箱)
        /// </summary>
        /// <param name="car"></param>
        /// <param name="oilID">油耗参数ID</param>
        public static void Bind_OilConsume_old(this ICar car, uint oilID)
        {
            CyhkTcpClient.Instance.Bind_OilConsume_old(car, oilID);
        }

        /// <summary>
        /// 设置三急报警
        /// </summary>
        /// <param name="car"></param>
        /// <param name="x">X轴加速度阈值</param>
        /// <param name="y">Y轴加速度阈值</param>
        /// <param name="z">Z轴加速度阈值</param>
        public static void Set3jiAlarm(this ICar car, short x, short y, short z)
        {
            CyhkTcpClient.Instance.Set3jiAlarm(car, x, y, z);
        }

        /// <summary>
        /// 终端巡检
        /// </summary>
        /// <param name="car"></param>
        public static void DevInspection(this ICar car)
        {
            if (car == null) return;
            CyhkTcpClient.Instance.DevInspection(car);
        }
        /// <summary>
        /// 查询终端参数
        /// </summary>
        /// <param name="car"></param>
        public static void DevParameter(this ICar car)
        {
            if (car == null) return;
            CyhkTcpClient.Instance.DevParameter(car);
        }

        /// <summary>
        /// 发送供水应急消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="netType"></param>
        public static void SendGSYJMsg(this ICar car, string msg,string usreName, byte netType = 0)
        {
            if (car == null) return;
            CyhkTcpClient.Instance.SendGSYJMsg(car, msg,usreName, netType);
        }


    }
}
