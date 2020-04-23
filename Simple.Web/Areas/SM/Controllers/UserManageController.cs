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
using Simple.Web.Extension.ControllerEx;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class UserManageController : SimpleBaseController
    {
        private readonly IUserService userService;
        private readonly IRolesService rolesService;
        private readonly IUnitService unitService;
        private readonly IDmDomainService dmDomainService;
        public UserManageController(IDmDomainService dmDomainService, IUnitService unitService, IRolesService rolesService, IUserService userService)
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
        [SimpleAction]
        public async Task<bool> Add(UsersExEntity exEntity)
        {
            await RecordLog("用户", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.CreateTime = DateTime.Now;
            return await userService.AddUser(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(UsersExEntity exEntity)
        {
            await RecordLog("用户", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.UpdateUser(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(UsersExEntity exEntity)
        {
            await RecordLog("用户", exEntity, Infrastructure.Enums.OperateTypeEnum.删除);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.DeleteUser(new List<UsersExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<UsersExEntity> exEntities)
        {
            await RecordLog("用户", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.ModifyTime = DateTime.Now; });
            return await userService.DeleteUser(exEntities);
        }
        public async Task<JsonResult> QueryAllRoles()
        {
            var list = await rolesService.GetAllRoles();
            return Json(list);
        }
        [SimpleAction]
        public async Task<bool> SaveUsersRole(UserRoleExEntity userRoleExEntity)
        {
            await RecordLog("用户的角色", userRoleExEntity, Infrastructure.Enums.OperateTypeEnum.修改);
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
        [SimpleAction]
        public async Task<bool> SaveUsersDevice(List<ElementTreeModel> nodes, int userId)
        {
            await RecordLog("设备分配", nodes, Infrastructure.Enums.OperateTypeEnum.修改);
            return await dmDomainService.UpdateAuthLimits(nodes, userId);
        }
    }
}