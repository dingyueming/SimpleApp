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
    public class CarRepository : BaseRepository<CarEntity>, ICarRepository
    {
        public async Task<List<CarEntity>> GetCarEntitiesByUser(int userId)
        {
            var sql = "SELECT C.* FROM CARS C JOIN AUTH_LIMITS A ON C.CARID=A.CARID WHERE A.USERID=:USERID";
            var users = await base.Connection.QueryAsync<CarEntity>(sql, new { USERID = userId });
            return users.AsList();
        }
    }
}
