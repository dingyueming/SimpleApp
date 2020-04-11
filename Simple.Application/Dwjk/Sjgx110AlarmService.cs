using Simple.ExEntity.Map;
using Simple.IApplication.Dwjk;
using Simple.IDomain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Application.Dwjk
{
    public class Sjgx110AlarmService : ISjgx110AlarmService
    {
        private readonly IIfDomainService ifDomainService;
        public Sjgx110AlarmService(IIfDomainService ifDomainService)
        {
            this.ifDomainService = ifDomainService;
        }

        public async Task<List<Sjgx110AlarmExEntity>> GetAlarmPositionList(DateTime startTime, DateTime endTime)
        {
            return await ifDomainService.GetAlarmPositionList(startTime, endTime);
        }
    }
}
