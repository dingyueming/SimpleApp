using Simple.ExEntity.DM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.Map
{
    public class XfKeyUnitExEntity
    {
        #region Model
        /// <summary>
        /// 单位名称
        /// </summary>
        public virtual string NAME { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string ADDRESS { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public virtual string UNIT_TYPE { get; set; }
        /// <summary>
        /// 所属中队
        /// </summary>
        public virtual string FIRE_BRIGADE { get; set; }
        /// <summary>
        /// 人员总数
        /// </summary>
        public virtual int? TOTAL_PERSON { get; set; }
        /// <summary>
        /// 建筑高度
        /// </summary>
        public virtual double? BUILDING_HEIGHT { get; set; }
        /// <summary>
        /// 层数
        /// </summary>
        public virtual string BUILDING_STOREY { get; set; }
        /// <summary>
        /// 建筑结构
        /// </summary>
        public virtual string BUILDING_STRUCTURE { get; set; }
        /// <summary>
        /// 占地面积
        /// </summary>
        public virtual double? COVER_AREA { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public virtual double? BUILDING_AREA { get; set; }
        /// <summary>
        /// 使用性质
        /// </summary>
        public virtual string USE_NATURE { get; set; }
        /// <summary>
        /// 毗连建筑（东）
        /// </summary>
        public virtual string JOIN_BUILDING_EAST { get; set; }
        /// <summary>
        /// 毗连建筑（南）
        /// </summary>
        public virtual string JOIN_BUILDING_SOUTH { get; set; }
        /// <summary>
        /// 毗连建筑（西）
        /// </summary>
        public virtual string JOIN_BUILDING_WEST { get; set; }
        /// <summary>
        /// 毗连建筑（北）
        /// </summary>
        public virtual string JOIN_BUILDING_NORTH { get; set; }
        /// <summary>
        /// 辖区中队行驶路线及时间
        /// </summary>
        public virtual string DRIVING_ROUTE { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public virtual double? GIS_X { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public virtual double? GIS_Y { get; set; }
        #endregion
    }
}
