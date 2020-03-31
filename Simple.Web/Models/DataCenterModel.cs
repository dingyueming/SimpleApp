using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    public class DataCenterModel
    {
        /// <summary>
        /// mac
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 定位数据
        /// </summary>
        public GpsDataModel GpsData { get; set; }
        /// <summary>
        /// 报警数据
        /// </summary>
        public AlarmDataModel AlarmData { get; set; }

    }
    /// <summary>
    /// 定位对象
    /// </summary>
    public class GpsDataModel
    {
        /// <summary>
        /// 定位时间
        /// </summary>
        public string GnssTime { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 方向 正北0 正东 90
        /// </summary>
        public int Heading { get; set; }
        public string HeadingStr
        {
            get
            {
                var heading = Heading;
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
        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// 定位状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 定位状态文字描述
        /// </summary>
        public string Status_Str { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Altitude { get; set; }
        /// <summary>
        /// 总里程
        /// </summary>
        public int Mileage { get; set; }
        /// <summary>
        /// 偏移量X（使用网络地图需要）
        /// </summary>
        public int OffsetX { get; set; }
        /// <summary>
        /// 偏移量Y（使用网络地图需要）
        /// </summary>
        public int OffsetY { get; set; }
        /// <summary>
        /// 是否定位
        /// </summary>
        public bool Locate { get; set; }
        /// <summary>
        /// 是否定位
        /// </summary>
        public string LocateStr => this.Locate ? "已定位" : "未定位";
        /// <summary>
        /// 定位模式
        /// </summary>
        public string LocateMode { get; set; }
    }
    /// <summary>
    /// 报警对象
    /// </summary>
    public class AlarmDataModel
    {
        /// <summary>
        /// 报警时间
        /// </summary>
        public string AlarmTime { get; set; }
        /// <summary>
        /// 报警描述
        /// </summary>
        public string AlarmType { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 方向 正北0 正东 90
        /// </summary>
        public int Heading { get; set; }
        public string HeadingStr
        {
            get
            {
                var heading = Heading;
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
        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// 定位状态文字描述
        /// </summary>
        public string Status_Str { get; set; }
        /// <summary>
        /// 偏移量X（使用网络地图需要）
        /// </summary>
        public int OffsetX { get; set; }
        /// <summary>
        /// 偏移量Y（使用网络地图需要）
        /// </summary>
        public int OffsetY { get; set; }
        /// <summary>
        /// 是否定位
        /// </summary>
        public bool Locate { get; set; }
        /// <summary>
        /// 是否定位
        /// </summary>
        public string LocateStr => this.Locate ? "已定位" : "未定位";
        /// <summary>
        /// 定位模式
        /// </summary>
        public string LocateMode { get; set; }
    }

}


