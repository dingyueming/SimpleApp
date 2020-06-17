using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.SA;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using System.Security.Cryptography;
using System.Text;
using Simple.ExEntity.Map;

namespace Simple.Web.Areas.SA.Controllers
{
    [Area("SA")]
    public class LastLocatedController : SimpleBaseController
    {
        private readonly ILastLocatedService lastLocatedService;
        public LastLocatedController(ILastLocatedService lastLocatedService)
        {
            this.lastLocatedService = lastLocatedService;
        }
        public async Task<JsonResult> Query(Pagination<LastLocatedExEntity> pagination)
        {
            var data = await lastLocatedService.GetPage(pagination, pagination.SearchData.DateTimes);
            return Json(data);
        }
        public override IActionResult Index()
        {
            return base.Index();
        }
    }
}