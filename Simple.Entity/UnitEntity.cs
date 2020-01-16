﻿using Dapper.Contrib.Extensions;
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
        /// <summary>
        /// 单位ID号
        /// </summary>		
        [Key]
        public int UNITID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>		
        public string UNITNAME { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public string ORG_CODE { get; set; }
        /// <summary>
        /// 上级组织机构
        /// </summary>
        public string P_ORG_CODE { get; set; }
        /// <summary>
        /// 单位电话
        /// </summary>		
        public string TELPHONE { get; set; }
        /// <summary>
        /// 单位传真
        /// </summary>		
        [Computed]
        public string FAX { get; set; }
        /// <summary>
        /// 单位邮政编码
        /// </summary>		
        [Computed]
        public string POST { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>		
        public string CONTACTMAN { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>		
        [Computed]
        public string CONTACTTEL { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        [Computed]
        public string FULL_NAME { get; set; }
        /// <summary>
        /// 注释
        /// </summary>		
        public string REMARK { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>		
        public int RECMAN { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>		
        public DateTime RECDATE { get; set; }
        /// <summary>
        /// 所属分中心
        /// </summary>		
        [Computed]
        public int CENTERID { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>		
        public int PID { get; set; }
        /// <summary>
        /// 级别
        /// </summary>		
        [Computed]
        public uint UNIT_LEVEL { get; set; }
        /// <summary>
        /// GIS_X
        /// </summary>		
        [Computed]
        public uint GIS_X { get; set; }
        /// <summary>
        /// GIS_Y
        /// </summary>		
        [Computed]
        public uint GIS_Y { get; set; }
        /// <summary>
        /// 短名称
        /// </summary>
        [Computed]
        public string SHORT_NAME { get; set; }
        /// <summary>
        /// 上级单位
        /// </summary>
        [Computed]
        public UnitEntity ParentUnit { get; set; }
    }
}
