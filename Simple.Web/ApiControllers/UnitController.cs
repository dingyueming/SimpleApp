using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                return ApiResult<object>.Success(units.Select(x => new { x.UNITID, x.UNITNAME, x.PID }));
            }
            catch (Exception e)
            {

                return ApiResult<object>.Fail(e.Message);
            }
        }
    }
}
