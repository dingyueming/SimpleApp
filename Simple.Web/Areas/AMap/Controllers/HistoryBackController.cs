using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.Web.Controllers;
using Simple.Web.Areas.EzMap.Models;
using Simple.IApplication.MapShow;
using System.Data;

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
        public async Task<FileResult> ExportExcel(QueryHistoryBackModel search)
        {
            search.DeviceId = search.DeviceId.Replace("car-", "");
            var list = await historyBackService.GetNewTrackList(search);
            var dt = new DataTable();
            string[] columns = { "车牌号", "识别码", "SIM卡", "定位时间", "接收时间", "速度", "定位", "方向", "状态", "定位模式", "累计里程", "经度", "纬度" };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            foreach (var x in list)
            {
                DataRow dr = dt.NewRow();
                dr["车牌号"] = $"{x.Device.LICENSE}({x.Device.CARNO})";
                dr["识别码"] = x.Device.MAC;
                dr["SIM卡"] = x.Device.SIM;
                dr["定位时间"] = x.GNSSTIME;
                dr["接收时间"] = x.RECORD_TIME;
                dr["速度"] = x.SPEED;
                dr["定位"] = x.LOCATE_STR;
                dr["方向"] = x.HEADING_STR;
                dr["状态"] = x.StatusShow;
                dr["定位模式"] = x.LOCATEMODE_STR;
                dr["累计里程"] = (int)x.KILOMETRE / 1000;
                dr["经度"] = x.LONGITUDE;
                dr["纬度"] = x.LATITUDE;
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }
}