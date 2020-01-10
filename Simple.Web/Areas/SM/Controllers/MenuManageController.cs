﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class MenuManageController : SimpleBaseController
    {
        public MenuManageController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}