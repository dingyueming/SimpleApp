using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Simple.Entity;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System.Linq;

namespace Simple.Repository
{
    public class AlarmRecordRepository : BaseRepository<AlarmRecordEntity>, IAlarmRecordRepository
    {
        public async Task<List<AlarmRecordEntity>> GetEntities(int? unitId, DateTime startTime, DateTime endTime, int? alarmType)
        {
            var sql = "SELECT A.*,B.*,C.* FROM ALARM_RECORD A JOIN CARS B ON A.CARID=B.CARID LEFT JOIN UNIT C ON B.UNITID=C.UNITID WHERE A.ALARM_TIME BETWEEN :startTime AND :endTime ";

            if (unitId.HasValue)
            {
                sql += " AND C.UNITID IN (SELECT UNITID FROM UNIT START WITH UNITID = :unitId CONNECT BY PRIOR UNITID = PID) ";
            }
            if (alarmType.HasValue)
            {
                sql += " AND A.RECORD_EVENT=:alarmType ";
            }

            var command = new CommandDefinition(sql, new { alarmType = alarmType, startTime = startTime, endTime = endTime, unitId = unitId });
            var entities = await Connection.QueryAsync<AlarmRecordEntity, CarEntity, UnitEntity, AlarmRecordEntity>(command, (a, b, c) =>
              {
                  a.Car = b;
                  a.Unit = c;
                  return a;
              }, splitOn: "carid,UnitId");
            return entities.ToList();
        }
    }
}



