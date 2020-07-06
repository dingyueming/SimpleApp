using Simple.ExEntity.Map;
using Simple.ExEntity.SA;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IDomain
{
    public interface ISaDomainService
    {
        Task<Pagination<LastLocatedExEntity>> GetLastLocatedPage(Pagination<LastLocatedExEntity> param, DateTime[] dateTimes);

        Task<Pagination<AlarmRecordExEntity>> GetAlarmRecordPage(Pagination<AlarmRecordExEntity> param);
    }
}
