using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.ExEntity;
using Simple.IDomain;
using Simple.IRepository.SM;
using AutoMapper;


namespace Simple.Domain
{
    public class SmDomainService : ISmDomainService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper mapper;
        public SmDomainService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
        }
        public void AddUser()
        {
            _userRepository.Add(new UsersEntity());
        }

        public async Task<List<UsersExEntity>> GetAllUsers()
        {
            var usersEntities = await _userRepository.GetAll();
            var userExEntities = mapper.Map<List<UsersExEntity>>(usersEntities);
            return userExEntities;
        }
    }
}
