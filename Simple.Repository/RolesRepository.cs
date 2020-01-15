using Dapper;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Repository
{
    public class RolesRepository : BaseRepository<RolesEntity>, IRolesRepository
    {
        public async Task<Pagination<RolesEntity>> GetRolePage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<RolesEntity>();
            string totalSql = $"select count(1) from tb_roles where 1=1 ";
            var sql = "select a.*,b.* from tb_roles a left join tb_users b on a.creator=b.usersid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<RolesEntity, UsersEntity, RolesEntity>(sql, (a, b) =>
            {
                a.User = b;
                return a;
            }, splitOn: "UsersId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }
    }
}
