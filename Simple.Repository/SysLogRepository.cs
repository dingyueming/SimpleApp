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
    public class SysLogRepository : BaseRepository<SysLogEntity>, ISysLogRepository
    {
        public async Task<Pagination<SysLogEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby)
        {
            var pagination = new Pagination<SysLogEntity>();
            var totalSql = $"select count(1) from TB_SYSLOG a where 1=1 ";
            var sql = "select a.* from TB_SYSLOG a  where 1=1";
            if (!string.IsNullOrEmpty(where))
            {
                sql += where;
                totalSql += where;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += $" order by {orderby}";
            }
            var list = await Connection.QueryAsync<SysLogEntity>(sql);
            pagination.Data = list.AsList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            pagination.Total = await Connection.QuerySingleAsync<int>(totalSql);
            return pagination;
        }
    }
}
