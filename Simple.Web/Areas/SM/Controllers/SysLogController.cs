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
    public class SysLogController : SimpleBaseController
    {
        public override IActionResult Index()
        {
            return base.Index();
        }
        private readonly ISysLogService sysLogService;
        public SysLogController(ISysLogService sysLogService)
        {
            this.sysLogService = sysLogService;
        }
        public async Task<JsonResult> Query(Pagination<SysLogExEntity> pagination)
        {
            var data = await sysLogService.GetLogPage(pagination);
            return Json(data);
        }
    }
}