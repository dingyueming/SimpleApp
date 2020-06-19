using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Web.ApiControllers.Models
{
    public class ApiResult<T> where T : class
    {
        /// <summary>
        /// 状态码1成功0失败
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        public static ApiResult<T> Success(T data = null)
        {
            return new ApiResult<T>()
            {
                Code = 1,
                Msg = "success",
                Data = data
            };
        }
        public static ApiResult<T> Fail(string msg = "fail")
        {
            return new ApiResult<T>()
            {
                Code = 0,
                Msg = msg,
                Data = null
            };
        }
    }
}
