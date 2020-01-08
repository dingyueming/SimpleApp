using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class CarController : SimpleBaseController
    {
        public CarController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}