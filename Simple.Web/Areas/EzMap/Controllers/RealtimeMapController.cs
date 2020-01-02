using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.MapShow;
using Simple.IApplication.SM;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.EzMap.Controllers
{
    [Area("EzMap")]
    public class RealtimeMapController : SimpleBaseController
    {
        private IRealTimeMapService realTimeMapService;
        public RealtimeMapController(IRealTimeMapService realTimeMapService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.realTimeMapService = realTimeMapService;
        }

        public async Task<JsonResult> QueryDeviceList()
        {
            var treeView = await realTimeMapService.GetDeviceTreeByUser(LoginUser.UsersId);
            return Json(treeView);
        }

    }
}