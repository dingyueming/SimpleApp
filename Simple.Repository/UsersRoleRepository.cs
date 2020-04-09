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
            try
            {
                var delsql = "delete from tb_usersrole t where t.usersid=:usersId";
                await Connection.ExecuteAsync(delsql, new { usersId = userRoleEntity.Usersid }, trans);
                await Connection.ExecuteAsync(@"insert into tb_usersrole
                                              (usersroleid, usersid, rolesid, creator, createtime, remark)
                                            values
                                              (:usersroleid, :usersid, :rolesid, :creator, :createtime, :remark)", userRoleEntity, trans);
                trans.Commit();
            }
            catch (System.Exception)
            {
                trans.Rollback();
                throw;
            }
            return true;
        }

        public async Task DeleteUsersRoleByUserId(decimal usersId)
        {
            var delsql = "delete from tb_usersrole t where t.usersid=:usersId";
            await Connection.ExecuteAsync(delsql, new { usersId });
        }
    }
}
