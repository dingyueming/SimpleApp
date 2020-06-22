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
    public class CarController : ApiBaseController
    {
        private readonly ICarService carService;
        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpGet]
        public async Task<ApiResult<object>> Get()
        {
            try
            {
                var units = await carService.GetAll();
                return ApiResult<object>.Success(units.Select(x => new { x.CARID, x.LICENSE, x.CARNO, x.UNITID }));
            }
            catch (Exception e)
            {

                return ApiResult<object>.Fail(e.Message);
            }
        }
    }
}
