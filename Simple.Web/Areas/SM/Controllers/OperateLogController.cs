using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class OperateLogController : SimpleBaseController
    {
        public override IActionResult Index()
        {
            return base.Index();
        }
        private readonly IOperateLogService operateLogService;
        public OperateLogController(IOperateLogService operateLogService)
        {
            this.operateLogService = operateLogService;
        }
        public async Task<JsonResult> Query(Pagination<OperateLogExEntity> pagination)
        {
            var data = await operateLogService.GetLogPage(pagination);
            return Json(data);
        }
    }
}