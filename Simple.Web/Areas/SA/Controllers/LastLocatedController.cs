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
using System.Data;

namespace Simple.Web.Areas.SA.Controllers
{
    [Area("SA")]
    public class LastLocatedController : SimpleBaseController
    {
        private readonly ILastLocatedService lastLocatedService;
        public LastLocatedController(ILastLocatedService lastLocatedService)
        {
            this.lastLocatedService = lastLocatedService;
        }
        public async Task<JsonResult> Query(Pagination<LastLocatedExEntity> pagination)
        {
            var data = await lastLocatedService.GetPage(pagination, pagination.SearchData.DateTimes);
            return Json(data);
        }
        public override IActionResult Index()
        {
            return base.Index();
        }
        /// <summary>
        /// 最近一周、一个月、三天内未定位的车辆查询页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Index2()
        {
            return base.Index();
        }

        public async Task<FileResult> ExportExcel(Pagination<LastLocatedExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await lastLocatedService.GetPage(pagination, pagination.SearchData.DateTimes);
            var dt = new DataTable();
            string[] columns = { "车牌号", "单位", "最后上线时间" };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["车牌号"] = $"{x.Car.LICENSE}({x.Car.CARNO})"; 
                dr["单位"] = x.Unit.UNITNAME;
                dr["最后上线时间"] = x.GNSSTIME;
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }
}