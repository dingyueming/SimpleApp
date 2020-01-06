using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.ExEntity;
using Simple.IApplication.SM;
using Simple.IDomain;

namespace Simple.Application.SM
{
    public class UserService : IUserService
    {
        private readonly ISmDomainService _smDomainService;
        public UserService(ISmDomainService smDomainService)
        {
            _smDomainService = smDomainService;
        }

        public async Task<List<UsersExEntity>> GetAllUsers()
        {
            return await _smDomainService.GetAllUsers();
        }

        public async Task<UsersExEntity> GetUserById(int userId)
        {
            return await _smDomainService.GetUserById(userId);
        }
    }
}
