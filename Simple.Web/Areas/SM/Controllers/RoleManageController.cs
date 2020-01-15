using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class RoleManageController : SimpleBaseController
    {
        private readonly IRolesService rolesService;
        private readonly IMenusService menusService;
        public RoleManageController(IMenusService menusService, IRolesService rolesService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.rolesService = rolesService;
            this.menusService = menusService;
        }
        public async Task<JsonResult> QueryRoles(Pagination<RolesExEntity> pagination)
        {
            var data = await rolesService.GetRolePage(pagination);
            return Json(data);
        }

        public async Task<bool> Add(RolesExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.Modifytime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.Createtime = DateTime.Now;
            return await rolesService.AddRole(exEntity);
        }
        public async Task<bool> Update(RolesExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.Modifytime = DateTime.Now;
            return await rolesService.UpdateRole(exEntity);
        }
        public async Task<bool> Delete(RolesExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.Modifytime = DateTime.Now;
            return await rolesService.DeleteRole(new List<RolesExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<RolesExEntity> exEntities)
        {
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.Modifytime = DateTime.Now; });
            return await rolesService.DeleteRole(exEntities);
        }

        public async Task<JsonResult> QueryRolesMenu(decimal rolesId)
        {
            var menus = await menusService.GetMenusByRole(rolesId);
            return Json(menus);
        }

        public async Task<bool> SaveRolesMenu(List<MenusExEntity> menus, decimal rolesId)
        {
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
            return await rolesService.UpdateRolesMenu(list);
        }
    }
}