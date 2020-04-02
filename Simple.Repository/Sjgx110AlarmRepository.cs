﻿using Simple.Infrastructure.Dapper.Contrib;
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
    public class Sjgx110AlarmRepository : BaseRepository<Sjgx110AlarmEntity>, ISjgx110AlarmRepository
    {
        public async Task<List<Sjgx110AlarmEntity>> GetAlarmEntities(DateTime startTime, DateTime endTime, double[] startPoint, double[] endPoint)
        {
            var sql = "select t.* from sjgx_110_alarm t where t.bjsj between :startTime and :endTime";
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
            var entities = await Connection.QueryAsync<Sjgx110AlarmEntity>(sql, param);
            return entities.ToList();
        }
    }
}
