using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.SM
{
    public class SysLogExEntity
    {
        #region Model
        /// <summary>
        /// 日志ID
        /// </summary>
        public virtual int Logid { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public virtual DateTime Logtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual object Logcontent { get; set; }

        /// <summary>
        /// 时间数组（查询用）
        /// </summary>
        public virtual DateTime[] DateTimes { get; set; }
        #endregion
    }
}
