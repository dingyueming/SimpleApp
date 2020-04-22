using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.Dwjk;
using Simple.Web.ApiControllers.Models;

namespace Simple.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceListController : ControllerBase
    {
        private IInterfaceService interfaceService;
        private IViewAllTargetService viewAllTargetService;
        public DeviceListController(IInterfaceService interfaceService, IViewAllTargetService viewAllTargetService)
        {
            this.interfaceService = interfaceService;
            this.viewAllTargetService = viewAllTargetService;
        }
        public async Task<List<DeviceModel>> Get(string auth, string keyword)
        {
            try
            {
                #region 认证编码验证

                var pagination = await interfaceService.GetPage(new Infrastructure.InfrastructureModel.Paionation.Pagination<ExEntity.IM.InterfaceExEntity>() { Where = $" and a.password='{auth}'" });
                if (pagination.Total == 0)
                {
                    return null;
                }

                #endregion
                var list = new List<DeviceModel>();
                var exEntities = await viewAllTargetService.GetViewAllTargetListByOrgcode(keyword);
                if (exEntities != null && exEntities.Count > 0)
                {
                    exEntities.ForEach((exEntity) =>
                    {
                        list.Add(new DeviceModel
                        {
                            Mac = exEntity.MAC,
                            Name = exEntity.LICENSE,
                            OrgCode = exEntity.ORG_CODE,
                            OrgName = exEntity.UNITNAME
                        });
                    });
                    return list;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}