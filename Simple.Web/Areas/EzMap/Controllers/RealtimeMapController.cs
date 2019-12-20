using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.EzMap.Controllers
{
    [Area("EzMap")]
    public class RealtimeMapController : SimpleBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}