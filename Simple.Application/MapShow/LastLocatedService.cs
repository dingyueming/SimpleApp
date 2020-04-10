using Simple.ExEntity.Map;
using Simple.IApplication.MapShow;
using Simple.IDomain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.MapShow
{
    public class LastLocatedService : ILastLocatedService
    {
        private readonly IMapShowDomainService mapShowDomainService;
        public LastLocatedService(IMapShowDomainService mapShowDomainService)
        {
            this.mapShowDomainService = mapShowDomainService;
        }

        public async Task<LastLocatedExEntity> GetLastLocatedByMac(string mac)
        {
            return await mapShowDomainService.GetLastLocatedByMac(mac);
        }
    }
}
