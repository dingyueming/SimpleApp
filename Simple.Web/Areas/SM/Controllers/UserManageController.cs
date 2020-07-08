using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity;
using Simple.ExEntity.SM;
using Simple.IApplication.SM;
using Simple.IApplication.DM;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Infrastructure.InfrastructureModel.Element;
using Simple.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simple.IDomain;
using Simple.Web.Extension.ControllerEx;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Linq;

namespace Simple.Web.Areas.SM.Controllers
{
    [Area("SM")]
    public class UserManageController : SimpleBaseController
    {
        private readonly IUserService userService;
        private readonly IRolesService rolesService;
        private readonly IUnitService unitService;
        private readonly IDmDomainService dmDomainService;
        public UserManageController(IDmDomainService dmDomainService, IUnitService unitService, IRolesService rolesService, IUserService userService)
        {
            this.dmDomainService = dmDomainService;
            this.unitService = unitService;
            this.rolesService = rolesService;
            this.userService = userService;
        }
        public async Task<JsonResult> QueryUsers(Pagination<UsersExEntity> pagination)
        {
            var data = await userService.GetUserPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            exEntity.Creator = LoginUser.UsersId;
            exEntity.CreateTime = DateTime.Now;
            return await userService.AddUser(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.UpdateUser(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(UsersExEntity exEntity)
        {
            exEntity.Modifier = LoginUser.UsersId;
            exEntity.ModifyTime = DateTime.Now;
            return await userService.DeleteUser(new List<UsersExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<UsersExEntity> exEntities)
        {
            exEntities.ForEach(x => { x.Modifier = LoginUser.UsersId; x.ModifyTime = DateTime.Now; });
            return await userService.DeleteUser(exEntities);
        }
        public async Task<JsonResult> QueryAllRoles()
        {
            var list = await rolesService.GetAllRoles();
            return Json(list);
        }
        [SimpleAction]
        public async Task<bool> SaveUsersRole(UserRoleExEntity userRoleExEntity)
        {
            userRoleExEntity.Createtime = DateTime.Now;
            userRoleExEntity.Creator = LoginUser.UsersId;
            return await userService.UpdateUsersRole(userRoleExEntity);
        }
        public async Task<JsonResult> QueryDevices()
        {
            var list = await unitService.GetUnitAndDeviceTree();
            return Json(list);
        }
        public async Task<JsonResult> QueryUsersDevice(int userId)
        {
            var list = await dmDomainService.GetDeviceIdsByUser(userId);
            return Json(list);
        }
        [SimpleAction]
        public async Task<bool> SaveUsersDevice([FromBody] object json)
        {
            JObject jObject = JObject.Parse(json.ToString());
            var nodes = JsonConvert.DeserializeObject<List<ElementTreeModel>>(jObject.SelectToken("nodes").ToString());
            var userId = JsonConvert.DeserializeObject<int>(jObject.SelectToken("userId").ToString());
            //await RecordLog("设备分配", nodes, Infrastructure.Enums.OperateTypeEnum.修改);
            return await dmDomainService.UpdateAuthLimits(nodes, userId);
        }
        public async Task<FileResult> ExportExcel(Pagination<UsersExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await userService.GetUserPage(pagination);
            var dt = new DataTable();
            string[] columns = { "用户名", "单位", "电话", "邮箱", "创建时间", "创建人", "备注" };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["用户名"] = x.UsersName;
                dr["单位"] = x.Unit.UNITNAME;
                dr["电话"] = x.Telephone;
                dr["邮箱"] = x.Email;
                dr["创建时间"] = x.CreateTime;
                dr["创建人"] = x.CreateStr;
                dr["备注"] = x.Remark;
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }
}