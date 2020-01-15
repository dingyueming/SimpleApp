using Dapper;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple.Repository
{
    public class MenusRepository : BaseRepository<MenusEntity>, IMenusRepository
    {
        public async Task<Pagination<MenusEntity>> GetMenuPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<MenusEntity>();
            string totalSql = $"select count(1) from tb_menus where 1=1 ";
            var sql = "select a.*,b.* from tb_menus a left join tb_users b on a.creator=b.usersid where 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<MenusEntity, UsersEntity, MenusEntity>(sql, (a, b) =>
            {
                a.User = b;
                return a;
            }, splitOn: "UsersId");
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }

        public async Task<List<MenusEntity>> GetMenusByUser(int usersId)
        {
            var sql = "select m.* from tb_menus m  join tb_rolemenu rm on m.menusid=rm.menusid WHERE RM.ROLESID=(select ur.rolesid from tb_usersrole ur where ur.usersid=:userId)";
            var listMenus = await Connection.QueryAsync<MenusEntity>(sql, new { userId = usersId });
            return listMenus.AsList();
        }
    }
}
