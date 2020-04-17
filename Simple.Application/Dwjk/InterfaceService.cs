using Simple.ExEntity.IM;
using Simple.ExEntity.Map;
using Simple.IApplication.Dwjk;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Application.Dwjk
{
    public class InterfaceService : IInterfaceService
    {
        private readonly IIfDomainService ifDomainService;
        public InterfaceService(IIfDomainService ifDomainService)
        {
            this.ifDomainService = ifDomainService;
        }

        public async Task Add(InterfaceExEntity exEntity)
        {
            await ifDomainService.AddInterface(exEntity);
        }

        public async Task Delete(List<InterfaceExEntity> exEntities)
        {
            await ifDomainService.DeleteInterface(exEntities);
        }

        public async Task<Pagination<InterfaceExEntity>> GetPage(Pagination<InterfaceExEntity> param)
        {
            return await ifDomainService.GetInterfacePage(param);
        }

        public async Task Update(InterfaceExEntity exEntity)
        {
            await ifDomainService.UpdateInterface(exEntity);
        }
    }
}
