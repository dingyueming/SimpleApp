using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("NEW_ALARMINFO")]
    public class NewAlarmInfoEntity
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
        /// 速度
        /// </summary>
        public virtual double Speed { get; set; }
        /// <summary>
        /// 状态 同历史回放
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 扩展状态 不展示
        /// </summary>
        public virtual int Statusex { get; set; }
        /// <summary>
        /// 报警 暂时2种 16384 入区报警 8192 出区报警
        /// </summary>
        public virtual int Alarm { get; set; }
        /// <summary>
        /// 报警扩展 不展示
        /// </summary>
        public virtual int Alarmex { get; set; }
        /// <summary>
        /// 定位 1 定位 0 不定位
        /// </summary>
        public virtual int Locate { get; set; }
        /// <summary>
        /// 1 GPS 2 BD 3 GPS
        /// </summary>
        public virtual int Locatemode { get; set; }
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
        public virtual DateTime Record_Time { get; set; }
        /// <summary>
        /// 区域 关联区域表
        /// </summary>
        public virtual int Areaid { get; set; }
        /// <summary>
        /// 车辆
        /// </summary>
        [Computed]
        public CarEntity Car { get; set; }
        /// <summary>
        /// 报警区域
        /// </summary>
        [Computed]
        public AreaEntity Area { get; set; }
        #endregion
    }
}
