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
            var sql = "SELECT V.mac,N.* FROM NEWTRACK_LASTLOCATED N JOIN VIEW_ALL_TARGET V ON V.carid=N.CARID JOIN AUTH_LIMITS A ON N.CARID= A.CARID WHERE N.LOCATE =1 AND A.USERID=:USERID";
            var entities = await Connection.QueryAsync<LastLocatedEntity>(sql, new { USERID = userId });
            return entities.AsList();
        }
    }
}
