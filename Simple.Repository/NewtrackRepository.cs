using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using Dapper;

namespace Simple.Repository
{
    public class NewtrackRepository : BaseRepository<NewTrackEntity>, INewtrackRepository
    {
        public async Task<List<NewTrackEntity>> GetNewtracksByDeviceId(dynamic queryModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * from newtrack t join cars v on t.carid =v.carid where 1=1 and t.carid=:deviceid and t.gnsstime between :starttime and :endtime and t.speed>=:minspeed");
            sb.Append(" order by t.gnsstime");
            var command = new CommandDefinition(sb.ToString(), new { deviceid = queryModel.DeviceId, starttime = (DateTime)queryModel.TimeValue[0], endtime = (DateTime)queryModel.TimeValue[1], minspeed = queryModel.MinSpeed });
            var list = await Connection.QueryAsync<NewTrackEntity, CarEntity, NewTrackEntity>(command, (a, b) =>
            {
                a.Device = b;
                return a;
            }, splitOn: "carid");
            return list.AsList();
        }

        public async Task<List<NewTrackEntity>> GetNewTrackEntities(string keyword, DateTime startTime, DateTime endTime)
        {
            var sql = $"select t.*,v.* from newtrack t join view_all_target v on t.carid =v.carid where t.gnsstime between :starttime and :endtime and (v.mac=:keyword or v.license=:keyword)";
            var command = new CommandDefinition(sql, new { startTime, endTime, keyword });
            var list = await Connection.QueryAsync<NewTrackEntity, CarEntity, NewTrackEntity>(command, (a, b) =>
            {
                a.Device = b;
                return a;
            }, splitOn: "carid");
            return list.ToList();
        }

    }
}
