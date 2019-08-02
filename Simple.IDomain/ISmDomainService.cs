﻿using Simple.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.ExEntity;

namespace Simple.IDomain
{
    public interface ISmDomainService
    {
        void AddUser();

        Task<List<UsersExEntity>> GetAllUsers();
    }
}
