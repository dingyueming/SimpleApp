using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.EzMap.Controllers
{
    [Area("EzMap")]
    public class NewAlarmInfoController : SimpleBaseController
    {
        public override IActionResult Index()
        {
            return base.Index();
        }
        private readonly INewAlarmInfoService newAlarmInfoService;
        public NewAlarmInfoController(INewAlarmInfoService newAlarmInfoService)
        {
            this.newAlarmInfoService = newAlarmInfoService;
        }
        public async Task<JsonResult> Query(Pagination<NewAlarmInfoExEntity> pagination)
        {
            var data = await newAlarmInfoService.GetPage(pagination);
            return Json(data);
        }
    }
}