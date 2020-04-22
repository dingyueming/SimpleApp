using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.Dwjk;
using Simple.Web.ApiControllers.Models;
using System;
using System.Threading.Tasks;

namespace Simple.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LastlocatedController : ControllerBase
    {
        private readonly ILastLocatedService lastLocatedService;
        private readonly IInterfaceService interfaceService;
        public LastlocatedController(IInterfaceService interfaceService, ILastLocatedService lastLocatedService)
        {
            this.interfaceService = interfaceService;
            this.lastLocatedService = lastLocatedService;
        }
        public async Task<LastLocatedModel> Get(string auth, string mac)
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
                var entity = await lastLocatedService.GetLastLocatedByMac(mac);
                if (entity != null)
                {
                    return new LastLocatedModel
                    {
                        Gnsstime = entity.GNSSTIME,
                        Heading = (int)entity.HEADING,
                        Latitude = entity.LATITUDE,
                        Longitude = entity.LONGITUDE,
                        Mac = entity.Mac,
                        Speed = (int)entity.SPEED,
                        Name = entity.License
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