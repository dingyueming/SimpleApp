using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.IDomain;
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
        public async Task<List<MenusExEntity>> GetAllMenus()
        {
            return await smDomainService.GetAllMenus();
        }
    }
}
