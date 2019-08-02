using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.ExEntity;
using Simple.IDomain;
using Simple.IRepository;
using Simple.IRepository.SM;

namespace Simple.Domain
{
    public class SmDomainService : ISmDomainService
    {
        private readonly IUserRepository _userRepository;
        public SmDomainService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser()
        {
            _userRepository.Add(new UsersEntity());
        }

        public async Task<List<UsersExEntity>> GetAllUsers()
        {
            var usersEntities = await _userRepository.GetAll();
            return new List<UsersExEntity>();
        }
    }
}
