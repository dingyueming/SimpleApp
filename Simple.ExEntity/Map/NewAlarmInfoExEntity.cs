using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Simple.ExEntity.Map;
using Simple.ExEntity.DM;

namespace Simple.ExEntity.Map
{
    public partial class NewAlarmInfoExEntity
    {
        #region Model
        /// <summary>
        /// 车辆ID
        /// </summary>
        public virtual int Carid { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime Gnsstime { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public virtual double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public virtual double Latitude { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public virtual double Heading { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public virtual string Headingshow
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
        public virtual double Speed { get; set; }
        /// <summary>
        /// 状态 同历史回放
        /// </summary>
        public virtual int Status { get; set; }
        public string Statusshow
        {
            get
            {
                CarStatus carStatus = new CarStatus();
                carStatus.RefreshStatus(this.Status, 0);
                return carStatus.ToString();
            }
        }
        /// <summary>
        /// 扩展状态 不展示
        /// </summary>
        public virtual int Statusex { get; set; }
        /// <summary>
        /// 报警 暂时2种 16384 入区报警 8192 出区报警
        /// </summary>
        public virtual int Alarm { get; set; }
        /// <summary>
        /// 报警 暂时2种 16384 入区报警 8192 出区报警
        /// </summary>
        public virtual string Alarmshow
        {
            get
            {
                if (Alarm == 16384)
                {
                    return "入区报警";
                }
                else
                {
                    return "出区报警";
                }
            }

        }
        /// <summary>
        /// 报警扩展 不展示
        /// </summary>
        public virtual int Alarmex { get; set; }
        /// <summary>
        /// 定位 1 定位 0 不定位
        /// </summary>
        public virtual int Locate { get; set; }
        public virtual string Locateshow
        {
            get
            {
                if (Locate == 1)
                {
                    return "定位";
                }
                else
                {
                    return "不定位";
                }
            }

        }
        /// <summary>
        /// 1 GPS 2 BD 3 GPS
        /// </summary>
        public virtual int Locatemode { get; set; }
        public virtual string Locatemodeshow
        {
            get
            {
                var str = string.Empty;
                switch (Locatemode)
                {

                    case 1:
                        str = "GPS";
                        break;
                    case 2:
                        str = "BD";
                        break;
                    case 3:
                        str = "GPS";
                        break;
                    default:
                        break;
                }
                return str;
            }

        }
        /// <summary>
        /// 高度 不展示
        /// </summary>
        public virtual int Altitude { get; set; }
        /// <summary>
        /// 偏移x 不展示
        /// </summary>
        public virtual int Offsetx { get; set; }
        /// <summary>
        /// 偏移y 不展示
        /// </summary>
        public virtual int Offsety { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public virtual DateTime Record_time { get; set; }
        /// <summary>
        /// 区域 关联区域表
        /// </summary>
        public virtual int Areaid { get; set; }
        /// <summary>
        /// 车辆
        /// </summary>
        public CarExEntity Car { get; set; }
        /// <summary>
        /// 报警区域
        /// </summary>
        public AreaExEntity Area { get; set; }
        #endregion
    }
    public partial class NewAlarmInfoExEntity
    {
        /// <summary>
        /// 时间数组（查询用）
        /// </summary>
        public virtual DateTime[] DateTimes { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public virtual string License { get; set; }
    }
}
