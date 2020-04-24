using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.SM
{
    public class SysLogService : ISysLogService
    {
        private readonly ILogDomainService logDomainService;
        public SysLogService(ILogDomainService logDomainService)
        {
            this.logDomainService = logDomainService;
        }

        public async Task<Pagination<SysLogExEntity>> GetLogPage(Pagination<SysLogExEntity> param)
        {
            return await logDomainService.GetSysLogPage(param);
        }
    }
}
