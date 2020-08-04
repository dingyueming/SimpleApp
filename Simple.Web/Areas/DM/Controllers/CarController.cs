using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.Extension.ControllerEx;
using Simple.Infrastructure.InfrastructureModel.Paionation;
using Simple.Web.Controllers;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace Simple.Web.Areas.DM.Controllers
{
    [Area("DM")]
    public class CarController : SimpleBaseController
    {
        private readonly ICarService carService;
        private readonly IAreaAlarmService areaAlarmService;
        public CarController(IAreaAlarmService areaAlarmService, ICarService carService)
        {
            this.areaAlarmService = areaAlarmService;
            this.carService = carService;
        }
        public async Task<JsonResult> Query(Pagination<CarExEntity> pagination)
        {
            if (pagination.Where.IndexOf("and a.unitid") == -1)
            {
                pagination.Where += $" and a.unitid in (SELECT UNITID FROM UNIT START WITH UNITID ={LoginUser.UnitId}  CONNECT BY PRIOR UNITID = PID)";
            }
            var data = await carService.GetPage(pagination);
            return Json(data);
        }
        [SimpleAction]
        public async Task<bool> Add(CarExEntity exEntity)
        {
            return await carService.Add(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Update(CarExEntity exEntity)
        {
            exEntity.RECORDDATE = DateTime.Now;
            exEntity.RECMAN = LoginUser.UsersId;
            return await carService.Update(exEntity);
        }
        [SimpleAction]
        public async Task<bool> Delete(CarExEntity exEntity)
        {
            return await carService.Delete(new List<CarExEntity>() { exEntity });
        }
        [SimpleAction]
        public async Task<bool> BatchDelete(List<CarExEntity> exEntities)
        {
            return await carService.Delete(exEntities);
        }
        public async Task<JsonResult> QueryAlarmArea()
        {
            var list = await areaAlarmService.GetAllAras();
            return FormerJson(list);
        }
        [SimpleAction]
        public async Task SaveCarArea(CarAreaExEntity exEntity)
        {
            await areaAlarmService.AddCarArea(exEntity);
        }
        public async Task<FileResult> ExportExcel(Pagination<CarExEntity> pagination)
        {
            pagination.PageSize = 10000;
            var data = await carService.GetPage(pagination);
            var dt = new DataTable();
            string[] columns = { "单位", "显示名", "车牌号", "识别码", "SIM卡号", "车辆类别", "技术参数", "参数简要", "发动机号", "车架号", };
            columns.ToList().ForEach(x =>
            {
                dt.Columns.Add(x);
            });
            string usageStr = "[{\"id\":1,\"name\":\"灭火消防车\",\"sub\":[{\"id\":1,\"name\":\"水罐消防车\"},{\"id\":2,\"name\":\"泡沫消防车\"}," +
                "{\"id\":3,\"name\":\"压缩空气泡沫消防车\"},{\"id\":4,\"name\":\"泡沫干粉联用消防车\"},{\"id\":5,\"name\":\"干粉消防车\"}]}," +
                "{\"id\":2,\"name\":\"举高消防车\",\"sub\":[{\"id\":6,\"name\":\"云梯消防车\"},{\"id\":7,\"name\":\"登高平台消防车\"}," +
                "{\"id\":8,\"name\":\"举高喷射消防车\"}]},{\"id\":3,\"name\":\"专勤消防车\",\"sub\":[{\"id\":9,\"name\":\"抢险救援消防车\"}," +
                "{\"id\":10,\"name\":\"照明消防车\"},{\"id\":11,\"name\":\"排烟消防车\"},{\"id\":12,\"name\":\"化学事故抢险救援\"}," +
                "{\"id\":13,\"name\":\"防化洗消消防车\"},{\"id\":14,\"name\":\"核生化侦检消防车\"},{\"id\":15,\"name\":\"通信指挥消防车\"}]}," +
                "{\"id\":4,\"name\":\"战勤保障车\",\"sub\":[{\"id\":16,\"name\":\"供气消防车\"},{\"id\":17,\"name\":\"器材消防车\"}," +
                "{\"id\":18,\"name\":\"供液消防车\"},{\"id\":19,\"name\":\"供水消防车\"},{\"id\":20,\"name\":\"自装卸式消防车\"},{\"id\":21,\"name\":\"装备抢修车\"}," +
                "{\"id\":22,\"name\":\"饮食保障车\"},{\"id\":23,\"name\":\"加油车\"},{\"id\":24,\"name\":\"运兵车\"},{\"id\":25,\"name\":\"宿营车\"}," +
                "{\"id\":26,\"name\":\"卫勤保障车\"},{\"id\":27,\"name\":\"发电车\"},{\"id\":28,\"name\":\"淋浴车\"},{\"id\":29,\"name\":\"工程机械车辆\"}]}," +
                "{\"id\":5,\"name\":\"公务车\",\"sub\":[]}]";
            var listUsage = JsonConvert.DeserializeObject<List<Usage>>(usageStr);
            foreach (var x in data.Data)
            {
                DataRow dr = dt.NewRow();
                dr["车牌号"] = x.CARNO;
                dr["显示名"] = x.LICENSE;
                dr["单位"] = x.Unit.UNITNAME;
                dr["识别码"] = x.MAC;
                dr["发动机号"] = x.ENGINENO;
                dr["车架号"] = x.CHASSISNO;
                dr["SIM卡号"] = x.SIM;
                dr["技术参数"] = x.TECH_PARAMETERS;
                dr["参数简要"] = x.TECH_PARAMETERS_BRIEF;
                if (listUsage.Any(o => o.Id == x.USAGE))
                {
                    var u = listUsage.FirstOrDefault(o => o.Id == x.USAGE);
                    dr["车辆类别"] = u.Name;
                }
                dt.Rows.Add(dr);
            }
            var buffer = await OutputExcel(dt, columns);
            return File(buffer, "application/ms-excel");
        }
    }

    public class Usage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Usage> Sub { get; set; }
    }
}