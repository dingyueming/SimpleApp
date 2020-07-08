using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using System.Linq;
using System.Data;

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
            return await carService.Add(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(CarExEntity exEntity)
        {
            exEntity.RECORDDATE = DateTime.Now;
            exEntity.RECMAN = LoginUser.UsersId;
            return await carService.Update(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(CarExEntity exEntity)
        {
            return await carService.Delete(new List<CarExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<CarExEntity> exEntities)
        {
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
        public async Task<FileResult> ExportExcel(Pagination<CarExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await carService.GetPage(pagination);
            var dt = new DataTable();
            string[] columns = { "单位", "车牌号", "内部编号", "设备号", "发动机号", "车架号", "SIM卡号" };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["车牌号"] = x.CARNO;
                dr["内部编号"] = x.LICENSE;
                dr["单位"] = x.Unit.UNITNAME;
                dr["设备号"] = x.MAC;
                dr["发动机号"] = x.ENGINENO;
                dr["车架号"] = x.CHASSISNO;
                dr["SIM卡号"] = x.SIM;
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }
}