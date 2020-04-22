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
    public class OperateLogService : IOperateLogService
    {
        private readonly ILogDomainService logDomainService;
        public OperateLogService(ILogDomainService logDomainService)
        {
            this.logDomainService = logDomainService;
        }
        public async Task AddLog(OperateLogExEntity exEntity)
        {
            await logDomainService.AddLog(exEntity);
        }

        public async Task<Pagination<OperateLogExEntity>> GetLogPage(Pagination<OperateLogExEntity> param)
        {
            return await logDomainService.GetLogPage(param);
        }
    }
}
