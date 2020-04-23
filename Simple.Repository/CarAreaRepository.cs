using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace Simple.Repository
{
    public class CarAreaRepository : BaseRepository<CarAreaEntity>, ICarAreaRepository
    {
        public async Task InsertCarArea(CarAreaEntity entity)
        {
            var delSql = "delete from car_area c where c.carid=:carid and c.areaid=:areaid";
            var insSql = @"insert into car_area
                          (carid, areaid, status, alarmtype, alarmdelaytime, errorscope, mobileareaid, areaflag, overspeed, openlocknumber)
                        values
                          (:carid, :areaid, :status, :alarmtype, :alarmdelaytime, :errorscope, :mobileareaid, :areaflag, :overspeed, :openlocknumber)";
            var trans = Connection.BeginTransaction();
            try
            {
                await Connection.ExecuteAsync(delSql, new { carid = entity.CARID, areaid = entity.AREAID });
                await Connection.ExecuteAsync(insSql, entity);
                trans.Commit();
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw e;
            }
        }
        public async Task BatchInsertCarArea(List<CarAreaEntity> entities)
        {
            var insSql = @"insert into car_area
                          (carid, areaid, status, alarmtype, alarmdelaytime, errorscope, mobileareaid, areaflag, overspeed, openlocknumber)
                        values
                          (:carid, :areaid, :status, :alarmtype, :alarmdelaytime, :errorscope, :mobileareaid, :areaflag, :overspeed, :openlocknumber)";
            try
            {
                await Connection.ExecuteAsync(insSql, entities);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async override Task<bool> DeleteAsync(CarAreaEntity entity)
        {
            var sql = "delete from car_area t where t.carid=:carid and t.areaid=:areaid";
            var count = await Connection.ExecuteAsync(sql, new { carid = entity.CARID, areaid = entity.AREAID });
            return count > 0;
        }
    }
}
