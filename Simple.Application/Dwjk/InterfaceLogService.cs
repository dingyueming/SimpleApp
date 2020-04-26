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
    public class InterfaceLogService : IInterfaceLogService
    {
        private readonly IIfDomainService ifDomainService;
        public InterfaceLogService(IIfDomainService ifDomainService)
        {
            this.ifDomainService = ifDomainService;
        }

        public async Task<Pagination<InterfaceLogExEntity>> GetPage(Pagination<InterfaceLogExEntity> param)
        {
            return await ifDomainService.GetInterfaceLogPage(param);
        }
    }
}
