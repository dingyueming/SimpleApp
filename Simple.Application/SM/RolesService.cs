using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Application.SM
{
    public class RolesService : IRolesService
    {
        private readonly ISmDomainService smDomainService;
        public RolesService(ISmDomainService smDomainService)
        {
            this.smDomainService = smDomainService;
        }

        public async Task<bool> AddRole(RolesExEntity exEntity)
        {
            return await smDomainService.AddRole(exEntity);
        }

        public async Task<bool> DeleteRole(List<RolesExEntity> exEntities)
        {
            return await smDomainService.DeleteRole(exEntities);
        }

        public async Task<bool> UpdateRole(RolesExEntity exEntity)
        {
            return await smDomainService.UpdateRole(exEntity);
        }

        public async Task<Pagination<RolesExEntity>> GetRolePage(Pagination<RolesExEntity> param)
        {
            return await smDomainService.GetRolePage(param);
        }

        public async Task<bool> UpdateRolesMenu(List<RoleMenuExEntity> roleMenuExEntities)
        {
            return await smDomainService.UpdateRolesMenu(roleMenuExEntities);
        }

    }
}
