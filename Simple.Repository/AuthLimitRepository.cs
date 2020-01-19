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
    public class AuthLimitRepository : BaseRepository<AuthLimitsEntity>, IAuthLimitRepository
    {
        public async Task<bool> UpdateAuthLimits(List<AuthLimitsEntity> entities, int userId)
        {
            var trans = Connection.BeginTransaction();
            try
            {
                var delSql = "delete from auth_limits t where t.userid=:userid";
                await Connection.ExecuteAsync(delSql, new { userid = userId });
                await this.InsertAsync(entities);
                trans.Commit();
                return true;
            }
            catch (Exception)
            {
                trans.Rollback();
            }
            return false;
        }
    }
}
