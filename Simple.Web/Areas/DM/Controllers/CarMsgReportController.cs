using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using Simple.IApplication.MapShow;
using System.Data;
using System.Linq;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class CarMsgReportController : SimpleBaseController
    {
        private readonly IHistoryBackService historyBackService;
        private readonly ICarMsgReportService carMsgReportService;
        public CarMsgReportController(IHistoryBackService historyBackService, ICarMsgReportService carMsgReportService)
        {
            this.carMsgReportService = carMsgReportService;
            this.historyBackService = historyBackService;
        }
        public override IActionResult Index()
        {
            return base.Index();
        }
        public async Task<JsonResult> QueryDeviceTree()
        {
            var arr = await historyBackService.GetVueTreeSelectModels(LoginUser.UsersId);
            return Json(arr);
        }
        public async Task<JsonResult> Query(Pagination<CarMsgReportExEntity> pagination)
        {
            var data = await carMsgReportService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(CarMsgReportExEntity exEntity)
        {
            var exEntities = new List<CarMsgReportExEntity>();
            foreach (var item in exEntity.CARIDS)
            {
                exEntities.Add(new CarMsgReportExEntity()
                {
                    CARID = item,
                    CREATETIME = DateTime.Now,
                    CREATOR = LoginUser.UsersId,
                    APPROVER = exEntity.APPROVER,
                    BACKTIME = exEntity.BACKTIME,
                    CONTENT = exEntity.CONTENT,
                    REMARK = exEntity.REMARK,
                    SENDTIME = exEntity.SENDTIME
                });
            }
            return await carMsgReportService.Add(exEntities.ToArray());
        }
        [SimpleAction]
        public async Task<bool> Update(CarMsgReportExEntity exEntity)
        {
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
        public async Task<FileResult> ExportExcel(Pagination<CarMsgReportExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await carMsgReportService.GetPage(pagination);
            var dt = new DataTable();
            string[] columns = { "报备车辆", "审批人", "出动时间", "返回时间", "任务内容" };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["报备车辆"] = $"{x.Car.LICENSE}({x.Car.CARNO})";
                dr["审批人"] = x.APPROVER;
                dr["出动时间"] = x.SENDTIME;
                dr["返回时间"] = x.BACKTIME;
                dr["任务内容"] = x.CONTENT;
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }
}