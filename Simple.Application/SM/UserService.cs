using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.ExEntity;
using Simple.IApplication.SM;
using Simple.IDomain;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.Application.SM
{
    public class UserService : IUserService
    {
        private readonly ISmDomainService smDomainService;
        public UserService(ISmDomainService smDomainService)
        {
            this.smDomainService = smDomainService;
        }

        public async Task<bool> AddUser(UsersExEntity exEntity)
        {
            return await smDomainService.AddUser(exEntity);
        }

        public async Task<bool> DeleteUser(UsersExEntity exEntity)
        {
            return await smDomainService.DeleteUser(exEntity);
        }

        public async Task<List<UsersExEntity>> GetAllUsers()
        {
            return await smDomainService.GetAllUsers();
        }

        public async Task<UsersExEntity> GetUserById(int userId)
        {
            return await smDomainService.GetUserById(userId);
        }

        public async Task<Pagination<UsersExEntity>> GetUserPage(Pagination<UsersExEntity> param)
        {
            var data = await smDomainService.GetUserPage(param);
            return data;
        }

        public async Task<bool> UpdateUser(UsersExEntity exEntity)
        {
            return await smDomainService.UpdateUser(exEntity);
        }
    }
}
