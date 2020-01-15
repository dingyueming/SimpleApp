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
    public class MenusService : IMenusService
    {
        private readonly ISmDomainService smDomainService;
        public MenusService(ISmDomainService smDomainService)
        {
            this.smDomainService = smDomainService;
        }

        public async Task<bool> AddMenu(MenusExEntity exEntity)
        {
            return await smDomainService.AddMenu(exEntity);
        }

        public async Task<bool> DeleteMenu(List<MenusExEntity> exEntities)
        {
            return await smDomainService.DeleteMenu(exEntities);
        }

        public async Task<bool> UpdateMenu(MenusExEntity exEntity)
        {
            return await smDomainService.UpdateMenu(exEntity);
        }

        public async Task<List<MenusExEntity>> GetAllMenus()
        {
            return await smDomainService.GetAllMenus();
        }

        public async Task<Pagination<MenusExEntity>> GetMenuPage(Pagination<MenusExEntity> param)
        {
            return await smDomainService.GetMenuPage(param);
        }

        public async Task<List<MenusExEntity>> GetMenusByRole(decimal rolesId)
        {
            return await smDomainService.GetMenusByRole(rolesId);
        }
    }
}
