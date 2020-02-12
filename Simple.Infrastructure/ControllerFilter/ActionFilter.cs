using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Simple.Infrastructure.InfrastructureModel;

namespace Simple.Infrastructure.ControllerFilter
{
    public class ActionFilterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //等Controller的Action方法执行完后，通过更改ActionExecutedContext类的Result属性，来替换Action方法返回的Json对象
            //判断是否为正常执行结束,如果不为空为异常执行结束.
            if (context.Exception == null)
            {
                //返回JsonResult结果信息
                context.Result = new JsonResult(new CommonResult { ResultId = context.Result is EmptyResult ? null : ((ContentResult)(context.Result)).Content, IsSuccess = true, Message = Message });
            }
            else
            {
                //返回JsonResult结果信息
                context.Result = new JsonResult(new CommonResult { ResultId = context.Result is EmptyResult ? null : ((ContentResult)(context.Result)).Content, IsSuccess = true, Message = context.Exception.Message });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Do something...
            //context.Result = new EmptyResult();//在IActionFilter.OnActionExecuting方法中，context的Result属性只要被赋值了不为null，
            //就不会执行Controller的Action了，也不会执行该IActionFilter拦截器的OnActionExecuted方法，同时在该IActionFilter拦截器之后注册的其它Filter拦截器也都不会被执行了
        }
    }
}
