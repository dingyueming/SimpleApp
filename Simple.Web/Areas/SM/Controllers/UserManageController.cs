using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.IApplication.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.Element;
using Simple.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.IDomain;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class UserManageController : SimpleBaseController
    {
        private readonly IUserService userService;
        private readonly IRolesService rolesService;
        private readonly IUnitService unitService;
        private readonly IDmDomainService dmDomainService;
        public UserManageController(IDmDomainService dmDomainService, IUnitService unitService, IRolesService rolesService, IUserService userService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.dmDomainService = dmDomainService;
            this.unitService = unitService;
            this.rolesService = rolesService;
            this.userService = userService;
        }
        public async Task<JsonResult> QueryUsers(Pagination<UsersExEntity> pagination)
        {
            var data = await userService.GetUserPage(pagination);
            return Json(data);
        }
        public async Task<bool> Add(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.CreateTime = DateTime.Now;
            return await userService.AddUser(exEntity);
        }
        public async Task<bool> Update(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.UpdateUser(exEntity);
        }
        public async Task<bool> Delete(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.DeleteUser(new List<UsersExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<UsersExEntity> exEntities)
        {
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.ModifyTime = DateTime.Now; });
            return await userService.DeleteUser(exEntities);
        }
        public async Task<JsonResult> QueryAllRoles()
        {
            var list = await rolesService.GetAllRoles();
            return Json(list);
        }
        public async Task<bool> SaveUsersRole(UserRoleExEntity userRoleExEntity)
        {
            userRoleExEntity.Createtime = DateTime.Now;
            userRoleExEntity.Creator = LoginUser.UsersId;
            return await userService.UpdateUsersRole(userRoleExEntity);
        }
        public async Task<JsonResult> QueryDevices()
        {
            var list = await unitService.GetUnitAndDeviceTree();
            return Json(list);
        }
        public async Task<JsonResult> QueryUsersDevice(int userId)
        {
            var list = await dmDomainService.GetDeviceIdsByUser(userId);
            return Json(list);
        }
        public async Task<bool> SaveUsersDevice(List<ElementTreeModel> nodes, int userId)
        {
            return await dmDomainService.UpdateAuthLimits(nodes, userId);
        }
    }
}