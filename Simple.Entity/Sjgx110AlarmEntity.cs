using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Entity
{
    [Table("sjgx_110_alarm")]
    public  class Sjgx110AlarmEntity
    {
        #region Model
        /// <summary>
        /// 报警单编号
        /// </summary>
        [ExplicitKey]
        public virtual string Jjdbh { get; set; }
        /// <summary>
        /// 接警单位编码
        /// </summary>
        public virtual string Jjdwdm { get; set; }
        /// <summary>
        /// 报警时间
        /// </summary>
        public virtual DateTime Bjsj { get; set; }
        /// <summary>
        /// 报警人姓名
        /// </summary>
        public virtual string Bjrxm { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public virtual string Lxdh { get; set; }
        /// <summary>
        /// 管辖单位代码
        /// </summary>
        public virtual string Gxdwdm { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public virtual double Jd { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public virtual double Wd { get; set; }
        /// <summary>
        /// 原系统插入时间戳
        /// </summary>
        public virtual DateTime Gxsjc { get; set; }
        /// <summary>
        /// 插入时间戳
        /// </summary>
        public virtual DateTime InsertDate { get; set; }
        #endregion
    }
}
