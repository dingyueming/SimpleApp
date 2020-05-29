using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using Simple.ExEntity;
using Simple.IApplication.DM;
using Simple.IApplication.SM;
using Simple.Web.Extension.ServiceExpend;
using Microsoft.Extensions.Caching.Memory;
using Simple.ExEntity.SM;
using Simple.Web.Extension.ControllerEx;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Simple.Web.Controllers
{
    [Authorize]
    public class SimpleBaseController : Controller
    {
        #region 构造函数
        private readonly IMenusService menusService;
        private readonly IUnitService unitService;
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        public SimpleBaseController()
        {
            var serviceProvider = ServiceLocator.Services;
            memoryCache = (IMemoryCache)serviceProvider.GetService(typeof(IMemoryCache));
            menusService = (IMenusService)serviceProvider.GetService(typeof(IMenusService));
            unitService = (IUnitService)serviceProvider.GetService(typeof(IUnitService));
            configuration = (IConfiguration)serviceProvider.GetService(typeof(IConfiguration));
        }
        #endregion

        #region 页面BaseIndex
        public virtual IActionResult Index()
        {
            ViewBag.UserName = LoginUser.UsersName;
            var strMapJsUrl = configuration.GetSection("MapConfig")["MapJsUrl"];
            if (!string.IsNullOrEmpty(strMapJsUrl))
            {
                ViewBag.MapJs = strMapJsUrl.Split('|').ToList();
            }
            var strMapCssUrl = configuration.GetSection("MapConfig")["MapCssUrl"];
            if (!string.IsNullOrEmpty(strMapCssUrl))
            {
                ViewBag.MapCss = strMapCssUrl.Split('|').ToList();
            }

            //根据当前页面url设置页面的title
            var path = HttpContext.Request.Path;
            if (path.HasValue)
            {
                foreach (var menu in UsersMenus)
                {
                    var secondMenu = menu.ChildMenus.FirstOrDefault(x => x.MenusUrl.IndexOf(path.Value) > -1);
                    if (secondMenu != null)
                    {
                        ViewBag.Title = secondMenu.MenusName;
                    }
                }

            }
            return View();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 登录用户
        /// </summary>
        public UsersExEntity LoginUser
        {
            get
            {
                var user = new UsersExEntity();
                var authResult = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
                if (authResult.Succeeded)
                {
                    //user.Email = authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                    user.UsersId = int.Parse(authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    user.UsersName = authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                    user.RealName = authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value;
                }
                return user;
            }
        }

        /// <summary>
        /// 用户拥有的菜单
        /// </summary>
        public List<MenusExEntity> UsersMenus
        {
            get
            {
                var flag = memoryCache.TryGetValue("cacheMenu", out var menus);
                if (!flag)
                {
                    menus = menusService.GetMenusByUser(LoginUser.UsersId).Result;
                    memoryCache.Set("cacheMenu", menus, DateTime.Now.AddMinutes(30) - DateTime.Now);
                }
                return menus as List<MenusExEntity>;
            }
        }


        #endregion

        #region 方法

        /// <summary>
        /// 查询用户所拥有的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryMenusByUser()
        {
            var flag = memoryCache.TryGetValue("cacheMenu", out var menus);
            if (!flag)
            {
                menus = await menusService.GetMenusByUser(LoginUser.UsersId);
                memoryCache.Set("cacheMenu", menus, DateTime.Now.AddMinutes(30) - DateTime.Now);
            }
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
        /// <summary>
        /// 查询单位树
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryUnitTree()
        {
            var trees = await unitService.GetUnitTree();
            return Json(trees);
        }

        /// <summary>
        /// 查询单位
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> Query1And2Unit()
        {
            var pagination = await unitService.GetPage(new Infrastructure.InfrastructureModel.Paionation.Pagination<ExEntity.DM.UnitExEntity>() { PageSize = 10000 });
            var data = pagination.Data;
            var list = new List<ExEntity.DM.UnitExEntity>();
            if (data != null)
            {
                //list.AddRange(data.Where(x => x.ORG_CODE == "520100000000" || x.P_ORG_CODE == "520100000000"));
            }
            return LowerJson(list.OrderBy(x => x.UNITID));
        }

        /// <summary>
        /// 查询所有单位
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> QueryAllUnit()
        {
            var list = await unitService.GetAllUnitExEntities();
            return LowerJson(list);
        }

        #region 记录操作日志

        internal async Task RecordLog(string modelName, object operateModel, Infrastructure.Enums.OperateTypeEnum operateType)
        {
            try
            {
                var serviceProvider = ServiceLocator.Services;
                var logService = serviceProvider.GetService<IOperateLogService>();
                await logService.AddLog(new OperateLogExEntity()
                {
                    Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                    Loginname = LoginUser.UsersName,
                    Realname = LoginUser.RealName,
                    Modelname = modelName ?? "",
                    Operatetype = (int)operateType,
                    Remark = operateModel == null ? "" : JsonConvert.SerializeObject(operateModel),
                });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 重载Json方法

        /// <summary>
        /// 格式化返回JSON
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回扩展实体一模一样的大小写</returns>
        internal JsonResult FormerJson(object data)
        {
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new DefaultContractResolver() });
        }

        /// <summary>
        /// 格式化返回JSON
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回扩展实体一模一样的大小写</returns>
        internal JsonResult LowerJson(object data)
        {
            return Json(data, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new LowerContractResolver() });
        }

        #endregion

        #endregion
    }
}