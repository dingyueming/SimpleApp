using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.ExEntity;
using Simple.IDomain;
using Simple.IRepository.SM;
using AutoMapper;
using Simple.ExEntity.SM;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Simple.Domain
{
    /// <summary>
    /// SM模块领域服务
    /// </summary>
    public class SmDomainService : ISmDomainService
    {
        #region 构造函数

        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IMenusRepository menusRepository;
        private readonly IConfiguration configuration;
        public SmDomainService(IMapper mapper, IConfiguration configuration, IUserRepository userRepository, IMenusRepository menusRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.menusRepository = menusRepository;
            this.configuration = configuration;
        }

        #endregion

        #region 用户管理

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

        #endregion

        #region 菜单管理

        public async Task<List<MenusExEntity>> GetAllMenus()
        {
            var list = await menusRepository.GetAll();
            var exList = mapper.Map<List<MenusExEntity>>(list);
            exList.ForEach(x =>
            {
                x.ChildMenus = exList.Where(o => o.ParentId == x.MenusId).ToList();
                var localUrl = configuration["localUrl"];
                x.MenusUrl = localUrl + x.MenusUrl;
            });
            return exList.Where(x => x.ParentId == 0).ToList();
        }

        #endregion
    }
}
