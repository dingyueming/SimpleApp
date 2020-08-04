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
using Simple.ExEntity.SA;
using Simple.Infrastructure.Tools;
using System.Data;

namespace Simple.Web.Areas.SA.Controllers
{
    [Area("SA")]
    public class AlarmRecordController : SimpleBaseController
    {
        private readonly IAlarmRecordService alarmRecordService;
        private readonly NpoiHelper npoiHelper;
        public AlarmRecordController(IAlarmRecordService alarmRecordService, NpoiHelper npoiHelper)
        {
            this.npoiHelper = npoiHelper;
            this.alarmRecordService = alarmRecordService;
        }
        public async Task<JsonResult> Query(Pagination<AlarmRecordExEntity> pagination)
        {
            if (!pagination.SearchData.UnitId.HasValue)
            {
                pagination.SearchData.UnitId = LoginUser.UnitId;
            }
            var data = await alarmRecordService.GetPage(pagination);
            return Json(data);
        }
        public override IActionResult Index()
        {
            return base.Index();
        }
        public async Task<FileResult> ExportExcel(Pagination<AlarmRecordExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await alarmRecordService.GetPage(pagination);
            var dt = new DataTable();
            dt.Columns.Add("车牌号");
            dt.Columns.Add("单位");
            dt.Columns.Add("出围栏时间");
            dt.Columns.Add("报警事件");
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["车牌号"] = $"{x.Car.LICENSE}({x.Car.CARNO})";
                dr["单位"] = x.Unit.UNITNAME;
                dr["出围栏时间"] = x.ALARM_TIME;
                dr["报警事件"] = x.RECORD_EVENT_STR;
                dt.Rows.Add(dr);
            }
            var buffer = await Task.Run(() => { return npoiHelper.OutputExcel(dt, "车牌号,单位,出围栏时间,报警事件".Split(',')); });
            return File(buffer, "application/ms-excel");
        }
    }
}