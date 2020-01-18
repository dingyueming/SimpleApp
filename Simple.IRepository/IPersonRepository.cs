using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface IPersonRepository : IBaseRepository<PersonEntity>
    {
        Task<List<PersonEntity>> GetPersonEntitiesByUser(int userId);
        Task<Pagination<PersonEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby);
    }
}
