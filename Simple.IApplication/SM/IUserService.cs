using Simple.ExEntity;
using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
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

        Task<Pagination<UsersExEntity>> GetUserPage(Pagination<UsersExEntity> param);

        Task<bool> AddUser(UsersExEntity exEntity);
        Task<bool> DeleteUser(List<UsersExEntity> exEntities);
        Task<bool> UpdateUser(UsersExEntity exEntity);
    }
}
