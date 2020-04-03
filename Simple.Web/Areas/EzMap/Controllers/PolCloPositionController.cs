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
    /// <summary>
    /// 警员打卡位置查询
    /// </summary>
    [Area("EzMap")]
    public class PolCloPositionController : SimpleBaseController
    {
        private readonly ISjtlAttendancePositionService sjtlAttendancePositionService;
        public PolCloPositionController(ISjtlAttendancePositionService sjtlAttendancePositionService)
        {
            this.sjtlAttendancePositionService = sjtlAttendancePositionService;
        }

        public override IActionResult Index()
        {
            return base.Index();
        }

        public async Task<JsonResult> QueryPositionData(DateTime[] dateTimes, List<double[]> points, string nameOrPoliceCode)
        {
            var qm = new SjtlAttPosQm() { DateTimes = dateTimes, Points = points, NameOrPoliceCode = nameOrPoliceCode };
            var list = await sjtlAttendancePositionService.GetSjtlAttenPosExEntities(qm);
            return Json(list);
        }
    }
}