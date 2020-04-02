using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.MapShow;
using Simple.Infrastructure.QueryModels;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.EzMap.Controllers
{
    [Area("EzMap")]
    public class LbsAlarmMapController : SimpleBaseController
    {
        private readonly ISjgx110AlarmService sjgx110AlarmService;
        public LbsAlarmMapController(ISjgx110AlarmService sjgx110AlarmService)
        {
            this.sjgx110AlarmService = sjgx110AlarmService;
        }

        public override IActionResult Index()
        {
            return base.Index();
        }

        public async Task<JsonResult> QueryLbsData(DateTime[] dateTimes, List<double[]> points)
        {
            var queryModel = new Sjgx110AlarmQm() { DateTimes = dateTimes, Points = points };
            var list = await sjgx110AlarmService.GetSjgx110AlarmExEntities(queryModel);
            return Json(list);
        }
    }
}