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
        public UnitController(IUnitService unitService)
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
            await RecordLog("单位", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
            return await unitService.Add(exEntity);
        }
        public async Task<bool> Update(UnitExEntity exEntity)
        {
            await RecordLog("单位", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
            exEntity.RECDATE = DateTime.Now;
            exEntity.RECMAN = LoginUser.UsersId;
            return await unitService.Update(exEntity);
        }
        public async Task<bool> Delete(UnitExEntity exEntity)
        {
            await RecordLog("单位", exEntity, Infrastructure.Enums.OperateTypeEnum.删除);
            return await unitService.Delete(new List<UnitExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<UnitExEntity> exEntities)
        {
            await RecordLog("单位", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            return await unitService.Delete(exEntities);
        }
    }
}