using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;

namespace Simple.IRepository
{
    public interface ISjgx110AlarmRepository : IBaseRepository<Sjgx110AlarmEntity>
    {
        Task<List<Sjgx110AlarmEntity>> GetAlarmEntities(DateTime startTime, DateTime endTime, double[] startPoint, double[] endPoint);
    }
}
