using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class UnitController : SimpleBaseController
    {
        private readonly IUnitService unitService;
        public UnitController(IUnitService unitService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.unitService = unitService;
        }
        public async Task<JsonResult> Query(Pagination<UnitExEntity> pagination)
        {
            var data = await unitService.GetPage(pagination);
            return Json(data);
        }

        public async Task<bool> Add(UnitExEntity exEntity)
        {
            return await unitService.Add(exEntity);
        }
        public async Task<bool> Update(UnitExEntity exEntity)
        {
            exEntity.RECDATE = DateTime.Now;
            exEntity.RECMAN = (uint)LoginUser.UsersId;
            return await unitService.Update(exEntity);
        }
        public async Task<bool> Delete(UnitExEntity exEntity)
        {
            return await unitService.Delete(new List<UnitExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<UnitExEntity> exEntities)
        {
            return await unitService.Delete(exEntities);
        }
        public async Task<JsonResult> QueryUnitTree()
        {
            var trees = await unitService.GetUnitTree();
            return Json(trees);
        }
    }
}