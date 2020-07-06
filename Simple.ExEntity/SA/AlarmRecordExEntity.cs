using Dapper.Contrib.Extensions;
using Simple.ExEntity.DM;
using System;
using System.Collections.Generic;
using System.Text;


namespace Simple.ExEntity.SA
{
    public class AlarmRecordExEntity
    {
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
        public int? RECORD_EVENT { get; set; }
        /// <summary>
        /// 报警事件  0 报备 1 出警 2 出围栏报警 3 未出动报警
        /// </summary>		
        public string RECORD_EVENT_STR
        {
            get
            {
                var str = "";
                switch (RECORD_EVENT)
                {
                    case 0:
                        str = "报备";
                        break;
                    case 1:
                        str = "出警";
                        break;
                    case 2:
                        str = "出围栏报警";
                        break;
                    case 3:
                        str = "未出动报警";
                        break;
                    default:
                        break;
                }
                return str;
            }
        }
        /// <summary>
        /// 车辆
        /// </summary>
        public CarExEntity Car { get; set; }

        public UnitExEntity Unit { get; set; }

        public int? UnitId { get; set; }

        public DateTime[] DateTimes { get; set; }
    }
}
