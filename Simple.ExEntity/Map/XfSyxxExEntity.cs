using Simple.ExEntity.DM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.Map
{
    public class XfSyxxExEntity
    {
        #region Model
        /// <summary>
        /// 
        /// </summary>
        public virtual string SYBH { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string SYMC { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string SYDZ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual double? GIS_X { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual double? GIS_Y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string QSXS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string SYFL { get; set; }
        /// <summary>
        /// 采集单位
        /// </summary>
        public virtual string UNIT { get; set; }
        /// <summary>
        /// 消火栓形式
        /// </summary>
        public virtual string XHSXS { get; set; }
        /// <summary>
        /// 管网形式
        /// </summary>
        public virtual string GWXS { get; set; }
        /// <summary>
        /// 压力(Pa)
        /// </summary>
        public virtual double? YL { get; set; }
        /// <summary>
        /// 管径(mm)
        /// </summary>
        public virtual int? GJ { get; set; }
        #endregion
    }
}
