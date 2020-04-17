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
    public class InterfaceRepository : BaseRepository<InterfaceEntity>, IInterfaceRepository
    {
        public async Task<Pagination<InterfaceEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<InterfaceEntity>();
            var list = new List<InterfaceEntity>();
            var totalSql = $"select count(1) from tbl_interface a where 1=1 ";
            var sql = "select a.*,b.* from tbl_interface a left join tbl_interface_right b on a.ip=b.app_id where 1=1";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            await Connection.QueryAsync<InterfaceEntity, InterfaceRightEntity, InterfaceEntity>(sql, (a, b) =>
             {
                 var interfaceEntity = list.FirstOrDefault(x => x.App_Id == a.App_Id);
                 if (interfaceEntity == null)
                 {
                     a.RightEntities.Add(b);
                     list.Add(a);
                 }
                 else
                 {
                     interfaceEntity.RightEntities.Add(b);
                 }
                 return a;
             }, splitOn: "app_id");
            pagination.Data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }
    }
}
