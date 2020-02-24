using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("CARS")]
    public class CarEntity
    {
        /// <summary>
        /// 车辆ID号
        /// </summary>		
        [Key]
        public int CARID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string LICENSE { get; set; }
        /// <summary>
        /// 车主标识       owner
        /// </summary>		
        public int OWNERID { get; set; }
        /// <summary>
        /// 单位ID号          unit
        /// </summary>		
        public int UNITID { get; set; }
        /// <summary>
        /// 通讯号码
        /// </summary>
        public string MAC { get; set; }
        /// <summary>
        /// 车台类型 
        /// </summary>		
        public int MTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int CTYPE { get; set; }
        /// <summary>
        /// 主驾驶
        /// </summary>		
        public int FIRSTDRIVER { get; set; }
        /// <summary>
        /// 副驾驶    
        /// </summary>		
        public int SECONDDRIVER { get; set; }
        /// <summary>
        /// 所属标志
        /// </summary>
        public string OWNERTYPE { get; set; }
        /// <summary>
        /// 车辆类型，比如：富康，捷达等            dict_cartypes
        /// </summary>		
        public int CARTYPE { get; set; }
        /// <summary>
        /// 车辆用途，比如：警车，私家车，运钞车等        dict_carusage (电力,1001主业、1004集体、1005农电)
        /// </summary>		
        public int USAGE { get; set; }
        /// <summary>
        /// 车辆颜色                                         dict_color
        /// </summary>		
        public int COLOR { get; set; }
        /// <summary>
        /// 车辆描述
        /// </summary>		
        public string CARDESC { get; set; }
        /// <summary>
        /// 投用时间
        /// </summary>		
        public DateTime USETIME { get; set; }
        /// <summary>
        /// 入网时间
        /// </summary>		
        public DateTime REGTIME { get; set; }
        /// <summary>
        /// 所属分中心 0：为总中心                  sub_center
        /// </summary>		
        public int CENTERID { get; set; }
        /// <summary>
        /// 载重量单位为吨
        /// </summary>		
        public int SOTRAGE { get; set; }
        /// <summary>
        /// 调度状态 1，请载 2，任务执行中 0：非请载也非任务执行
        /// </summary>		
        public int ATTEMPSTATUS { get; set; }
        /// <summary>
        /// 录入人        manager
        /// </summary>		
        public int RECMAN { get; set; }
        /// <summary>
        /// 车辆编号，缺省为车牌号
        /// </summary>		
        public string CARNO { get; set; }
        /// <summary>
        /// 录入日期
        /// </summary>		
        public DateTime RECORDDATE { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>		
        public string SIM { get; set; }
        /// <summary>
        /// FACTORYCODE
        /// </summary>		
        public string FACTORYCODE { get; set; }
        /// <summary>
        /// 发动机编号
        /// </summary>		
        public string ENGINENO { get; set; }
        /// <summary>
        /// 底盘号（车架号）
        /// </summary>		
        public string CHASSISNO { get; set; }
        /// <summary>
        /// 车台电源方式 0  ACC  1电瓶  2大闸
        /// </summary>		
        public int POWERTYPE { get; set; }
        /// <summary>
        /// 百公里油耗
        /// </summary>		
        public int FUEL_CONSUMPTION { get; set; }
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
