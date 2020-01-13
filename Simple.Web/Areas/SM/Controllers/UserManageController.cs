using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity;
using Simple.IApplication.SM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class UserManageController : SimpleBaseController
    {
        private readonly IUserService userService;
        public UserManageController(IUserService userService, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.userService = userService;
        }
        public async Task<JsonResult> QueryUsers(Pagination<UsersExEntity> pagination)
        {
            var data = await userService.GetUserPage(pagination);
            return Json(data);
        }

        public async Task<bool> Add(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.CreateTime = DateTime.Now;
            return await userService.AddUser(exEntity);
        }
        public async Task<bool> Update(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.UpdateUser(exEntity);
        }
        public async Task<bool> Delete(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.DeleteUser(new List<UsersExEntity>() { exEntity });
        }
        public async Task<bool> BatchDelete(List<UsersExEntity> exEntities)
        {
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.ModifyTime = DateTime.Now; });
            return await userService.DeleteUser(exEntities);
        }

    }
}