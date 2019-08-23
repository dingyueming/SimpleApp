using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Simple.Web.Controllers
{
    public class CodeGenerateController : Controller
    {
        /// <summary>
        /// 实体生成页面
        /// </summary>
        /// <returns></returns>
        public IActionResult GenerateEntity()
        {
            return View();
        }

        //public IActionResult GenerateDomai
    }
}