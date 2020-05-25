using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.Map
{
    public class NewTrackExEntity
    {
        /// <summary>
        /// CARID
        /// </summary>
        public uint CARID { get; set; }
        /// <summary>
        /// mac
        /// </summary>
        public CarExEntity Device { get; set; }
        /// <summary>
        /// 时间
        /// </summary>		
        public DateTime GNSSTIME { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public int TERMINALTYPE { get; set; }
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
        /// 千米
        /// </summary>
        public long KILOMETRE { get { return MILEAGE / 1000; } }
        /// <summary>
        /// 位置描述
        /// </summary>
        //[StringLength(260)]
        public string POSITION { get; set; }
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

        public string HEADING_STR
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
        public string LOCATE_STR => this.LOCATE == 1 ? "已定位" : "未定位";
        public string LOCATEMODE_STR
        {
            get
            {
                List<string> modeList = new List<string>();

                if ((LOCATE & 0x1) == 0x1) { modeList.Add("GPS"); }
                if ((LOCATE & 0x2) == 0x2) { modeList.Add("北斗"); }
                if ((LOCATE & 0x8) == 0x8) { modeList.Add("北斗一代"); }

                return string.Join("+", modeList);
            }
        }
        public string StatusShow { get; set; }
    }
}
