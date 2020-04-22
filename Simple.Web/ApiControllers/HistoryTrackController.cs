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
        private readonly IInterfaceService interfaceService;
        private readonly INewTrackService newTrackService;

        public HistoryTrackController(IInterfaceService interfaceService, INewTrackService newTrackService)
        {
            this.interfaceService = interfaceService;
            this.newTrackService = newTrackService;
        }
        public async Task<dynamic> Get(string auth, string keyword, string startTime, string endTime)
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