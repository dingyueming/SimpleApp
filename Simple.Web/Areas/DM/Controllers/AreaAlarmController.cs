using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Infrastructure.ControllerFilter;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.Tools;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class AreaAlarmController : SimpleBaseController
    {
        private readonly IAreaAlarmService areaAlarmService;
        public AreaAlarmController(IAreaAlarmService areaAlarmService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.areaAlarmService = areaAlarmService;
        }

        public IActionResult AddOrEdit()
        {
            return View();
        }

        public async Task<JsonResult> Query(Pagination<AreaExEntity> pagination)
        {
            var data = await areaAlarmService.GetPage(pagination);
            return FormerJson(data);
        }

        [SimpleActionFilter]
        public async Task Add(AreaExEntity exEntitiy, List<List<double[]>> points)
        {
            exEntitiy.USERID = LoginUser.UsersId;
            for (int i = 0; i < points[0].Count; i++)
            {
                var lo = points[0][i][0];
                var la = points[0][i][1];
                GeoLatLng geo = GPSTool.gcj02towgs84(lo, la);
                exEntitiy.AreaDetails.Add(new AreaDetailExEntity()
                {
                    POINTNO = i,
                    LONGTITUDE = geo.longitude * 1000000,
                    LATITUDE = geo.latitude * 1000000,
                });
            }
            await areaAlarmService.AddAreaAlarm(exEntitiy);
        }

        [SimpleActionFilter]
        public async Task BatchDelete(List<AreaExEntity> exEntities)
        {
            await areaAlarmService.DeleteAreaAlarm(exEntities);
        }
    }
}