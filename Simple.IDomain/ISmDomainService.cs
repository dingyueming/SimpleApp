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
        #endregion

        #region 菜单管理

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        Task<List<MenusExEntity>> GetAllMenus();

        #endregion
    }
}
