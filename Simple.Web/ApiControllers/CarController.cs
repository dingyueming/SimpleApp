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
using RestSharp;
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
                var result = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                var usersId = int.Parse(result.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var cars = await carService.GetAll(usersId);
                return ApiResult<object>.Success(cars.Select(x => new { x.CARID, x.LICENSE, x.CARNO, x.UNITID }));
            }
            catch (Exception e)
            {

                return ApiResult<object>.Fail(e.Message);
            }
        }
    }
}
