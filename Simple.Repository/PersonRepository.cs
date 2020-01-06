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
    public class PersonRepository : BaseRepository<PersonEntity>, IPersonRepository
    {
        public async Task<List<PersonEntity>> GetPersonEntitiesByUser(int userId)
        {
            var sql = "SELECT C.* FROM PERSONS C JOIN AUTH_LIMITS A ON C.ID=A.CARID WHERE A.USERID=:USERID";
            var persons = await Connection.QueryAsync<PersonEntity>(sql, new { USERID = userId });
            return persons.AsList();
        }
    }
}
