using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.IM;
using Simple.IApplication.Dwjk;
using Simple.Infrastructure.ControllerFilter;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.IM.Controllers
{
    [Area("IM")]
    public class InterfaceManageController : SimpleBaseController
    {
        private readonly IInterfaceService interfaceService;
        public InterfaceManageController(IInterfaceService interfaceService)
        {
            this.interfaceService = interfaceService;
        }
        public async Task<JsonResult> Query(Pagination<InterfaceExEntity> pagination)
        {
            var data = await interfaceService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task SaveData(InterfaceExEntity exEntity)
        {
            if (exEntity.App_Id == 0)
            {
                await interfaceService.Add(exEntity);
            }
            else
            {
                await interfaceService.Update(exEntity);
            }
            
        }
        [SimpleAction]
        public async Task Delete(List<InterfaceExEntity> exEntities)
        {
            await interfaceService.Delete(exEntities);
        }
    }
}