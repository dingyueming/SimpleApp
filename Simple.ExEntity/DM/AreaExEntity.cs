using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Simple.ExEntity.Map;

namespace Simple.ExEntity.DM
{
    public class AreaExEntity
    {
        /// <summary>
        /// 区域Id
        /// </summary>		
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
        public List<AreaDetailExEntity> AreaDetails { get; set; } = new List<AreaDetailExEntity>();
        private List<AreaDetailExEntity> _areaDetailsOrderBy;
        public List<AreaDetailExEntity> AreaDetailsOrderBy { get { return AreaDetails.OrderBy(x => x.POINTNO).ToList(); } set => _areaDetailsOrderBy = value; }
        public List<CarAreaExEntity> CarAreas { get; set; } = new List<CarAreaExEntity>();
        public int Count
        {
            get
            {
                return CarAreas.Count;
            }
        }
        public List<ViewAllTargetExEntity> Devices { get; set; }
    }
}
