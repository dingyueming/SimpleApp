using System;

namespace Simple.Entity
{
    public class AUTHEntity
    {
        /// <summary>
        /// 用户号
        /// </summary>
        public uint USERID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string USERNAME { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PASSWD { get; set; }
        /// <summary>
        /// 用户描述
        /// </summary>
        public string UDESC { get; set; }
        /// <summary>
        /// 用户状态 0：停用 其他启用
        /// </summary>
        public int USTATUS { get; set; } = 1;
        /// <summary>
        /// 所属用户组         auth_group
        /// </summary>
        public int GROUPID { get; set; }
        /// <summary>
        /// 所属分中心 0 ：为总中心        sub_center
        /// </summary>
        public int CENTERID { get; set; }
        /// <summary>
        /// 记录人
        /// </summary>
        public uint RECORDMAN { get; set; }
        /// <summary>
        /// 记录日期
        /// </summary>
        public DateTime RECORDDATE { get; set; }
        /// <summary>
        /// 车辆分组方式： 0：按用户分组 ，1：单位分组 2：按中心分组
        /// </summary>
        public int GROUPSTYPE { get; set; } = 1;
        /// <summary>
        /// 车辆名称显示方式：0:车牌号，1：车辆编号
        /// </summary>
        public int CARDISPLAYSTYPE { get; set; }
        /// <summary>
        /// 最小呼叫间隔时间，单位：秒
        /// </summary>
        public ushort MINCALLTIME { get; set; } = 20;
        /// <summary>
        /// 单位id,单位坐席员
        /// </summary>
        public int UNITID { get; set; } = -100;
        /// <summary>
        /// 绑定查询车辆的手机号
        /// </summary>
        public string LOOKUPSIM { get; set; }
        /// <summary>
        /// 地图 中心经度
        /// </summary>
        public double CENTER_LO { get; set; } = 116;
        /// <summary>
        /// 地图 中心纬度
        /// </summary>
        public double CENTER_LA { get; set; } = 40;
        /// <summary>
        /// 初始级别
        /// </summary>
        public double MAP_LEVEL { get; set; } = 5;
        /// <summary>
        /// 姓名
        /// </summary>
        public string PERSONAL_NAME { get; set; }
        /// <summary>
        /// 未知字段
        /// </summary>
        public int MONITORAREA { get; set; }
        /// <summary>
        /// 0,无登陆，1，登陆成功，2，正在登陆，等待确认，4，被拒绝登陆
        /// </summary>
        public int LOGIN_STATE { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LOGIN_TIME { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LOGIN_IP { get; set; }
        /// <summary>
        /// 绑定的IP，只能从这个IP上登陆
        /// </summary>
        public string BIND_IP { get; set; }
        /// <summary>
        /// 自动监控该单位下的车辆 0-手动 1-自动
        /// </summary>
        public bool AUTO_MONITOR { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LOGIN_COUNT { get; set; }
    }
}
