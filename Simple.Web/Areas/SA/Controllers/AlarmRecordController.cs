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

namespace Simple.Web.Areas.SA.Controllers
{
    [Area("SA")]
    public class AlarmRecordController : SimpleBaseController
    {
        private readonly IAlarmRecordService alarmRecordService;
        public AlarmRecordController(IAlarmRecordService alarmRecordService)
        {
            this.alarmRecordService = alarmRecordService;
        }
        public async Task<JsonResult> Query(Pagination<AlarmRecordExEntity> pagination)
        {
            var data = await alarmRecordService.GetPage(pagination);
            return Json(data);
        }
        public override IActionResult Index()
        {
            return base.Index();
        }
    }
}