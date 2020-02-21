using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface ICarAreaRepository : IBaseRepository<CarAreaEntity>
    {
        Task InsertCarArea(CarAreaEntity entity);
    }
}
