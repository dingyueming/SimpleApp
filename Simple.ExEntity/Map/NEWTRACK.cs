using Simple.Common;
using System;

namespace Simple.ExEntity.Map
{
    public partial class NEWTRACK
    {
        private DateTime _gnsstime;

        /// <summary>
        /// CARID
        /// </summary>
        public int CARID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>		
        public DateTime GNSSTIME
        {
            get { return _gnsstime; }
            set
            {
                if ((value - DateTimeHelper.Now).TotalDays >= 1)
                    value = DateTimeHelper.Now.AddMinutes(-10);
                else if ((value - DateTimeHelper.Now).TotalMinutes > 10)
                    value = DateTimeHelper.Now;
                _gnsstime = value;
            }
        }

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
        public int STATUSEX { get; set; }
        /// <summary>
        /// 报警
        /// </summary>
        public int ALARM { get; set; }
        /// <summary>
        /// 报警扩展
        /// </summary>
        public int ALARMEX { get; set; }
        /// <summary>
        /// 定位 1 定位 0 不定位
        /// </summary>
        public byte LOCATE { get; set; }
        /// <summary>
        /// 1 GPS 2 BD 3 GPS_BD
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
        /// 是否有油量
        /// </summary>
        public byte HAVEOIL { get; set; }
        /// <summary>
        /// 油量(总油量)
        /// </summary>
        public float OIL { get; set; }

        /// <summary>
        /// 位置描述
        /// </summary>
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
        /// 停车时间
        /// </summary>
        public ushort STOPTIME { get; set; }
        /// <summary>
        /// 捕获卫星数
        /// </summary>
        public ushort SATELLITENUMBER { get; set; }
        /// <summary>
        /// 通信信号强度
        /// </summary>
        public ushort SIGNALSTRENGTH { get; set; }
        /// <summary>
        /// 模拟量1
        /// </summary>
        public ushort SENSOR1 { get; set; }
        /// <summary>
        /// 模拟量2
        /// </summary>
        public ushort SENSOR2 { get; set; }
        /// <summary>
        /// 模拟量3
        /// </summary>
        public ushort SENSOR3 { get; set; }
        /// <summary>
        /// 模拟量4
        /// </summary>
        public ushort SENSOR4 { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>		
        public DateTime RECORD_TIME { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public float TEMPERATURE { get; set; }
        private float _oil1;
        /// <summary>
        /// OIL1
        /// </summary>
        public float OIL1 { get { return _oil1; } set { if (value != 0) _oil1 = value; } }
        private float _oil2;
        /// <summary>
        /// OIL2
        /// </summary>
        public float OIL2 { get { return _oil2; } set { if (value != 0) _oil2 = value; } }
        private float _oil3;
        /// <summary>
        /// OIL3
        /// </summary>
        public float OIL3 { get { return _oil3; } set { if (value != 0) _oil3 = value; } }
        private float _oil4;
        /// <summary>
        /// OIL4
        /// </summary>
        public float OIL4 { get { return _oil4; } set { if (value != 0) _oil4 = value; } }

        /// <summary>
        /// 区域
        /// </summary>
        public int AREAID { get; set; }
    }
}