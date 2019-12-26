
namespace Simple.Common.Cyhk
{
    /// <summary>
    /// 命令号类
    /// </summary>
    public static class CommandID
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        public const ushort USER_LOGIN = 1;
        /// <summary>
        /// 移动用户登录
        /// </summary>
        public const ushort USER_LOGIN_MOBILE = 601;
        /// <summary>
        /// 用户离线
        /// </summary>
        public const ushort USER_LOGOUT = 2;
        /// <summary>
        /// 通信模块连接请求
        /// </summary>
        public const ushort CONNECT = 90;
        /// <summary>
        /// 链路测试
        /// </summary>
        public const ushort KEEP_ALIVE = 100;
        /// <summary>
        /// 命令反馈
        /// </summary>
        public const ushort CMD_ACK = 101;
        /// <summary>
        /// GPS实时数据
        /// </summary>
        public const ushort GPS_PACKET = 3000;
        /// <summary>
        /// 锁状态GPS数据包
        /// </summary>
        public const ushort LOCKGPS_PACKET = 2001;
        /// <summary>
        /// 发送消息
        /// </summary>
        public const ushort SND_MSG = 4;
        /// <summary>
        /// 位置查询
        /// </summary>
        public const ushort RETR = 5;
        /// <summary>
        /// 车辆监控
        /// </summary>
        public const ushort CALL_MOBIL = 6;
        /// <summary>
        /// 停止监控
        /// </summary>
        public const ushort CANCEL_CALL = 7;
        /// <summary>
        /// 取消报警
        /// </summary>
        public const ushort CANCEL_ALARM = 17;
        /// <summary>
        /// 锁车操作
        /// </summary>
        public const ushort LOCK_MOBIL = 8;
        /// <summary>
        /// 解锁操作
        /// </summary>
        public const ushort UNLOCK = 9;
        /// <summary>
        /// 通油路
        /// </summary>
        public const ushort OPEN_GUN = 10;
        /// <summary>
        /// 断油路 
        /// </summary>
        public const ushort CLOSE_GUN = 11;
        /// <summary>
        /// 设置报警电话或视频监控
        /// </summary>
        public const ushort SET_MONITOR = 12;
        /// <summary>
        /// 定时取图片
        /// </summary>
        public const ushort GATHER_IMAGE_TIMER = 13;
        /// <summary>
        /// 业务数据请求
        /// </summary>
        public const ushort DATA_REQUEST = 15;
        /// <summary>
        /// 图像采集
        /// </summary>
        public const ushort GATHER_IMAGE = 18;
        /// <summary>
        /// 车载终端参数设置 
        /// </summary>
        public const ushort CARSET = 19;
        /// <summary>
        /// 设置区域报警
        /// </summary>
        public const ushort SET_AREA_ALARM = 21;
        /// <summary>
        /// 设置线路报警
        /// </summary>
        public const ushort SET_LINE_ALARM = 22;
        /// <summary>
        /// 设置电话本
        /// </summary>
        public const ushort SET_PHONE = 25;
        /// <summary>
        /// 删除区域
        /// </summary>
        public const ushort DELETE_AREA = 103;
        /// <summary>
        /// 设置终端工作模式
        /// </summary>
        public const ushort SET_WORK_MODE = 34;
        /// <summary>
        /// 下发导航点
        /// </summary>
        public const ushort SET_NAVI_POINT = 36;
        /// <summary>
        /// 绑定分锁
        /// </summary>
        public const ushort BIND_PART_LOCK = 401;
        /// <summary>
        /// 读取分锁
        /// </summary>
        public const ushort READ_PART_LOCK = 403;
        /// <summary>
        /// 取消绑定分锁
        /// </summary>
        public const ushort UNBIND_PART_LOCK = 402;
        /// <summary>
        /// 锁操作 施封/解封
        /// </summary>
        public const ushort LOCKOPERATION = 404;
        /// <summary>
        /// 解/施封卡权限操作
        /// </summary>
        public const ushort LOCKCARDBIND = 405;
        /// <summary>
        /// 锁控工作模式设定
        /// </summary>
        public const ushort LOCKSETTING = 406;
        /// <summary>
        /// 报警消息(已经废弃 从GPS信息里读取)
        /// </summary>
        public const ushort AlarmInfo = 1003;
        /// <summary>
        /// 上发消息 
        /// </summary>
        public const ushort UP_MSG = 1002;
        /// <summary>
        /// 图像消息传输
        /// </summary>
        public const ushort ImageInfo = 1006;
        /// <summary>
        /// 注册卡刷卡及中心施解封操作记录
        /// </summary>
        public const ushort LCV_ACTION = 2002;
        /// <summary>
        /// 非注册卡刷卡操作
        /// </summary>
        public const ushort UnRegCard = 2003;
        /// <summary>
        /// 锁控报警
        /// </summary>
        public const ushort LCV_ALARM = 2005;
        /// <summary>
        /// 查询锁号
        /// </summary>
        public const ushort ReadLockNo = 2006;
        /// <summary>
        /// 绑定油箱参数
        /// </summary>
        public const ushort Bind_OilConsume = 202;
        /// <summary>
        /// 绑定油箱参数旧（油控宝）
        /// </summary>
        public const ushort Bind_OilConsume_old = 201;
        /// <summary>
        /// 设置三急报警
        /// </summary>
        public const ushort Set3jiAlarm = 56;
        /// <summary>
        /// 终端巡检
        /// </summary>
        public const ushort DevInspection = 57;
        /// <summary>
        /// 终端巡检数据反馈
        /// </summary>
        public const ushort DevInspectionAck = 1057;
        /// <summary>
        /// 查询终端参数
        /// </summary>
        public const ushort DevParameter = 58;
        /// <summary>
        /// 查询终端参数反馈
        /// </summary>
        public const ushort DevParameterAck = 1058;
        /// <summary>
        /// 终端上传VTR数据（透传）
        /// </summary>
        public const ushort UP_VTR_DATA = 1017;
        /// <summary>
        /// 供水应急上报消息
        /// </summary>
        public const ushort DOWN_MSG_GSYJ = 600;
        /// <summary>
        /// 供水应急上报消息
        /// </summary>
        public const ushort UP_MSG_GSYJ = 2600;
    }
}
