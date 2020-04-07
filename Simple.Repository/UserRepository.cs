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
            var sql = "select a.*,b.*,c.*,d.* from tb_users a left join tb_users b on a.creator=b.usersid left join tb_usersrole ur on a.usersid=ur.usersid left join tb_roles c on ur.rolesid=c.rolesid left join unit d on a.unitid=d.unitid  where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<UsersEntity, UsersEntity, RolesEntity, UnitEntity, UsersEntity>(sql, (a, b, c, d) =>
               {
                   a.User = b;
                   a.Role = c;
                   a.Unit = d;
                   return a;
               }, splitOn: "UsersId,RolesId,unitId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public async Task<List<UsersEntity>> GetUsersEntity(UsersEntity usersEntity)
        {
            var sql = $"select * from tb_users where usersname = '{usersEntity.UsersName.Trim()}' and usersid!={usersEntity.UsersId}";
            var entities = await Connection.QueryAsync<UsersEntity>(sql);
            return entities.AsList();
        }
    }
}
