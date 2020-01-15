using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.IApplication.SM
{
    public interface IRolesService
    {
        Task<Pagination<RolesExEntity>> GetRolePage(Pagination<RolesExEntity> param);
        Task<bool> AddRole(RolesExEntity exEntity);
        Task<bool> DeleteRole(List<RolesExEntity> exEntities);
        Task<bool> UpdateRole(RolesExEntity exEntity);
        Task<bool> UpdateRolesMenu(List<RoleMenuExEntity> roleMenuExEntities);
        Task<List<RolesExEntity>> GetAllRoles();
    }
}
