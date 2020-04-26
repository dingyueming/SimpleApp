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
    public class NewAlarmInfoRepository : BaseRepository<NewAlarmInfoEntity>, INewAlarmInfoRepository
    {
        public async Task<Pagination<NewAlarmInfoEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<NewAlarmInfoEntity>();
            var totalSql = $"select count(1) from new_alarminfo a join cars b on a.carid=b.carid join area c on a.areaid=c.areaid where 1=1 ";
            var sql = "select a.*,b.*,c.* from new_alarminfo a join cars b on a.carid=b.carid join area c on a.areaid=c.areaid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<NewAlarmInfoEntity, CarEntity, AreaEntity, NewAlarmInfoEntity>(sql, (a, b, c) =>
            {
                a.Car = b;
                a.Area = c;
                return a;
            }, splitOn: "carid,areaid");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }
    }
}
