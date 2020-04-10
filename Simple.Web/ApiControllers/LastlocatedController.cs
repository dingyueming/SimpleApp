using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.Dwjk;
using Simple.Web.Models;
using Simple.Web.Models.OutputModels;
using System;
using System.Threading.Tasks;

namespace Simple.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LastlocatedController : ControllerBase
    {
        private readonly ILastLocatedService lastLocatedService;

        public LastlocatedController(ILastLocatedService lastLocatedService)
        {
            this.lastLocatedService = lastLocatedService;
        }

        public async Task<OutputBaseModel<LastLocatedModel>> Get(string mac)
        {
            var output = new OutputBaseModel<LastLocatedModel>();
            try
            {
                var entity = await lastLocatedService.GetLastLocatedByMac(mac);
                if (entity != null)
                {
                    output.Data = new LastLocatedModel
                    {
                        Gnsstime = entity.GNSSTIME,
                        Heading = (int)entity.HEADING,
                        Latitude = entity.LATITUDE,
                        Longitude = entity.LONGITUDE,
                        Mac = entity.Mac,
                        Speed = (int)entity.SPEED,
                        License = entity.License
                    };
                }
            }
            catch (Exception e)
            {
                output.Status = 0;
                output.Message = e.Message;
            }
            return output;
        }
    }
}