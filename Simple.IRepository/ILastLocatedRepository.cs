﻿using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IRepository
{
    public interface ILastLocatedRepository : IBaseRepository<LastLocatedEntity>
    {
        Task<List<LastLocatedEntity>> GetLastLocatedEntityByUser(int userId);

        Task<List<LastLocatedEntity>> GetLastLocatedByUser(int userId);

        Task<LastLocatedEntity> GetEntityByMac(string mac);

        Task<LastLocatedEntity> GetEntityByKeyword(string keyword);
    }
}
