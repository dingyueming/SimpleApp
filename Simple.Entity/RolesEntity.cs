using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("tb_roles")]
    public class RolesEntity
    {
        #region Model
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        public virtual decimal Rolesid { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual string Rolesname { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public virtual decimal Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual decimal Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime Createtime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public virtual decimal? Modifier { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? Modifytime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Computed]
        public UsersEntity User { get; set; }
        #endregion
    }
}
