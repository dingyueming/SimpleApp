﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.Infrastructure.Dapper.Contrib;
using Dapper;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IRepository
{
    public interface IUserRepository : IBaseRepository<UsersEntity>
    {
        Task<Pagination<UsersEntity>> GetUserPage(int pageSize, int pageIndex, string orderby, string where);

        Task<List<UsersEntity>> GetUsersEntityByUserName(string userName);
    }
}
