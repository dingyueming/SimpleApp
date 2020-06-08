using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    /// <summary>
    /// 单位信息
    /// </summary>
    [Table("UNIT")]
    public class UnitEntity
    {
        #region Model
        /// <summary>
        /// 单位ID号
        /// </summary>
        [Key]
        public virtual int UNITID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public virtual string UNITNAME { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public virtual string URL { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public virtual string ADDRESS { get; set; }
        /// <summary>
        /// 单位电话
        /// </summary>
        public virtual string TELPHONE { get; set; }
        /// <summary>
        /// 单位传真
        /// </summary>
        public virtual string FAX { get; set; }
        /// <summary>
        /// 单位邮政编码
        /// </summary>
        public virtual string POST { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public virtual string CONTACTMAN { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public virtual string CONTACTTEL { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        public virtual string ACCOUNT { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public virtual string REMARK { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>
        public virtual int? RECMAN { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public virtual DateTime? RECDATE { get; set; }
        /// <summary>
        /// 所属分中心
        /// </summary>
        public virtual int? CENTERID { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public virtual int? PID { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public virtual int? UNIT_LEVEL { get; set; }
        /// <summary>
        /// 单位性质 
        /// </summary>
        public virtual string DL_UNIT_CHARACTER { get; set; }
        /// <summary>
        /// 值班电话
        /// </summary>
        public virtual string DUTYPHONE { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public virtual string PRINCIPAL { get; set; }
        /// <summary>
        /// 执勤人数
        /// </summary>
        public virtual string ONDUTYCOUNT { get; set; }
        /// <summary>
        /// 执勤车辆
        /// </summary>
        public virtual string ONDUTYCAR { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual double? GIS_X { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual double? GIS_Y { get; set; }
        /// <summary>
        /// 上级单位
        /// </summary>
        [Computed]
        public UnitEntity ParentUnit { get; set; }
        #endregion

    }
}
