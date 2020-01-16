using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.DM
{
    /// <summary>
    /// 单位信息
    /// </summary>
    public class UnitExEntity
    {
        /// <summary>
        /// 单位ID号
        /// </summary>		
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
        public string FAX { get; set; }
        /// <summary>
        /// 单位邮政编码
        /// </summary>		
        public string POST { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>		
        public string CONTACTMAN { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>		
        public string CONTACTTEL { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        public string FULL_NAME { get; set; }
        /// <summary>
        /// 注释
        /// </summary>		
        public string REMARK { get; set; }
        /// <summary>
        /// 录入人
        /// </summary>		
        public uint RECMAN { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>		
        public DateTime RECDATE { get; set; }
        /// <summary>
        /// 所属分中心
        /// </summary>		
        public int CENTERID { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>		
        public int PID { get; set; }
        /// <summary>
        /// 级别
        /// </summary>		
        public uint UNIT_LEVEL { get; set; }
        /// <summary>
        /// GIS_X
        /// </summary>		
        public uint GIS_X { get; set; }
        /// <summary>
        /// GIS_Y
        /// </summary>		
        public uint GIS_Y { get; set; }
        /// <summary>
        /// 短名称
        /// </summary>
        public string SHORT_NAME { get; set; }

        public UnitExEntity ParentUnit { get; set; }
        public int? ParentUnitId
        {
            get
            {
                return ParentUnit?.UNITID;
            }
        }
    }
}
