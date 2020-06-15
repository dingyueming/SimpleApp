using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("CARS")]
    public class CarEntity
    {
        #region Model
        /// <summary>
        /// 车辆ID号
        /// </summary>
        [Key]
        public virtual int CARID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public virtual string LICENSE { get; set; }
        /// <summary>
        /// 单位ID号          unit
        /// </summary>
        public virtual int UNITID { get; set; }
        /// <summary>
        /// 车台通讯码      手机号
        /// </summary>
        public virtual string MAC { get; set; }
        /// <summary>
        /// 车台类型  mobil_type
        /// </summary>
        public virtual int MTYPE { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public virtual int CTYPE { get; set; }
        /// <summary>
        /// 车辆类型，比如：富康，捷达等            dict_cartypes
        /// </summary>
        public virtual int CARTYPE { get; set; }
        /// <summary>
        /// 车辆用途1公务车，2水罐车，3非水罐车
        /// </summary>
        public virtual int USAGE { get; set; }
        /// <summary>
        /// 车辆颜色                                         dict_color
        /// </summary>
        public virtual int COLOR { get; set; }
        /// <summary>
        /// 车辆描述
        /// </summary>
        public virtual string CARDESC { get; set; }
        /// <summary>
        /// 投用时间
        /// </summary>
        public virtual DateTime USETIME { get; set; }
        /// <summary>
        /// 入网时间
        /// </summary>
        public virtual DateTime REGTIME { get; set; }
        /// <summary>
        /// 所属分中心 0：为总中心                  sub_center
        /// </summary>
        public virtual int CENTERID { get; set; }
        /// <summary>
        /// 载重量单位为吨
        /// </summary>
        public virtual double SOTRAGE { get; set; }
        /// <summary>
        /// 调度状态 1，请载 2，任务执行中 0：非请载也非任务执行
        /// </summary>
        public virtual int ATTEMPSTATUS { get; set; }
        /// <summary>
        /// 录入人        manager
        /// </summary>
        public virtual int RECMAN { get; set; }
        /// <summary>
        /// 车辆编号，缺省为车牌号
        /// </summary>
        public virtual string CARNO { get; set; }
        /// <summary>
        /// 录入日期
        /// </summary>
        public virtual DateTime RECORDDATE { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public virtual string SIM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string FACTORYCODE { get; set; }
        /// <summary>
        /// 发动机编号
        /// </summary>
        public virtual string ENGINENO { get; set; }
        /// <summary>
        /// 底盘号（车架号）
        /// </summary>
        public virtual string CHASSISNO { get; set; }
        /// <summary>
        /// 车台电源方式 0  ACC  1电瓶 2大闸 
        /// </summary>
        public virtual int POWERTYPE { get; set; }
        /// <summary>
        /// 1001公务、1002生产
        /// </summary>
        public virtual int USAGE_SUB { get; set; }
        /// <summary>
        /// 服务截止日期
        /// </summary>
        public virtual DateTime STOP_CAR_DATE { get; set; }
        /// <summary>
        /// 车辆状态
        /// </summary>
        public virtual int CAR_STATUS { get; set; }
        /// <summary>
        /// 技术参数 如 8T水罐5T轻水泡沫
        /// </summary>
        public virtual string TECH_PARAMETERS { get; set; }
        /// <summary>
        /// 参数摘要 水8轻泡5 此摘要可以附加在地图车辆名称后
        /// </summary>
        public virtual string TECH_PARAMETERS_BRIEF { get; set; }
        /// <summary>
        /// 厂牌型号  以下不用
        /// </summary>
        public virtual string BRAND_MODEL { get; set; }
       
        #endregion
        /// <summary>
        /// 所属单位
        /// </summary>
        [Computed]
        public UnitEntity Unit { get; set; }
        /// <summary>
        /// 拥有的报警区域
        /// </summary>
        [Computed]
        public List<AreaEntity> Areas { get; set; } = new List<AreaEntity>();
    }
}
