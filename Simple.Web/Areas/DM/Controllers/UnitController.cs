using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using Simple.Web.Extension.ControllerEx;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class UnitController : SimpleBaseController
    {
        private readonly IUnitService unitService;
        public UnitController(IUnitService unitService)
        {
            this.unitService = unitService;
        }
        public async Task<JsonResult> Query(Pagination<UnitExEntity> pagination)
        {
            var data = await unitService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(UnitExEntity exEntity)
        {
            return await unitService.Add(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(UnitExEntity exEntity)
        {
            exEntity.RECDATE = DateTime.Now;
            exEntity.RECMAN = LoginUser.UsersId;
            return await unitService.Update(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(UnitExEntity exEntity)
        {
            return await unitService.Delete(new List<UnitExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<UnitExEntity> exEntities)
        {
            return await unitService.Delete(exEntities);
        }
        public async Task<FileResult> ExportExcel(Pagination<UnitExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await unitService.GetPage(pagination);
            var dt = new DataTable();
            string[] columns = { "单位名称", "上级单位", "地址", "值班电话", "执勤车辆", "执勤人数", "负责人" };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["单位名称"] = x.UNITNAME;
                dr["上级单位"] = x.ParentUnit == null ? "" : x.ParentUnit.UNITNAME;
                dr["地址"] = x.ADDRESS;
                dr["值班电话"] = x.DUTYPHONE;
                dr["执勤车辆"] = x.ONDUTYCAR;
                dr["执勤人数"] = x.ONDUTYCOUNT;
                dr["负责人"] = x.PRINCIPAL;
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }
}