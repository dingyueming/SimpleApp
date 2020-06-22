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
    public class CarMsgReportRepository : BaseRepository<CarMsgReportEntity>, ICarMsgReportRepository
    {
        public async Task<Pagination<CarMsgReportEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<CarMsgReportEntity>();
            var totalSql = $"select count(1) from car_msgreport a left join cars b on a.carid=b.carid left join tb_users c on a.creator=c.usersid where 1=1 ";
            var sql = "select a.*,b.*,c.* from car_msgreport a left join cars b on a.carid=b.carid left join tb_users c on a.creator=c.usersid  where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<CarMsgReportEntity, CarEntity, UsersEntity, CarMsgReportEntity>(sql, (a, b, c) =>
            {
                a.Car = b;
                a.CreateUser = c;
                return a;
            }, splitOn: "carid,usersid");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public async Task<List<CarMsgReportEntity>> GetEntities(DateTime[] dateTimes, string carNo)
        {
            var sql = "select a.*,b.* from car_msgreport a left join cars b on a.carid=b.carid where b.carno like :carNo and a.sendtime between :starttime and :endtime";
            var result = await Connection.QueryAsync<CarMsgReportEntity, CarEntity, CarMsgReportEntity>(sql, (a, b) =>
             {
                 a.Car = b;
                 return a;
             }, splitOn: "carid", param: new { starttime = dateTimes[0], endtime = dateTimes[1], carNo = $"%{carNo}%" });
            return result.ToList();
        }
    }
}
