using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SM
{
    public interface IOperateLogService
    {
        Task AddLog(OperateLogExEntity exEntity);
        Task<Pagination<OperateLogExEntity>> GetLogPage(Pagination<OperateLogExEntity> param);
    }
}
