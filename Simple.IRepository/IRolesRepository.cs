using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IRepository
{
    public interface IRolesRepository : IBaseRepository<RolesEntity>
    {
        Task<Pagination<RolesEntity>> GetRolePage(int pageSize, int pageIndex, string where, string orderby);
    }
}
