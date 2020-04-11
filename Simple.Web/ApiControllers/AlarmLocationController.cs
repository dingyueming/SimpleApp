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
    public class AlarmLocationController : ControllerBase
    {
        private ISjgx110AlarmService sjgx110AlarmService;
        public AlarmLocationController(ISjgx110AlarmService sjgx110AlarmService)
        {
            this.sjgx110AlarmService = sjgx110AlarmService;
        }
        public async Task<List<AlarmLocationModel>> Get(string startTime, string endTime)
        {
            try
            {
                var list = new List<AlarmLocationModel>();
                var exEntities = await sjgx110AlarmService.GetAlarmPositionList(DateTime.Parse(startTime), DateTime.Parse(endTime));
                if (exEntities != null && exEntities.Count > 0)
                {
                    exEntities.ForEach((exEntity) =>
                    {
                        list.Add(new AlarmLocationModel
                        {
                            Bjrxm = exEntity.Bjrxm,
                            Bjsj = exEntity.Bjsj,
                            Gxdw = exEntity.Gxdw.UNITNAME,
                            Jd = exEntity.Jd,
                            Wd = exEntity.Wd,
                            Jjdbh = exEntity.Jjdbh,
                            Jjdw = exEntity.Jjdw.UNITNAME,
                            Lxdh = exEntity.Lxdh
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