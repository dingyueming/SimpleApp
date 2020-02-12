using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.InfrastructureModel
{
    public class CommonResult
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public object ResultId { get; set; }

        /// <summary>
        /// 返回的结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 结果描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonResult()
        {
            IsSuccess = true;
            ResultId = Guid.NewGuid();
        }
    }
}
