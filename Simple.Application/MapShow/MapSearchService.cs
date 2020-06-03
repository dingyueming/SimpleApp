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
    public class MapSearchService : IMapSearchService
    {
        private IMapShowDomainService mapShowDomainService;
        public MapSearchService(IMapShowDomainService mapShowDomainService)
        {
            this.mapShowDomainService = mapShowDomainService;
        }
        public async Task<List<XfKeyUnitExEntity>> GetXfKeyUnitExEntities()
        {
            return await mapShowDomainService.GetXfKeyUnitExEntities();
        }

        public async Task<List<XfSyxxExEntity>> GetXfSyxxExEntities()
        {
            return await mapShowDomainService.GetXfSyxxExEntities();
        }
    }
}
