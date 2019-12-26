﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;

namespace Simple.IRepository.SM
{
    public interface IUserRepository : IBaseRepository<UsersEntity>
    {
    }
}
