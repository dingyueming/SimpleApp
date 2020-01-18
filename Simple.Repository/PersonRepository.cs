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
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        public async Task<List<PersonEntity>> GetPersonEntitiesByUser(int userId)
        {
            var sql = "SELECT C.* FROM PERSONS C JOIN AUTH_LIMITS A ON C.ID=A.CARID WHERE A.USERID=:USERID";
            var persons = await Connection.QueryAsync<PersonEntity>(sql, new { USERID = userId });
            return persons.AsList();
        }
        public async Task<Pagination<PersonEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<PersonEntity>();
            string totalSql = $"select count(1) from persons a where 1=1 ";
            var sql = "select a.*,b.* from persons a left join unit b on a.unitid=b.unitid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<PersonEntity, UnitEntity, PersonEntity>(sql, (a, b) =>
            {
                a.Unit = b;
                return a;
            }, splitOn: "UnitId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }
    }
}
