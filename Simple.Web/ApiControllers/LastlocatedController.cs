﻿using Microsoft.AspNetCore.Mvc;
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

        public LastlocatedController(ILastLocatedService lastLocatedService)
        {
            this.lastLocatedService = lastLocatedService;
        }

        public async Task<LastLocatedModel> Get(string mac)
        {
            try
            {
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