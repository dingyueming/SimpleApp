using Simple.Entity;
using Simple.Entity.GM;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.IRepository.GM
{
    public interface IUnitRepository : IBaseRepository<UnitEntity>
    {
    }
}
