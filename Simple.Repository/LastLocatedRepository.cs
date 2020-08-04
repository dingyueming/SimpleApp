using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Simple.Entity;

namespace Simple.Repository
{
    public class LastLocatedRepository : BaseRepository<LastLocatedEntity>, ILastLocatedRepository
    {
        public async Task<List<LastLocatedEntity>> GetLastLocatedEntityByUser(int userId)
        {
            var sql = "SELECT V.mac,N.* FROM NEWTRACK_LASTLOCATED N JOIN CARS V ON V.carid=N.CARID JOIN AUTH_LIMITS A ON N.CARID= A.CARID WHERE N.gnsstime >= sysdate-5 AND N.LOCATE =1 AND A.USERID=:USERID";
            var entities = await Connection.QueryAsync<LastLocatedEntity>(sql, new { USERID = userId });
            return entities.AsList();
        }

        public async Task<List<LastLocatedEntity>> GetLastLocatedByUser(int userId)
        {
            var sql = "SELECT V.mac,N.* FROM NEWTRACK_LASTLOCATED N JOIN CARS V ON V.carid=N.CARID JOIN AUTH_LIMITS A ON N.CARID= A.CARID WHERE 1=1 AND A.USERID=:USERID";
            var entities = await Connection.QueryAsync<LastLocatedEntity>(sql, new { USERID = userId });
            return entities.AsList();
        }

        public async Task<LastLocatedEntity> GetEntityByMac(string mac)
        {
            var sql = "select t.*,c.mac,c.license from newtrack_lastlocated t join  view_all_target c on t.carid=c.carid where c.mac=:mac";
            var entity = await Connection.QueryFirstOrDefaultAsync<LastLocatedEntity>(sql, new { mac });
            return entity;
        }

        public async Task<LastLocatedEntity> GetEntityByKeyword(string keyword)
        {
            var sql = "select t.*,c.mac,c.license from newtrack_lastlocated t join  view_all_target c on t.carid=c.carid where c.mac=:keyword or c.license=:keyword";
            var entity = await Connection.QueryFirstOrDefaultAsync<LastLocatedEntity>(sql, new { keyword });
            return entity;
        }
    }
}
