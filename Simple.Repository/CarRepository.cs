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
    public class CarRepository : BaseRepository<CarEntity>, ICarRepository
    {
        public async Task<List<CarEntity>> GetCarEntitiesByUser(int userId)
        {
            var sql = "SELECT C.* FROM CARS C JOIN AUTH_LIMITS A ON C.CARID=A.CARID WHERE A.USERID=:USERID";
            var users = await base.Connection.QueryAsync<CarEntity>(sql, new { USERID = userId });
            return users.AsList();
        }

        public async Task<Pagination<CarEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<CarEntity>();
            string totalSql = $"select count(1) from cars a where 1=1 ";
            var sql = "select a.*,b.* from cars a left join unit b on a.unitid=b.unitid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<CarEntity, UnitEntity, CarEntity>(sql, (a, b) =>
            {
                a.Unit = b;
                return a;
            }, splitOn: "UnitId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            if (pagination.Data.Count > 0)
            {
                //查询车辆的报警区域
                sql = $"select a.*,c.* from cars a left join  car_area b on a.carid=b.carid left join area c on b.areaid=c.areaid  where b.status=1 and a.carid in ({string.Join(',', pagination.Data.Select(x => x.CARID).ToArray())})";
                list = await Connection.QueryAsync<CarEntity, AreaEntity, CarEntity>(sql, (a, b) =>
                {
                    var car = pagination.Data.Find(x => x.CARID == a.CARID);
                    if (b != null)
                    {
                        car.Areas.Add(b);
                    }
                    return a;
                }, splitOn: "areaid");
            }

            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public async Task<CarEntity> GetCarEntityForValdata(CarEntity car)
        {
            var sql = "select * from cars c where (c.license=:license or c.mac=:mac or c.sim=:sim)";
            var entity = await Connection.QuerySingleOrDefaultAsync<CarEntity>(sql,
                    new { license = car.LICENSE, mac = car.MAC, sim = car.SIM});
            return entity;
        }
    }
}
