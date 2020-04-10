using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.MapShow;

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

        public async Task<dynamic> Get(string mac)
        {
            var entity = await lastLocatedService.GetLastLocatedByMac(mac);
            return new { entity.License, entity.Mac, entity.LONGITUDE, entity.LATITUDE, entity.GNSSTIME, entity.HEADING, entity.SPEED };
        }
    }
}