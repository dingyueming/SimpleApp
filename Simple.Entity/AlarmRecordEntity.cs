using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;


namespace Simple.Entity
{
    [Table("ALARM_RECORD")]
    public class AlarmRecordEntity
    {
        [ExplicitKey]
        /// <summary>
        /// 报警ID
        /// </summary>		
        public int ALARM_ID { get; set; }
        /// <summary>
        /// 车辆ID
        /// </summary>		
        public int CARID { get; set; }
        /// <summary>
        /// 出围栏时间
        /// </summary>		
        public DateTime ALARM_TIME { get; set; }
        /// <summary>
        /// 报警事件  0 报备 1 出警 2 出围栏报警 3 未出动报警
        /// </summary>		
        public int RECORD_EVENT { get; set; }
        /// <summary>
        /// 车辆
        /// </summary>
        [Computed]
        public CarEntity Car { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [Computed]
        public UnitEntity Unit { get; set; }
    }
}
