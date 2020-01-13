using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IApplication.SM
{
    public interface IMenusService
    {
        Task<List<MenusExEntity>> GetAllMenus();
        Task<Pagination<MenusExEntity>> GetMenuPage(Pagination<MenusExEntity> param);
        Task<bool> AddMenu(MenusExEntity exEntity);
        Task<bool> DeleteMenu(List<MenusExEntity> exEntities);
        Task<bool> UpdateMenu(MenusExEntity exEntity);
    }
}
