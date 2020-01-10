using System;
using System.Security.Principal;

namespace Simple.ExEntity
{
    public class UsersExEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UsersId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UsersName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public int Status { get; set; }
        public string StatusStr
        {
            get
            {
                return Status == 0 ? "停用" : "启用";
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int Creator { get; set; }
        public string CreateStr
        {
            get
            {
                return User?.UsersName;
            }
        }
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

        public UsersExEntity User { get; set; }
    }
}
