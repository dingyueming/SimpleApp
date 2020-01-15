using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface IRoleMenuRepository : IBaseRepository<RoleMenuEntity>
    {
        Task<List<RoleMenuEntity>> GetRoleMenuEntitiesByRole(decimal rolesId);

        Task<bool> UpdateRolesMenu(List<RoleMenuEntity> roleMenuExEntities);
        
    }
}
