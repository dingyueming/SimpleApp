using Simple.ExEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SM
{
    public interface IUserService
    {
        void Add();

        Task<List<UsersExEntity>> GetAllUsers();
    }
}
