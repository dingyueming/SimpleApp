using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;
using Simple.Web.Areas.EzMap.Models;
using Simple.IApplication.MapShow;

namespace Simple.Web.Areas.AMap.Controllers
{
    [Area("AMap")]
    public class HistoryBackController : SimpleBaseController
    {
       
        private readonly IHistoryBackService historyBackService;
        public HistoryBackController(IHistoryBackService historyBackService)
        {
            this.historyBackService = historyBackService;
        }

        public override IActionResult Index()
        {
            return base.Index();
        }

        public async Task<JsonResult> QueryHistoryBackData(QueryHistoryBackModel search)
        {
            search.DeviceId = search.DeviceId.Replace("car-", "");
            var list = await historyBackService.GetNewTrackList(search);
            return Json(list);
        }

        public async Task<JsonResult> QueryDeviceTree()
        {
            var arr = await historyBackService.GetVueTreeSelectModels(LoginUser.UsersId);
            return Json(arr);
        }
    }
}