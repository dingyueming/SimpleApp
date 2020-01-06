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
            //暂时先只查询车辆的
            var devices = await realTimeMapService.GetCarExEntitiesByUser(LoginUser.UsersId);
            return Json(devices);
        }

        public async Task<JsonResult> QueryLastLocatedData()
        {
            var locatedData = await realTimeMapService.GetLastLocatedByUser(LoginUser.UsersId);
            return Json(locatedData);
        }

    }
}