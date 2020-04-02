using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.IDomain;
using Simple.Infrastructure.QueryModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.MapShow
{
    public class Sjgx110AlarmService : ISjgx110AlarmService
    {
        private IMapShowDomainService mapShowDomainService;
        public Sjgx110AlarmService(IMapShowDomainService mapShowDomainService)
        {
            this.mapShowDomainService = mapShowDomainService;
        }
        public async Task<List<Sjgx110AlarmExEntity>> GetSjgx110AlarmExEntities(Sjgx110AlarmQm queryModel)
        {
            return await mapShowDomainService.GetSjgx110AlarmExEntities(queryModel);
        }
    }
}
