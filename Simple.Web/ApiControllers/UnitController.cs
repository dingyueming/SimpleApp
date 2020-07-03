using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple.ExEntity.DM;
using Simple.IApplication.DM;
using Simple.Web.ApiControllers.Models;

namespace Simple.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ApiBaseController
    {
        private readonly IUnitService unitService;
        public UnitController(IUnitService unitService)
        {
            this.unitService = unitService;
        }

        [HttpGet]
        public async Task<ApiResult<object>> Get()
        {
            try
            {
                var units = await unitService.GetAllUnitExEntities();
                var result = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                var unitId = int.Parse(result.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
                var list = new List<UnitExEntity>();
                var unit = units.FirstOrDefault(x => x.UNITID == unitId);
                if (unit != null)
                {
                    list.Add(unit);
                }
                var childUnits = GetUnits(units, unitId);
                if (childUnits.Count > 0)
                {
                    list.AddRange(childUnits);
                }
                return ApiResult<object>.Success(list.Select(x => new { x.UNITID, x.UNITNAME, x.PID }));
            }
            catch (Exception e)
            {

                return ApiResult<object>.Fail(e.Message);
            }
        }
        private List<UnitExEntity> GetUnits(List<UnitExEntity> allUnit, int unitId)
        {
            var list = new List<UnitExEntity>();
            var units = allUnit.Where(x => x.PID == unitId).ToList();
            foreach (var item in units)
            {
                list.Add(item);
                var childUnits = GetUnits(allUnit, item.UNITID);
                if (childUnits.Count > 0)
                {
                    list.AddRange(childUnits);
                }
            }
            return list;
        }
    }
}
