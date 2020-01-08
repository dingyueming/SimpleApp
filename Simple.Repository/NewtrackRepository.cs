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
            sb.Append("SELECT * from newtrack t where 1=1 and t.carid=:deviceid and t.gnsstime between :starttime and :endtime and t.speed>:minspeed");
            if ((bool)queryModel.ZeroSpeed)
            {
                sb.Append(" and t.speed<>0");
            }
            sb.Append(" order by t.gnsstime");
            var list = await Connection.QueryAsync<NewTrackEntity>(sb.ToString(), new { deviceid = queryModel.DeviceId, starttime = queryModel.StartTime, endtime = queryModel.EndTime, minspeed = queryModel.MinSpeed });
            return list.AsList();
        }
    }
}
