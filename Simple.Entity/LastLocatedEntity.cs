using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("NEWTRACK_LASTLOCATED")]
    public class LastLocatedEntity
    {
        /// <summary>
        /// CARID
        /// </summary>
        [Key]
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
        public uint STATUS { get; set; }
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
    }
}

