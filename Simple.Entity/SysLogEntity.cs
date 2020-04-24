using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("TB_SYSLOG")]
    public class SysLogEntity
    {
        #region Model
        /// <summary>
        /// 日志ID
        /// </summary>
        [Key]
        public virtual int Logid { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime Logtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual object Logcontent { get; set; }
        #endregion
    }
}
