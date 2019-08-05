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
    /// <summary>
    /// SM模块领域服务
    /// </summary>
    public class SmDomainService : ISmDomainService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public SmDomainService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public void AddUser()
        {
            userRepository.Add(new UsersEntity());
        }

        public async Task<List<UsersExEntity>> GetAllUsers()
        {
            var usersEntities = await userRepository.GetAll();
            var userExEntities = mapper.Map<List<UsersExEntity>>(usersEntities);
            return userExEntities;
        }
    }
}
