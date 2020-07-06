using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;

namespace Simple.IRepository
{
    public interface IAlarmRecordRepository : IBaseRepository<AlarmRecordEntity>
    {
        Task<List<AlarmRecordEntity>> GetEntities(int? unitId, DateTime startTime, DateTime endTime, int? alarmType);
    }
}
