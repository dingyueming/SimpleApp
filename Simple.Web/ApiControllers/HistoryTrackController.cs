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
    public class HistoryTrackController : ControllerBase
    {
        private readonly INewTrackService newTrackService;

        public HistoryTrackController(INewTrackService newTrackService)
        {
            this.newTrackService = newTrackService;
        }

        public async Task<dynamic> Get(string keyword, string startTime, string endTime)
        {
            try
            {
                var timespan = DateTime.Parse(endTime) - DateTime.Parse(startTime);
                if (timespan.TotalDays > 3)
                {
                    return "间隔时间不能大于3天";
                }

                var list = new List<LastLocatedModel>();
                var entities = await newTrackService.GetHistoryTrackList(keyword, DateTime.Parse(startTime), DateTime.Parse(endTime));
                if (entities != null && entities.ToList().Count > 0)
                {
                    foreach (var entity in entities.ToList())
                    {
                        list.Add(new LastLocatedModel
                        {
                            Gnsstime = entity.GNSSTIME,
                            Heading = (int)entity.HEADING,
                            Latitude = entity.LATITUDE,
                            Longitude = entity.LONGITUDE,
                            Mac = entity.Device.MAC,
                            Speed = (int)entity.SPEED,
                            Name = entity.Device.LICENSE
                        });
                    }
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