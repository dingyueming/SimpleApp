using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Simple.Web.Controllers
{
    [Authorize]
    public class SimpleBaseController : Controller
    {

        public async Task GetAuth()
        {
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                var aa = auth.Principal.Identity;
            }
        }
    }
}