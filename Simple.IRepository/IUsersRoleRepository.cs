﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;

namespace Simple.IRepository
{
    public interface IUsersRoleRepository : IBaseRepository<UserRoleEntity>
    {
        Task<UserRoleEntity> GetRoleEntityByUser(int usersId);
        Task<bool> UpdateUsersRole(UserRoleEntity userRoleEntity);
        Task DeleteUsersRoleByUserId(decimal userId);
    }
}
