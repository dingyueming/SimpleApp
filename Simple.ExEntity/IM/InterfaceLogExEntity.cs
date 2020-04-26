using Simple.ExEntity.DM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.ExEntity.IM
{
    public class InterfaceLogExEntity
    {
        #region Model
        /// <summary>
        /// 日志ID
        /// </summary>
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

        /// <summary>
        /// 时间数组（查询用）
        /// </summary>
        public virtual DateTime[] DateTimes { get; set; }
        #endregion
    }
}
