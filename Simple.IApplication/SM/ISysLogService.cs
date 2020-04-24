using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SM
{
    public interface ISysLogService
    {
        Task<Pagination<SysLogExEntity>> GetLogPage(Pagination<SysLogExEntity> param);
    }
}
