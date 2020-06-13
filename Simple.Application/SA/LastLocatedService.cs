using Simple.ExEntity.Map;
using Simple.IApplication.SA;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Threading.Tasks;

namespace Simple.Application.SA
{
    public class LastLocatedService : ILastLocatedService
    {
        private readonly ISaDomainService saDomainService;
        public LastLocatedService(ISaDomainService saDomainService)
        {
            this.saDomainService = saDomainService;
        }

        public async Task<Pagination<LastLocatedExEntity>> GetPage(Pagination<LastLocatedExEntity> param, DateTime[] dateTimes)
        {
            var page = await saDomainService.GetLastLocatedPage(param, dateTimes);
            return page;
        }
    }
}
