using Simple.Entity.GM;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository.GM
{
    public interface ICarRepository : IBaseRepository<CarEntity>
    {
        Task<List<CarEntity>> GetCarEntitiesByUser(int userId);
    }
}
