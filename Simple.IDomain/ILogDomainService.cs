using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    public interface ILogDomainService
    {
        #region 日志信息管理

        Task AddLog(OperateLogExEntity exEntity);
        Task DeleteOperateLog(List<OperateLogExEntity> exEntities);
        Task UpdateOperateLog(OperateLogExEntity exEntity);
        Task<Pagination<OperateLogExEntity>> GetLogPage(Pagination<OperateLogExEntity> param);

        #endregion

        #region 系统日志
        Task<Pagination<SysLogExEntity>> GetSysLogPage(Pagination<SysLogExEntity> param);
        #endregion

    }
}
