using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.SM;

namespace Simple.Web.Controllers
{
    public class _LayoutController : SimpleBaseController
    {
        private readonly IMenusService menusService;
        public _LayoutController(IMenusService menusService)
        {
            this.menusService = menusService;
        }
        /// <summary>
        /// 查询所有在用的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryMenus()
        {
            var menus = await menusService.GetAllMenus();
            return Json(menus);
        }
    }
}