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
    public class SjtlAttendancePositionRepository : BaseRepository<SjtlAttendancePositionEntity>, ISjtlAttendancePositionRepository
    {
        public async Task<List<SjtlAttendancePositionEntity>> GetEntities(DateTime startTime, DateTime endTime, double[] startPoint, double[] endPoint, string nameOrPoliceCode)
        {
            var sql = "select t.* from sjtl_attendance_position t where t.check_time between :startTime and :endTime";
            var param = new DynamicParameters();
            param.Add("startTime", startTime, System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);
            param.Add("endTime", endTime, System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);
            if (startPoint != null && endPoint != null)
            {
                var longitudeArr = new double[] { startPoint[0], endPoint[0] }.OrderBy((x) => { return x; }).ToArray();
                var latitudeArr = new double[] { startPoint[1], endPoint[1] }.OrderBy((x) => { return x; }).ToArray();
                sql += " and t.jd>:longitudeStart and t.jd <:longitudeEnd and t.wd>:latitudeStart and t.wd<:latitudeEnd";
                param.Add("longitudeStart", longitudeArr[0], System.Data.DbType.Double, System.Data.ParameterDirection.Input, 9);
                param.Add("longitudeEnd", longitudeArr[1], System.Data.DbType.Double, System.Data.ParameterDirection.Input, 9);
                param.Add("latitudeStart", latitudeArr[0], System.Data.DbType.Double, System.Data.ParameterDirection.Input, 9);
                param.Add("latitudeEnd", latitudeArr[1], System.Data.DbType.Double, System.Data.ParameterDirection.Input, 9);
            }
            if (!string.IsNullOrWhiteSpace(nameOrPoliceCode))
            {
                sql += " and (t.code=:code or t.name like :name)";
                param.Add("code", nameOrPoliceCode, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                param.Add("name", nameOrPoliceCode, System.Data.DbType.String, System.Data.ParameterDirection.Input);
            }
            var entities = await Connection.QueryAsync<SjtlAttendancePositionEntity>(sql, param);
            return entities.ToList();
        }
    }
}
