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
    public class DeviceDetailController : ControllerBase
    {
        private IViewAllTargetService viewAllTargetService;
        public DeviceDetailController(IViewAllTargetService viewAllTargetService)
        {
            this.viewAllTargetService = viewAllTargetService;
        }
        public async Task<DeviceModel> Get(string keyword)
        {
            try
            {
                var exEntity = await viewAllTargetService.GetViewAllTarget(keyword);
                if (exEntity != null)
                {
                    return new DeviceModel
                    {
                        Mac = exEntity.MAC,
                        Name = exEntity.LICENSE,
                        OrgCode = exEntity.ORG_CODE,
                        OrgName = exEntity.UNITNAME
                    };
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