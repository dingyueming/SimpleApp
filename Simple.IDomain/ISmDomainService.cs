using Simple.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.ExEntity;
using Simple.ExEntity.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;

namespace Simple.IDomain
{
    public interface ISmDomainService
    {

        #region 用户管理

        Task<List<UsersExEntity>> GetAllUsers();
        Task<UsersExEntity> GetUserById(int userId);
        Task<Pagination<UsersExEntity>> GetUserPage(Pagination<UsersExEntity> param);
        Task<bool> AddUser(UsersExEntity exEntity);
        Task<bool> DeleteUser(List<UsersExEntity> exEntities);
        Task<bool> UpdateUser(UsersExEntity exEntity);

        Task<bool> UpdateUsersRole(UserRoleExEntity userRoleExEntity);

        #endregion

        #region 菜单管理

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        Task<List<MenusExEntity>> GetAllMenus();
        Task<Pagination<MenusExEntity>> GetMenuPage(Pagination<MenusExEntity> param);
        Task<bool> AddMenu(MenusExEntity exEntity);
        Task<bool> DeleteMenu(List<MenusExEntity> exEntities);
        Task<bool> UpdateMenu(MenusExEntity exEntity);
        Task<List<MenusExEntity>> GetMenusByRole(decimal rolesId);
        Task<List<MenusExEntity>> GetMenusByUser(int usersId);
        #endregion

        #region 角色管理

        Task<Pagination<RolesExEntity>> GetRolePage(Pagination<RolesExEntity> param);
        Task<bool> AddRole(RolesExEntity exEntity);
        Task<bool> DeleteRole(List<RolesExEntity> exEntities);
        Task<bool> UpdateRole(RolesExEntity exEntity);
        Task<bool> UpdateRolesMenu(List<RoleMenuExEntity> roleMenuExEntities);
        Task<List<RolesExEntity>> GetAllRoles();
        #endregion
    }
}
