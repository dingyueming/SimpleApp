using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Infrastructure.ControllerFilter;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class PersonController : SimpleBaseController
    {
        private readonly IPersonService personService;
        public PersonController(IPersonService personService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.personService = personService;
        }
        public async Task<JsonResult> Query(Pagination<PersonExEntity> pagination)
        {
            var data = await personService.GetPage(pagination);
            return Json(data);
        }

        [SimpleActionFilter]
        public async Task<bool> Add(PersonExEntity exEntity)
        {
            return await personService.Add(exEntity);
        }
        [SimpleActionFilter]
        public async Task<bool> Update(PersonExEntity exEntity)
        {
            return await personService.Update(exEntity);
        }
        [SimpleActionFilter]
        public async Task<bool> Delete(PersonExEntity exEntity)
        {
            return await personService.Delete(new List<PersonExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<PersonExEntity> exEntities)
        {
            return await personService.Delete(exEntities);
        }
    }
}