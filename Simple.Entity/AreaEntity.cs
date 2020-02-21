using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("Area")]
    public class AreaEntity
    {
        public AreaEntity()
        {
            this.AreaDetails = new List<AreaDetailEntity>();
            this.CarAreas = new List<CarAreaEntity>();
            this.Devices = new List<ViewAllTargetEntity>();
        }
        /// <summary>
        /// 区域Id
        /// </summary>		
        [Key]
        public int AREAID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>		
        public string AREANAME { get; set; }
        /// <summary>
        /// 区域所属单位
        /// </summary>		
        public int UNITID { get; set; }
        /// <summary>
        /// 区域所属分中心
        /// </summary>		
        public int CENTERID { get; set; }
        /// <summary>
        /// 区域类型 0 矩形1 多边形 2 圆形3线路,4点
        /// </summary>		
        public int AREATYPE { get; set; }
        /// <summary>
        /// USERID
        /// </summary>		
        public int USERID { get; set; }
        /// <summary>
        /// 区域内超速速度值
        /// </summary>		
        public float OVERSPEED { get; set; }
        /// <summary>
        /// 0 一般区域 1 工作区域
        /// </summary>		
        public int WORKAREA { get; set; }
        /// <summary>
        /// ISSHARE
        /// </summary>		
        public int ISSHARE { get; set; }
        /// <summary>
        /// 东营油库油站，0油库，1油站
        /// </summary>		
        public int OIL_RECT_TYPE { get; set; }

        [Computed]
        public List<AreaDetailEntity> AreaDetails { get; private set; }
        [Computed]
        public List<CarAreaEntity> CarAreas { get; private set; }
        [Computed]
        public List<ViewAllTargetEntity> Devices { get; private set; }

    }
}
