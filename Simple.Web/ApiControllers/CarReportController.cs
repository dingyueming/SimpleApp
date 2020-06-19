using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
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
    public class CarReportController : ApiBaseController
    {
        private readonly ICarMsgReportService carMsgReportService;
        public CarReportController(ICarMsgReportService carMsgReportService)
        {
            this.carMsgReportService = carMsgReportService;
        }

        [HttpGet]
        public async Task<ApiResult<object>> Get(DateTime startTime, DateTime endTime)
        {
            try
            {
                var carMsgReportExEntities = await carMsgReportService.GetCarMsgReportExEntities(new DateTime[] { startTime, endTime });
                return ApiResult<object>.Success(carMsgReportExEntities);
            }
            catch (Exception e)
            {

                return ApiResult<object>.Fail(e.Message);
            }
        }
        [HttpPost]
        public async Task<ApiResult<object>> Post(CarMsgReportExEntity value)
        {
            try
            {

                //var authResult = await HttpContext.Authentication.AuthenticateAsync();
                var isSuccess = await carMsgReportService.Add(value);
                if (isSuccess)
                {
                    return ApiResult<object>.Success();
                }
                return ApiResult<object>.Fail("车辆报备失败");
            }
            catch (Exception e)
            {

                return ApiResult<object>.Fail(e.Message);
            }
        }

    }
}
