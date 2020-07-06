using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class RoleManageController : SimpleBaseController
    {
        private readonly IRolesService rolesService;
        private readonly IMenusService menusService;
        private readonly IMemoryCache memoryCache;
        public RoleManageController(IMemoryCache memoryCache, IMenusService menusService, IRolesService rolesService)
        {
            this.memoryCache = memoryCache;
            this.rolesService = rolesService;
            this.menusService = menusService;
        }
        public async Task<JsonResult> QueryRoles(Pagination<RolesExEntity> pagination)
        {
            var data = await rolesService.GetRolePage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(RolesExEntity exEntity)
        {
            await RecordLog("角色", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.Modifytime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.Createtime = DateTime.Now;
            return await rolesService.AddRole(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(RolesExEntity exEntity)
        {
            await RecordLog("角色", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.Modifytime = DateTime.Now;
            return await rolesService.UpdateRole(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(RolesExEntity exEntity)
        {
            await RecordLog("角色", exEntity, Infrastructure.Enums.OperateTypeEnum.删除);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.Modifytime = DateTime.Now;
            return await rolesService.DeleteRole(new List<RolesExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<RolesExEntity> exEntities)
        {
            await RecordLog("角色", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.Modifytime = DateTime.Now; });
            return await rolesService.DeleteRole(exEntities);
        }

        public async Task<JsonResult> QueryRolesMenu(decimal rolesId)
        {
            var menus = await menusService.GetMenusByRole(rolesId);
            return Json(menus);
        }
        [SimpleAction]
        public async Task<bool> SaveRolesMenu(List<MenusExEntity> menus, decimal rolesId)
        {
            await RecordLog("角色拥有的菜单", menus, Infrastructure.Enums.OperateTypeEnum.修改);
            var list = new List<RoleMenuExEntity>();
            menus.ForEach(x =>
            {
                list.Add(new RoleMenuExEntity()
                {
                    Creator = LoginUser.UsersId,
                    Createtime = DateTime.Now,
                    Menusid = x.MenusId,
                    Rolesid = rolesId
                });
            });
            var flag = memoryCache.TryGetValue("cacheMenu" + LoginUser.UsersId, out var cacheMenus);
            if (flag)
            {
                memoryCache.Remove("cacheMenu" + LoginUser.UsersId);
            }
            return await rolesService.UpdateRolesMenu(list);
        }
    }
}