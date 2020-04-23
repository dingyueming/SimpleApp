using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class CarController : SimpleBaseController
    {
        private readonly ICarService carService;
        private readonly IAreaAlarmService areaAlarmService;
        public CarController(IAreaAlarmService areaAlarmService, ICarService carService)
        {
            this.areaAlarmService = areaAlarmService;
            this.carService = carService;
        }
        public async Task<JsonResult> Query(Pagination<CarExEntity> pagination)
        {
            var data = await carService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(CarExEntity exEntity)
        {
            await RecordLog("车辆", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
            return await carService.Add(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(CarExEntity exEntity)
        {
            await RecordLog("车辆", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
            exEntity.RECORDDATE = DateTime.Now;
            exEntity.RECMAN = LoginUser.UsersId;
            return await carService.Update(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(CarExEntity exEntity)
        {
            await RecordLog("车辆", exEntity, Infrastructure.Enums.OperateTypeEnum.删除);
            return await carService.Delete(new List<CarExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<CarExEntity> exEntities)
        {
            await RecordLog("车辆", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            return await carService.Delete(exEntities);
        }
        public async Task<JsonResult> QueryAlarmArea()
        {
            var list = await areaAlarmService.GetAllAras();
            return FormerJson(list);
        }
        [SimpleAction]
        public async Task SaveCarArea(CarAreaExEntity exEntity)
        {
            await areaAlarmService.AddCarArea(exEntity);
        }
    }
}