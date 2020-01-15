using Simple.Entity;
using Simple.Infrastructure;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Simple.Repository
{
    public class UsersRoleRepository : BaseRepository<UserRoleEntity>, IUsersRoleRepository
    {
        public async Task<UserRoleEntity> GetRoleEntityByUser(int usersId)
        {
            var sql = "select * from tb_usersrole t where t.usersid=:usersId";
            var entity = await Connection.QuerySingleOrDefaultAsync<UserRoleEntity>(sql, new { userId = usersId });
            return entity;
        }

        public async Task<bool> UpdateUsersRole(UserRoleEntity userRoleEntity)
        {
            var trans = Connection.BeginTransaction();
            bool flag;
            try
            {
                var delsql = "delete from tb_usersrole t where t.usersid=:usersId";
                await Connection.ExecuteAsync(delsql, new { usersId = userRoleEntity.Usersid });
                await InsertAsync(userRoleEntity);
                trans.Commit();
                flag = true;
            }
            catch (System.Exception)
            {
                trans.Rollback();
                throw;
            }
            return flag;
        }
    }
}
