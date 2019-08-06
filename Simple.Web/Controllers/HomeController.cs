using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.IApplication.SM;
using Simple.Web.Models;

namespace Simple.Web.Controllers
{
    public class HomeController : SimpleBaseController
    {
        private readonly IUserService userService;
        private readonly IMenusService menusService;
        public HomeController(IUserService userService, IMenusService menusService)
        {
            this.userService = userService;
            this.menusService = menusService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult About()
        {
            //userService.Add();
            //return SignOut("Cookies", "oidc");
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignOut()
        {
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
