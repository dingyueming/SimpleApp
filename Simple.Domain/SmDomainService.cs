using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.Entity;
using Simple.ExEntity;
using Simple.IDomain;
using Simple.IRepository;
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
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;
        public SmDomainService(IMapper mapper, IConfiguration configuration, IUserRepository userRepository, IMenusRepository menusRepository
            , IAuthRepository authRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.menusRepository = menusRepository;
            this.configuration = configuration;
            this.authRepository = authRepository;
        }

        #endregion

        #region 用户管理

        public async Task<List<UsersExEntity>> GetAllUsers()
        {
            var usersEntities = await userRepository.GetAllAsync();
            var userExEntities = mapper.Map<List<UsersExEntity>>(usersEntities);
            return userExEntities;
        }

        public async Task<UsersExEntity> GetUserById(int userId)
        {
            var userEntities = await userRepository.FindByIDAsync(userId);
            var userExEntities = mapper.Map<UsersExEntity>(userEntities);
            return userExEntities;

        }

        #endregion



        #region 菜单管理

        public async Task<List<MenusExEntity>> GetAllMenus()
        {
            var list = await menusRepository.GetAllAsync();
            var exList = mapper.Map<List<MenusExEntity>>(list).ToList();
            exList.ForEach(x =>
              {
                  x.ChildMenus = exList.Where(o => o.ParentId == x.MenusId).ToList().OrderBy(o => o.OrderIndex).ToList();
                  var localUrl = configuration["localUrl"];
                  x.MenusUrl = localUrl + x.MenusUrl;
              });
            return exList.Where(x => x.ParentId == 0).ToList();
        }


        #endregion
    }
}
