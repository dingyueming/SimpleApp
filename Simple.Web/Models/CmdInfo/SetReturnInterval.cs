using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models.CmdInfo
{
    /// <summary>
    /// 设置回传间隔
    /// </summary>
    public class SetReturnInterval
    {
        /// <summary>
        /// 呼叫类型
        /// </summary>
        /// <remarks>
        /// 1：定时呼叫
        /// 2：紧急报警定时呼叫
        /// 3：定长呼叫
        /// 6：电压（油量）监控
        /// </remarks>
        public int Type { get; set; }
        /// <summary>
        /// 呼叫间隔参数
        /// </summary>
        /// <remarks>
        /// 定时、紧急报警单位为秒
        /// 定长单位为米
        /// 定时、紧急报警定时表示时间间隔
        /// </remarks>
        public int Para { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        /// <remarks>
        /// 0XFFFF：代表一直监控
        /// 0为不允许的值
        /// 定次的时候表示次数
        /// 其他表示总的时间长度
        /// </remarks>
        public int Number { get; set; }
    }
}
