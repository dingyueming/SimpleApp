using Dapper;
using Simple.Entity;
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

        public async Task<List<ViewAllTargetEntity>> GetAllDevice()
        {
            var sql = "select v.* from view_all_target v";
            var devices = await Connection.QueryAsync<ViewAllTargetEntity>(sql);
            return devices.AsList();
        }

        public async Task<ViewAllTargetEntity> GetViewAllTargetByKeyword(string keyword)
        {
            var sql = "select v.* from view_all_target v where v.mac=:keyword or v.license=:keyword";
            return await Connection.QueryFirstOrDefaultAsync<ViewAllTargetEntity>(sql, new { keyword });
        }

        public async Task<List<ViewAllTargetEntity>> GetViewAllTarget(string[] orgCodes)
        {
            var sql = "select v.* from view_all_target v where v.org_code in :orgCodes";
            var entities = await Connection.QueryAsync<ViewAllTargetEntity>(sql, new { orgCodes });
            return entities.AsList();
        }
    }
}
