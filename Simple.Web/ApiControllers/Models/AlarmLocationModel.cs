using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.ApiControllers.Models
{
    public class AlarmLocationModel
    {
        #region Model
        /// <summary>
        /// 接警单编号
        /// </summary>
        public virtual string Jjdbh { get; set; }
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
        /// 经度
        /// </summary>
        public virtual double Jd { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public virtual double Wd { get; set; }
        /// <summary>
        /// 接警单位
        /// </summary>
        public virtual string Jjdw { get; set; }
        /// <summary>
        /// 管辖单位
        /// </summary>
        public virtual string Gxdw { get; set; }
        #endregion
    }
}
