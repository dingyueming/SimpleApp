using Dapper.Contrib.Extensions;
using Simple.ExEntity.DM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.Map
{
    public class LastLocatedExEntity
    {
        /// <summary>
        /// CARID
        /// </summary>
        public uint CARID { get; set; }
        public int MTYPE { get; set; }
        /// <summary>
        /// 时间
        /// </summary>		
        public DateTime GNSSTIME { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double LONGITUDE { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double LATITUDE { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double LONGITUDEAMAP => LONGITUDE + (OFFSETX * 0.0000001);
        /// <summary>
        /// 纬度
        /// </summary>
        public double LATITUDEAMAP => LATITUDE + (OFFSETY * 0.0000001);
        /// <summary>
        /// 方向
        /// </summary>
        public float HEADING { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public float SPEED { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int STATUS { get; set; }
        /// <summary>
        /// 扩展状态
        /// </summary>	
        public uint STATUSEX { get; set; }
        /// <summary>
        /// 报警
        /// </summary>
        public uint ALARM { get; set; }
        /// <summary>
        /// 报警扩展
        /// </summary>
        public uint ALARMEX { get; set; }
        /// <summary>
        /// 定位 1 定位 0 不定位
        /// </summary>
        public byte LOCATE { get; set; }
        /// <summary>
        /// 位 0 GPS 1 BD 2惯导 3北斗一代
        /// </summary>
        public byte LOCATEMODE { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public ushort ALTITUDE { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        public long MILEAGE { get; set; }
        /// <summary>
        /// 偏移x
        /// </summary>
        public short OFFSETX { get; set; }
        /// <summary>
        /// 偏移y
        /// </summary>
        public short OFFSETY { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>		
        public DateTime RECORD_TIME { get; set; }

        /// <summary>
        /// mac
        /// </summary>
        public string Mac { get; set; }
        public string License { get; set; }
        public string HeadingStr
        {
            get
            {
                var heading = HEADING;
                if (heading > 337 && heading < 23)
                    return "北";
                if (heading > 22 && heading < 68)
                    return "东北";
                if (heading > 67 && heading < 113)
                    return "东";
                if (heading > 112 && heading < 158)
                    return "东南";
                if (heading > 157 && heading < 203)
                    return "南";
                if (heading > 202 && heading < 248)
                    return "西南";
                if (heading > 247 && heading < 293)
                    return "西";
                if (heading > 292 && heading < 338)
                    return "西北";
                return "北";
            }
        }

        public string Status_Str
        {
            get
            {
                CarStatus carStatus = new CarStatus();
                carStatus.RefreshStatus(this.STATUS, 0);
                return carStatus.ToString();
            }
        }

        public CarExEntity Car { get; set; }

        public UnitExEntity Unit { get; set; }

        #region searchData
        public DateTime[] DateTimes { get; set; }
        public bool IsSearchLocated { get; set; }
        #endregion
    }
}

