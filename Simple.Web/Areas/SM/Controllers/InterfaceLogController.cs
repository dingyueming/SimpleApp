using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.IM;
using Simple.ExEntity.SM;
using Simple.IApplication.Dwjk;
using Simple.IApplication.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class InterfaceLogController : SimpleBaseController
    {
        public override IActionResult Index()
        {
            return base.Index();
        }
        private readonly IInterfaceLogService interfaceLogService;
        public InterfaceLogController(IInterfaceLogService interfaceLogService)
        {
            this.interfaceLogService = interfaceLogService;
        }
        public async Task<JsonResult> Query(Pagination<InterfaceLogExEntity> pagination)
        {
            var data = await interfaceLogService.GetPage(pagination);
            return Json(data);
        }
    }
}