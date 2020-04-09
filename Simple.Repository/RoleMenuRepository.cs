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
    public class RoleMenuRepository : BaseRepository<RoleMenuEntity>, IRoleMenuRepository
    {
        public async Task<List<RoleMenuEntity>> GetRoleMenuEntitiesByRole(decimal rolesId)
        {
            var sql = "select a.*,b.* from tb_rolemenu a join tb_menus b on a.menusid=b.menusid where a.rolesid=:roleId";
            var entities = await Connection.QueryAsync<RoleMenuEntity, MenusEntity, RoleMenuEntity>(sql, (a, b) =>
                  {
                      a.Menu = b;
                      return a;
                  }, new { roleId = rolesId }, splitOn: "menusId");
            return entities.AsList();
        }
        public async Task<bool> UpdateRolesMenu(List<RoleMenuEntity> roleMenuExEntities)
        {
            var trans = Connection.BeginTransaction();
            var flag = false;
            try
            {
                var delSql = "delete from tb_rolemenu t where t.rolesid=:rolesId";
                await Connection.ExecuteAsync(delSql, new { rolesId = roleMenuExEntities.Select(x => x.Rolesid).FirstOrDefault() }, trans);
                var insSql = @"insert into tb_rolemenu
                              (rolemenuid, rolesid, menusid, creator, createtime, remark)
                            values
                              (:rolemenuid, :rolesid, :menusid, :creator, :createtime, :remark)";
                await Connection.ExecuteAsync(insSql, roleMenuExEntities, trans);
                trans.Commit();
                flag = true;
            }
            catch (System.Exception e)
            {
                trans.Rollback();
                throw e;
            }
            return flag;
        }
    }
}
