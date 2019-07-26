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
using Simple.ExEntity;

namespace Simple.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl, bool rememberMe)
        {
            if (!userName.IsNullOrEmpty())
            {
                var user = new UserExEntity()
                {
                    UserName = "zhangsan",
                    UserPwd = "123456"
                };
                //证件单元
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
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

                if (returnUrl.IsNullOrEmpty())
                {
                    return RedirectToAction("Index", "Home");
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
            return Redirect(returnUrl ?? Url.Action("Login"));
        }
    }
}