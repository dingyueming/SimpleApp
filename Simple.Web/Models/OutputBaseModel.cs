using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.Models
{
    /// <summary>
    /// 对外数据接口Base模型
    /// </summary>
    public class OutputBaseModel<T> where T : class
    {
        /// <summary>
        /// 返回状态
        /// </summary>
        /// <remarks>1成功，0失败</remarks>
        public int Status { get; set; } = 1;

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }


    }
}
