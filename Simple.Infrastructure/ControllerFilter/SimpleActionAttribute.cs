using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Simple.Infrastructure.InfrastructureModel;
using Newtonsoft.Json;

namespace Simple.Infrastructure.ControllerFilter
{
    public class SimpleActionAttribute : Attribute, IActionFilter
    {
        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; } = "";
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //等Controll er的Action方法执行完后，通过更改ActionExecutedContext类的Result属性，来替换Action方法返回的Json对象
            //判断是否为正常执行结束,如果不为空为异常执行结束.
            var result = new JsonResult("");
            if (context.Exception == null)
            {
                //返回JsonResult结果信息
                result.Value = new CommonResult { ResultId = context.Result is null ? null : context.Result, IsSuccess = true, Message = Message };
            }
            else
            {
                //返回JsonResult结果信息
                result.Value = new CommonResult { ResultId = context.Result is null ? null : context.Result, IsSuccess = false, Message = context.Exception.Message };
                context.Exception = null;
            }
            context.Result = result;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Do something...
            //context.Result = new EmptyResult();//在IActionFilter.OnActionExecuting方法中，context的Result属性只要被赋值了不为null，
            //就不会执行Controller的Action了，也不会执行该IActionFilter拦截器的OnActionExecuted方法，同时在该IActionFilter拦截器之后注册的其它Filter拦截器也都不会被执行了
        }
    }
}
