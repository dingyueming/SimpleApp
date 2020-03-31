using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Simple.IApplication.MapShow;
using Simple.IApplication.SM;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.EzMap.Controllers
{
    [Area("EzMap")]
    public class RealtimeMapController : SimpleBaseController
    {
        private IRealTimeMapService realTimeMapService;
        private IConfiguration configuration;
        public RealtimeMapController(IConfiguration configuration, IRealTimeMapService realTimeMapService)
        {
            this.configuration = configuration;
            this.realTimeMapService = realTimeMapService;
        }

        public override IActionResult Index()
        {
            ViewBag.SignalrUrl = configuration["SignalrUrl"];
            return base.Index();
        }

        /// <summary>
        /// 获取当前设备列表tree
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryDeviceTree()
        {
            var treeNodes = await realTimeMapService.GetDeviceTreeByUser(LoginUser.UsersId);
            return Json(treeNodes);
        }

        public async Task<JsonResult> QueryDeviceList()
        {
            //查询车辆和对讲机
            var devices = await realTimeMapService.GetViewAllTargetByUser(LoginUser.UsersId);
            return Json(devices);
        }

        public async Task<JsonResult> QueryLastLocatedData()
        {
            var locatedData = await realTimeMapService.GetLastLocatedByUser(LoginUser.UsersId);
            return Json(locatedData);
        }

    }
}