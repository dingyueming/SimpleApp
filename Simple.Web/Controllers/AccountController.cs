using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Simple.ExEntity;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.Web.Extension.ServiceExpend;

namespace Simple.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : SimpleBaseController
    {
        private IUserService userService;
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl, bool rememberMe)
        {
            if (!userName.IsNullOrEmpty() && !password.IsNullOrEmpty())
            {
                var users = await userService.GetAllUsers();
                var loginUser = users.FirstOrDefault(x => x.UsersName == userName && x.Password == password);
                if (loginUser == null)
                {
                    return View();
                }
                //证件单元
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,loginUser.UsersName),
                    new Claim(ClaimTypes.NameIdentifier,loginUser.UsersId.ToString()),
                    new Claim(ClaimTypes.GivenName,loginUser.RealName)
                };

                //使用证件单元创建一张cookie身份证
                var cookie = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //创建一个人携带cookie身份证
                var identity = new ClaimsPrincipal(cookie);

                AuthenticationProperties props = null;
                if (rememberMe)
                {
                    props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
                    };
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), props);

                #region 记录日志
                var serviceProvider = ServiceLocator.Services;
                var logService = serviceProvider.GetService<IOperateLogService>();
                await logService.AddLog(new OperateLogExEntity()
                {
                    Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                    Loginname = loginUser.UsersName,
                    Realname = loginUser.RealName,
                    Modelname = "",
                    Operatetype = (int)Infrastructure.Enums.OperateTypeEnum.登陆,
                    Remark = "",
                });
                #endregion

                if (returnUrl.IsNullOrEmpty())
                {
                    return RedirectToAction("Index", "RealtimeMap", new { Area = "EzMap" });
                }
                return Redirect(returnUrl);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <param name="returnUrl">回转页</param>
        /// <returns></returns>
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await RecordLog(null, null, Infrastructure.Enums.OperateTypeEnum.退出);
            return Redirect(returnUrl ?? Url.Action("Login"));
        }
    }
}