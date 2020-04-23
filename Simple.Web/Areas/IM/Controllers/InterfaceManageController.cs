﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.IM;
using Simple.IApplication.Dwjk;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using System.Security.Cryptography;
using System.Text;

namespace Simple.Web.Areas.IM.Controllers
{
    [Area("IM")]
    public class InterfaceManageController : SimpleBaseController
    {
        private readonly IInterfaceService interfaceService;
        public InterfaceManageController(IInterfaceService interfaceService)
        {
            this.interfaceService = interfaceService;
        }
        public async Task<JsonResult> Query(Pagination<InterfaceExEntity> pagination)
        {
            var data = await interfaceService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task SaveData(InterfaceExEntity exEntity)
        {
            if (exEntity.App_Id == 0)
            {
                var guid = Guid.NewGuid().ToString();
                using (var md5 = MD5.Create())
                {
                    var result = md5.ComputeHash(Encoding.UTF8.GetBytes(guid));
                    var strResult = BitConverter.ToString(result);
                    exEntity.Password = strResult.Replace("-", "");
                }
                await RecordLog("接口管理", exEntity, Infrastructure.Enums.OperateTypeEnum.增加);
                await interfaceService.Add(exEntity);
            }
            else
            {
                await RecordLog("接口管理", exEntity, Infrastructure.Enums.OperateTypeEnum.修改);
                await interfaceService.Update(exEntity);
            }

        }
        [SimpleAction]
        public async Task Delete(List<InterfaceExEntity> exEntities)
        {
            await RecordLog("接口管理", exEntities, Infrastructure.Enums.OperateTypeEnum.删除);
            await interfaceService.Delete(exEntities);
        }
    }
}