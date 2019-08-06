using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    public class MenusEntity
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenusId { get; set; }
        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenusName { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string MenusUrl { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public int Modifier { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public int Remark { get; set; }
    }
}
