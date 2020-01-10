using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    public class RoleMenuExEntity
    {
        #region Model
        /// <summary>
        /// ID
        /// </summary>
        public virtual decimal Rolemenuid { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual decimal Rolesid { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual decimal Menusid { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual decimal Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime Createtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        #endregion
    }
}
