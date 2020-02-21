using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.DM
{
    public class AreaDetailExEntity
    {
        /// <summary>
        /// 区域ID
        /// </summary>		
        public int AREAID { get; set; }
        /// <summary>
        /// 区域上点的序号,有顺序，如果是矩形，则存储左上角和右下角的点的坐标，
        /// 如果是多边形或线路，则存储每个定点的坐标，如果是圆形，则存储圆心坐标和半径长（米）
        /// </summary>		
        public int POINTNO { get; set; }
        /// <summary>
        /// X坐标 度*1000000
        /// </summary>		
        public double LONGTITUDE { get; set; }
        /// <summary>
        /// Y坐标 度*1000000
        /// </summary>		
        public double LATITUDE { get; set; }
    }
}
