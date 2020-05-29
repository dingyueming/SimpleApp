using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Simple.IApplication.MapShow;
using Simple.IApplication.SM;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.AMap.Controllers
{
    [Area("AMap")]
    public class RealTimeMapController : SimpleBaseController
    {

        private IRealTimeMapService realTimeMapService;
        private IMapSearchService mapSearchService;
        public RealTimeMapController(IMapSearchService mapSearchService, IRealTimeMapService realTimeMapService)
        {
            this.mapSearchService = mapSearchService;
            this.realTimeMapService = realTimeMapService;
        }

        public override IActionResult Index()
        {
            return base.Index();
        }

        /// <summary>
        /// 获取设备列表tree
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryDeviceTree()
        {
            var treeNodes = await realTimeMapService.GetDeviceTreeByUser(LoginUser.UsersId);
            return Json(treeNodes);
        }

        /// <summary>
        /// 查询设备列表
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryDeviceList()
        {
            var devices = await realTimeMapService.GetCarExEntitiesByUser(LoginUser.UsersId);
            return Json(devices);
        }
        /// <summary>
        /// 查询最后定位数据
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryLastLocatedData()
        {
            var locatedData = await realTimeMapService.GetLastLocatedByUser(LoginUser.UsersId);
            return Json(locatedData);
        }
        /// <summary>
        /// 查询所有的执勤力量
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryXfKeyUnit()
        {
            var list = await mapSearchService.GetXfKeyUnitExEntities();
            return LowerJson(list);
        }
    }
}