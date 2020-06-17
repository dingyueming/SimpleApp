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
    public class CarMsgReportController : SimpleBaseController
    {
        private readonly ICarMsgReportService carMsgReportService;
        public CarMsgReportController(IAreaAlarmService areaAlarmService, ICarMsgReportService carMsgReportService)
        {
            this.carMsgReportService = carMsgReportService;
        }
        public override IActionResult Index()
        {
            return base.Index();
        }

        public async Task<JsonResult> Query(Pagination<CarMsgReportExEntity> pagination)
        {
            var data = await carMsgReportService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(CarMsgReportExEntity exEntity)
        {
            return await carMsgReportService.Add(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(CarMsgReportExEntity exEntity)
        {
            exEntity.CREATETIME = DateTime.Now;
            exEntity.CREATOR = LoginUser.UsersId;
            return await carMsgReportService.Update(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(CarMsgReportExEntity exEntity)
        {
            return await carMsgReportService.Delete(new List<CarMsgReportExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<CarMsgReportExEntity> exEntities)
        {
            return await carMsgReportService.Delete(exEntities);
        }
    }
}