using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.IRepository
{
    public interface IUnitRepository : IBaseRepository<UnitEntity>
    {
    }
}
