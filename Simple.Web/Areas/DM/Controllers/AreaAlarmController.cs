using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.Tools;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class AreaAlarmController : SimpleBaseController
    {
        private readonly IAreaAlarmService areaAlarmService;
        public AreaAlarmController(IAreaAlarmService areaAlarmService)
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

        [SimpleAction]
        public async Task Add(AreaExEntity exEntitiy, List<List<double[]>> points)
        {
            await RecordLog("报警区域", exEntitiy, Infrastructure.Enums.OperateTypeEnum.增加);
            exEntitiy.USERID = LoginUser.UsersId;
            //圆的计算方式和其他不一样
            if (exEntitiy.AREATYPE == 2 && points.Count == 1 && points[0].Count == 2)
            {
                exEntitiy.AreaDetails.Add(new AreaDetailExEntity()
                {
                    POINTNO = 0,
                    LONGTITUDE = points[0][0][0] * 1000000,
                    LATITUDE = points[0][0][1] * 1000000,
                });
                if ((int)points[0][1][0] > 99999)
                {
                    throw new Exception("区域范围过大");
                }
                exEntitiy.AreaDetails.Add(new AreaDetailExEntity()
                {
                    POINTNO = 1,
                    LONGTITUDE = (int)points[0][1][0],
                });
            }
            else
            {
                for (int i = 0; i < points[0].Count; i++)
                {
                    var lo = points[0][i][0];
                    var la = points[0][i][1];
                    exEntitiy.AreaDetails.Add(new AreaDetailExEntity()
                    {
                        POINTNO = i,
                        LONGTITUDE = lo * 1000000,
                        LATITUDE = la * 1000000,
                    });
                }
            }
            await areaAlarmService.AddAreaAlarm(exEntitiy);
        }

        [SimpleAction]
        public async Task BatchDelete(List<AreaExEntity> exEntities)
        {
            await RecordLog("报警区域", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            await areaAlarmService.DeleteAreaAlarm(exEntities);
        }

        [SimpleAction]
        public async Task RemoveBind(CarAreaExEntity exEntitiy)
        {
            await RecordLog("解除区域绑定", exEntitiy, Infrastructure.Enums.OperateTypeEnum.修改);
            await areaAlarmService.DeleteCarArea(exEntitiy);
        }

        [SimpleAction]
        public async Task BindCars(List<CarExEntity> cars, int areaId, int alarmType)
        {
            await RecordLog("报警区域车辆绑定", cars, Infrastructure.Enums.OperateTypeEnum.增加);
            var list = new List<CarAreaExEntity>();
            cars.ToList().ForEach(x =>
            {
                list.Add(new CarAreaExEntity()
                {
                    CARID = x.CARID,
                    AREAID = areaId,
                    ALARMTYPE = alarmType,
                    STATUS = 1
                });
            });
            await areaAlarmService.AddCarArea(list);
        }

        public async Task<JsonResult> QueryArea(int areaId)
        {
            var data = await areaAlarmService.GetAreaExEntity(areaId);
            return Json(data);
        }
    }
}