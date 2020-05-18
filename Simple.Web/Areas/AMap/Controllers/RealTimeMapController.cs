using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.AMap.Controllers
{
    [Area("AMap")]
    public class RealTimeMapController : SimpleBaseController
    {
        public override IActionResult Index()
        {
            return base.Index();
        }
    }
}