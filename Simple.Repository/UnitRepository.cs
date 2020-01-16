using Dapper;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Repository
{
   public class UnitRepository : BaseRepository<UnitEntity>, IUnitRepository
    {
        public async Task<Pagination<UnitEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<UnitEntity>();
            string totalSql = $"select count(1) from unit a where 1=1 ";
            var sql = "select a.*,b.* from unit a left join unit b on a.pid=b.unitid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<UnitEntity, UnitEntity, UnitEntity>(sql, (a, b) =>
            {
                a.ParentUnit = b;
                return a;
            }, splitOn: "UnitId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }
    }
}
