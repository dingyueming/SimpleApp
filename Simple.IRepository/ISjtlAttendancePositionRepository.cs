using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;

namespace Simple.IRepository
{
    public interface ISjtlAttendancePositionRepository : IBaseRepository<SjtlAttendancePositionEntity>
    {
        Task<List<SjtlAttendancePositionEntity>> GetEntities(DateTime startTime, DateTime endTime, double[] startPoint, double[] endPoint, string nameOrPoliceCode);
    }
}
