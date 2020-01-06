using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Repository
{
   public class UnitRepository : BaseRepository<UnitEntity>, IUnitRepository
    {
    }
}
