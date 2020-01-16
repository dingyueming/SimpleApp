using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Simple.ExEntity;
using Simple.IApplication.SM;

namespace Simple.Web.Controllers
{
    [Authorize]
    public class SimpleBaseController : Controller
    {
        private readonly IMenusService menusService;
        public SimpleBaseController(IServiceProvider serviceProvider)
        {
            this.menusService = (IMenusService)serviceProvider.GetService(typeof(IMenusService));
        }

        public virtual IActionResult Index()
        {
            ViewBag.UserName = LoginUser.UsersName;
            return View();
        }

        public UsersExEntity LoginUser
        {
            get
            {
                var user = new UsersExEntity();
                var authResult = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
                if (authResult.Succeeded)
                {
                    user.Email = authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                    user.UsersId = int.Parse(authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    user.UsersName = authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                }
                return user;
            }
        }
        /// <summary>
        /// 查询用户所拥有的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryMenusByUser()
        {
            var menus = await menusService.GetMenusByUser(LoginUser.UsersId);
            return Json(menus);
        }

        /// <summary>
        /// 查询所有在用的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryAllMenus()
        {
            var menus = await menusService.GetAllMenus();
            return Json(menus);
        }

        public async Task GetAuth()
        {
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                var aa = auth.Principal.Identity;
            }
        }

        #region 重载Json方法

        internal JsonResult FirstJson(object data)
        {
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        #endregion
    }
}