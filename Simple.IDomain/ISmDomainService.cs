﻿using Simple.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Simple.ExEntity;
using Simple.ExEntity.SM;

namespace Simple.IDomain
{
    public interface ISmDomainService
    {

        Task<List<UsersExEntity>> GetAllUsers();

        #region 用户管理

        Task<List<AuthExEntity>> GetAllAuthUsers();

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
