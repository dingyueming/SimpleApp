using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Dapper;
using Simple.IRepository;
using Simple.Infrastructure.Tools;
using System.Linq;

namespace Simple.Repository
{
    public class UsersRepository : BaseRepository<UsersEntity>, IUserRepository
    {
        public async Task<Pagination<UsersEntity>> GetUserPage(int pageSize, int pageIndex, string orderby, string where)
        {
            var pagination = new Pagination<UsersEntity>();
            string totalSql = $"select count(1) from tb_users where 1=1";
            var sql = "select a.*,b.* from tb_users a left join tb_users b on a.creator=b.usersid where 1=1";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += orderby;
            }
            var list = await Connection.QueryAsync<UsersEntity, UsersEntity, UsersEntity>(sql, (a, b) =>
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
