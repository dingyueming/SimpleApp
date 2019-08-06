using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.ExEntity.SM;

namespace Simple.IApplication.SM
{
    public interface IMenusService
    {
        Task<List<MenusExEntity>> GetAllMenus();
    }
}
