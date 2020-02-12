using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface ICarRepository : IBaseRepository<CarEntity>
    {
        Task<List<CarEntity>> GetCarEntitiesByUser(int userId);

        Task<Pagination<CarEntity>> GetPage(int pageSize, int pageIndex, string where, string orderby);

        Task<CarEntity> GetCarEntityForValdata(CarEntity car);
    }
}
