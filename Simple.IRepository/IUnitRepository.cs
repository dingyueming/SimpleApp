using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface IUnitRepository : IBaseRepository<UnitEntity>
    {
        Task<Pagination<UnitEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby);
        Task<List<UnitEntity>> GetAllByUnitId(int unitId);
    }
}
