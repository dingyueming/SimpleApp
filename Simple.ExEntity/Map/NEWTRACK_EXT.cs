using System;

namespace Simple.ExEntity.Map
{
    public partial class NEWTRACK
    {

        #region 软件特有字段
        private bool _isRealTime = false;
        private string _dataType = "历史";
        /// <summary>
        /// 当天首次里程
        /// </summary>
        public long TODAYFIRSTMILEAGE { get; set; }

        /// <summary>
        /// 当天里程
        /// </summary>
        public int TODAYMILEAGE { get; set; }
        public int TODAYMILEAGE_K { get { return TODAYMILEAGE / 1000; } }
        public int MILEAGE_K { get { return (int)MILEAGE / 1000; } }
        /// <summary>
        /// 是否固定定位模式字符串
        /// </summary>
        public static string FixLocateModeStr { get; set; }
        /// <summary>
        /// 是否使用网络地址解析
        /// </summary>
        public static bool IsWebPosition { get; set; }
        /// <summary>
        /// 是否是实时
        /// </summary>
        public bool IsRealTime
        {
            get { return _isRealTime; }
            set
            {
                _isRealTime = value;
                _dataType = _isRealTime ? "实时" : "历史";
            }
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType
        {
            get { return _dataType; }
        }

        #endregion


        public string MAC { get; set; }

        //public string HEADING_STR
        //{
        //    get { return GPSTool.GetHeadingStr(HEADING); }
        //}
        //public string LOCATE_STR { get { return GPSTool.GetLocateStr(LOCATE); } }
        //public string LOCATEMODE_STR { get { return GPSTool.GetLocateModeStr(LOCATEMODE); } }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string POSITION_CITY
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(POSITION))
                {
                    string[] array = POSITION.Split(' ');
                    if (array.Length > 1)
                    {
                        return array[1];
                    }
                    return array[0];
                }
                else
                {
                    return "无";
                }
            }
        }

        public string StatusShow { get; set; }

        //private DateTime lastUpTime = DateTimeHelper.Now;
        /// <summary>
        /// 辅助定位模式字符串
        /// </summary>
        //public string ASSIST_LOCATEMODE_STR
        //{
        //    get
        //    {
        //        //return GPSTool.GetAssistLocateModeStr(LOCATEMODE);
        //    }
        //}
        public void UpdateData(ushort heading, int status, int statusEx, int alarmStatus, int alarmStatusEx, byte locate, byte locatemode, DateTime gnsstime,
                               double lo, double la, ushort altitude, float speed, int mileage, byte haveoil, ushort oil1, ushort oil2, ushort oil3, ushort oil4,
                               ushort sensor1, ushort sensor2, ushort sensor3, ushort sensor4, string position, short loadjust, short laadjust, ushort stoptime, short tmp, int areaid)
        {
            //lastUpTime = DateTimeHelper.Now;
            IsRealTime = true;

            HEADING = heading;
            STATUS = status;
            STATUSEX = statusEx;
            ALARM = alarmStatus;
            ALARMEX = alarmStatusEx;
            LOCATE = locate;
            LOCATEMODE = locatemode;
            GNSSTIME = gnsstime;
            LONGITUDE = lo;
            LATITUDE = la;
            ALTITUDE = altitude;
            SPEED = speed;
            MILEAGE = mileage;
            //TODAYMILEAGE = LOCATE == 1 ? (int)(MILEAGE - TODAYFIRSTMILEAGE) : 0;
            HAVEOIL = haveoil;
            OIL1 = oil1;
            OIL2 = oil2;
            OIL3 = oil3;
            OIL4 = oil4;
            OIL = OIL1 + OIL2 + OIL3 + OIL4;
            SENSOR1 = sensor1;
            SENSOR2 = sensor2;
            SENSOR3 = sensor3;
            SENSOR4 = sensor4;
            OFFSETX = loadjust;
            OFFSETY = laadjust;
            STOPTIME = stoptime;
            TEMPERATURE = tmp;
            AREAID = areaid;
            POSITION = position;
        }
    }
}
