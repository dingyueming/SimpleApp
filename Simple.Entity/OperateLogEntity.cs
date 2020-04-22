using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("TB_OPERATELOG")]
    public class OperateLogEntity
    {
        #region Model
        /// <summary>
        /// 日志ID
        /// </summary>
        [Key]
        public virtual int Logid { get; set; }
        /// <summary>
        /// 登陆名称
        /// </summary>
        public virtual string Loginname { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public virtual string Realname { get; set; }
        /// <summary>
        /// 登陆IP
        /// </summary>
        public virtual string Ip { get; set; }
        /// <summary>
        /// -1退出,0登陆,1增,2删,3改
        /// </summary>
        public virtual int Operatetype { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime Operatetime { get; set; }
        /// <summary>
        /// 日志对象名
        /// </summary>
        public virtual string Modelname { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        #endregion
    }
}
