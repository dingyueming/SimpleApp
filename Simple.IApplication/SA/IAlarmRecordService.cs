using Simple.ExEntity.Map;
using Simple.ExEntity.SA;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SA
{
    public interface IAlarmRecordService
    {
        Task<Pagination<AlarmRecordExEntity>> GetPage(Pagination<AlarmRecordExEntity> param);
    }
}
