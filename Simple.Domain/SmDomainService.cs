﻿using System;
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
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.Domain
{
    /// <summary>
    /// SM模块领域服务
    /// </summary>
    public class SmDomainService : ISmDomainService
    {
        #region 构造函数

        private readonly IRoleMenuRepository roleMenuRepository;
        private readonly IUserRepository userRepository;
        private readonly IMenusRepository menusRepository;
        private readonly IRolesRepository rolesRepository;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public SmDomainService(IRoleMenuRepository roleMenuRepository, IRolesRepository rolesRepository, IMapper mapper, IConfiguration configuration, IUserRepository userRepository, IMenusRepository menusRepository)
        {
            this.roleMenuRepository = roleMenuRepository;
            this.rolesRepository = rolesRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.menusRepository = menusRepository;
            this.configuration = configuration;
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

        public async Task<Pagination<UsersExEntity>> GetUserPage(Pagination<UsersExEntity> param)
        {
            var pagination = await userRepository.GetUserPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<UsersExEntity>>(pagination);
        }

        public async Task<bool> AddUser(UsersExEntity exEntity)
        {
            var entity = mapper.Map<UsersEntity>(exEntity);
            return await userRepository.InsertAsync(entity);
        }

        public async Task<bool> UpdateUser(UsersExEntity exEntity)
        {
            var entity = mapper.Map<UsersEntity>(exEntity);
            return await userRepository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteUser(List<UsersExEntity> exEntities)
        {
            var entities = mapper.Map<List<UsersEntity>>(exEntities);
            return await userRepository.DeleteAsync(entities);
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

        public async Task<Pagination<MenusExEntity>> GetMenuPage(Pagination<MenusExEntity> param)
        {
            var pagination = await menusRepository.GetMenuPage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<MenusExEntity>>(pagination);
        }

        public async Task<bool> AddMenu(MenusExEntity exEntity)
        {
            var entity = mapper.Map<MenusEntity>(exEntity);
            return await menusRepository.InsertAsync(entity);
        }

        public async Task<bool> DeleteMenu(List<MenusExEntity> exEntities)
        {
            var entities = mapper.Map<List<MenusEntity>>(exEntities);
            return await menusRepository.DeleteAsync(entities);
        }

        public async Task<bool> UpdateMenu(MenusExEntity exEntity)
        {
            var entity = mapper.Map<MenusEntity>(exEntity);
            return await menusRepository.UpdateAsync(entity);
        }

        public async Task<List<MenusExEntity>> GetMenusByRole(decimal rolesId)
        {
            var entities = await roleMenuRepository.GetRoleMenuEntitiesByRole(rolesId);
            var listMenus = entities.Where(o => o.Menu != null).Select(x => x.Menu).ToList();
            return mapper.Map<List<MenusExEntity>>(listMenus);
        }

        #endregion

        #region 角色管理

        public async Task<Pagination<RolesExEntity>> GetRolePage(Pagination<RolesExEntity> param)
        {
            var pagination = await rolesRepository.GetRolePage(param.PageSize, param.PageIndex, param.Where, param.OrderBy);
            return mapper.Map<Pagination<RolesExEntity>>(pagination);
        }

        public async Task<bool> AddRole(RolesExEntity exEntity)
        {
            var entity = mapper.Map<RolesEntity>(exEntity);
            return await rolesRepository.InsertAsync(entity);
        }

        public async Task<bool> DeleteRole(List<RolesExEntity> exEntities)
        {
            var entities = mapper.Map<List<RolesEntity>>(exEntities);
            return await rolesRepository.DeleteAsync(entities);
        }

        public async Task<bool> UpdateRole(RolesExEntity exEntity)
        {
            var entity = mapper.Map<RolesEntity>(exEntity);
            return await rolesRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateRolesMenu(List<RoleMenuExEntity> roleMenuExEntities)
        {
            var entities = mapper.Map<List<RoleMenuEntity>>(roleMenuExEntities);
            return await roleMenuRepository.UpdateRolesMenu(entities);
        }

        #endregion
    }
}
