using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("TBL_INTERFACE_LOG")]
    public class InterfaceLogEntity
    {
        #region Model
        /// <summary>
        /// 日志ID
        /// </summary>
        [Key]
        public virtual int Id { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public virtual string Appname { get; set; }
        /// <summary>
        /// 转发时间
        /// </summary>
        public virtual DateTime Stat_time { get; set; }
        /// <summary>
        /// 转发条数
        /// </summary>
        public virtual int Stat_numbeer { get; set; }
        #endregion
    }
}
