using Dapper;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.IRepository;
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
    }
}
