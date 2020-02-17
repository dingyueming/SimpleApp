using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Dapper;
using Simple.IRepository;
using Simple.Infrastructure.Tools;
using System.Linq;
using System.Collections.Generic;

namespace Simple.Repository
{
    public class UsersRepository : BaseRepository<UsersEntity>, IUserRepository
    {
        public async Task<Pagination<UsersEntity>> GetUserPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<UsersEntity>();
            string totalSql = $"select count(1) from tb_users where 1=1";
            var sql = "select a.*,b.*,c.* from tb_users a left join tb_users b on a.creator=b.usersid left join tb_usersrole ur on a.usersid=ur.usersid left join tb_roles c on ur.rolesid=c.rolesid  where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<UsersEntity, UsersEntity, RolesEntity, UsersEntity>(sql, (a, b, c) =>
              {
                  a.User = b;
                  a.Role = c;
                  return a;
              }, splitOn: "UsersId,RolesId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public async Task<List<UsersEntity>> GetUsersEntityByUserName(string userName)
        {
            var sql = $"select * from tb_users where usersname like '%{userName.Trim()}%'";
            var entities = await Connection.QueryAsync<UsersEntity>(sql);
            return entities.AsList();
        }
    }
}
