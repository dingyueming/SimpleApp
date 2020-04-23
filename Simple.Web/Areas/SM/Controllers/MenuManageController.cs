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
    public class MenuManageController : SimpleBaseController
    {
        private readonly IMenusService menusService;
        public MenuManageController(IMenusService menusService)
        {
            this.menusService = menusService;
        }
        public async Task<JsonResult> QueryMenus(Pagination<MenusExEntity> pagination)
        {
            var data = await menusService.GetMenuPage(pagination);
            return Json(data);
        }

        public async Task<bool> Add(MenusExEntity exEntity)
        {
            await RecordLog("菜单", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.CreateTime = DateTime.Now;
            return await menusService.AddMenu(exEntity);
        }
        public async Task<bool> Update(MenusExEntity exEntity)
        {
            await RecordLog("菜单", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await menusService.UpdateMenu(exEntity);
        }
        public async Task<bool> Delete(MenusExEntity exEntity)
        {
            await RecordLog("菜单", exEntity, Infrastructure.Enums.OperateTypeEnum.删除);
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await menusService.DeleteMenu(new List<MenusExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<MenusExEntity> exEntities)
        {
            await RecordLog("菜单", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.ModifyTime = DateTime.Now; });
            return await menusService.DeleteMenu(exEntities);
        }

        public async Task<JsonResult> QueryFirstMenus()
        {
            var allMeuns = await menusService.GetAllMenus();
            return Json(allMeuns.Where(x => x.ParentId == 0).ToList());
        }
    }
}