using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("car_area")]
    public class CarAreaEntity
    {
        /// <summary>
        /// 车辆ID
        /// </summary>		
        [ExplicitKey]
        public int CARID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string LICENSE { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>		
        public int AREAID { get; set; }
        /// <summary>
        /// 区域名字
        /// </summary>
        public string AREANAME { get; set; }
        /// <summary>
        /// 区域状态 0 禁用 1启用
        /// </summary>		
        public int STATUS { get; set; }
        /// <summary>
        /// 报警类型 0进入报警 1 出去报警 2 出入报警 3 路线偏离报警
        /// </summary>		
        public int ALARMTYPE { get; set; }
        /// <summary>
        /// 报警延迟时间 不用
        /// </summary>		
        public int ALARMDELAYTIME { get; set; }
        /// <summary>
        /// 允许的误差范围，即距离区域边界不为越界，单位米 不用
        /// </summary>		
        public int ERRORSCOPE { get; set; }
        /// <summary>
        /// MOBILEAREAID
        /// </summary>		
        public int MOBILEAREAID { get; set; }
        /// <summary>
        /// 0  系统区域 1 终端区域
        /// </summary>		
        public int AREAFLAG { get; set; }
        /// <summary>
        /// 超速值
        /// </summary>		
        public int OVERSPEED { get; set; }
        /// <summary>
        /// 允许解封卡在该区域解封锁控器的次数
        /// </summary>		
        public int OPENLOCKNUMBER { get; set; }
    }
}
