using Simple.ExEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Simple.IApplication.SM
{
    public interface IUserService
    {
        Task<UsersExEntity> GetUserById(int userId);

        Task<List<UsersExEntity>> GetAllUsers();
    }
}
