using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("tbl_interface_right")]
    public class InterfaceRightEntity
    {
        #region Model
        [ExplicitKey]
        /// <summary>
        /// 接口应用id
        /// </summary>
        public virtual int App_Id { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public virtual string Org_Code { get; set; }
        [Computed]
        public virtual UnitEntity Unit { get; set; }

        #endregion
    }
}
