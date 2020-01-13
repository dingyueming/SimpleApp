using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IRepository
{
    public interface IMenusRepository : IBaseRepository<MenusEntity>
    {
        Task<Pagination<MenusEntity>> GetMenuPage(int pageSize, int pageIndex, string where, string orderby);
    }
}
