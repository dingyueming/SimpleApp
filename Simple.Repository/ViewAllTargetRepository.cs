using Dapper;
using Simple.Entity.Map;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Repository
{
    public class ViewAllTargetRepository : BaseRepository<ViewAllTargetEntity>, IViewAllTargetRepository
    {
        public async Task<List<ViewAllTargetEntity>> GetDevicesByUser(int userId)
        {
            var sql = "select v.* from view_all_target v join auth_limits a on v.carid=a.carid where a.userid=:userid";
            var devices = await Connection.QueryAsync<ViewAllTargetEntity>(sql, new { userid = userId });
            return devices.AsList();
        }
    }
}
