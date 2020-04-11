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
        private IViewAllTargetService viewAllTargetService;
        public DeviceListController(IViewAllTargetService viewAllTargetService)
        {
            this.viewAllTargetService = viewAllTargetService;
        }
        public async Task<List<DeviceModel>> Get(string keyword)
        {
            try
            {
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