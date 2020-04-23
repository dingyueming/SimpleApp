using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class PersonController : SimpleBaseController
    {
        private readonly IPersonService personService;
        public PersonController(IPersonService personService)
        {
            this.personService = personService;
        }
        public async Task<JsonResult> Query(Pagination<PersonExEntity> pagination)
        {
            var data = await personService.GetPage(pagination);
            return Json(data);
        }

        [SimpleAction]
        public async Task<bool> Add(PersonExEntity exEntity)
        {
            await RecordLog("对讲机", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
            return await personService.Add(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(PersonExEntity exEntity)
        {
            await RecordLog("对讲机", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
            return await personService.Update(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(PersonExEntity exEntity)
        {
            await RecordLog("对讲机", exEntity, Infrastructure.Enums.OperateTypeEnum.删除);
            return await personService.Delete(new List<PersonExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<PersonExEntity> exEntities)
        {
            await RecordLog("对讲机", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            return await personService.Delete(exEntities);
        }
    }
}