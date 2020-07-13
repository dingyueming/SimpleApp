using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public class CarReportController : ApiBaseController
    {
        private readonly ICarMsgReportService carMsgReportService;
        public CarReportController(ICarMsgReportService carMsgReportService)
        {
            this.carMsgReportService = carMsgReportService;
        }

        [HttpGet]
        public async Task<ApiResult<object>> Get(DateTime startTime, DateTime endTime, string carNo)
        {
            try
            {
                var carMsgReportExEntities = await carMsgReportService.GetCarMsgReportExEntities(new DateTime[] { startTime, endTime }, carNo);
                return ApiResult<object>.Success(carMsgReportExEntities.Select(x => new
                {
                    x.CARID,
                    x.Car.LICENSE,
                    x.Car.CARNO,
                    x.APPROVER,
                    x.SENDTIME,
                    x.BACKTIME,
                    x.REMARK,
                    x.CONTENT
                }));
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
                var authResult = await AuthenticationHttpContextExtensions.AuthenticateAsync(HttpContext, "Bearer");
                var creator = int.Parse(authResult.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var exEntities = new List<CarMsgReportExEntity>();
                foreach (var item in value.CARIDS)
                {
                    exEntities.Add(new CarMsgReportExEntity()
                    {
                        CARID = item,
                        CREATETIME = DateTime.Now,
                        CREATOR = creator,
                        APPROVER = value.APPROVER,
                        BACKTIME = value.BACKTIME,
                        CONTENT = value.CONTENT,
                        REMARK = value.REMARK,
                        SENDTIME = value.SENDTIME
                    });
                }
                var isSuccess = await carMsgReportService.Add(exEntities.ToArray());
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
