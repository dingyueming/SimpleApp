using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.InterfaceDom.Controllers
{
    [Area("InterfaceDom")]
    public class AgreeMentPageController : SimpleBaseController
    {
        public override IActionResult Index()
        {
            return base.Index();
        }
    }
}