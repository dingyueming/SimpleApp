using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Simple.IApplication.SM;
using Simple.Web.ApiControllers.Models;

namespace Simple.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ApiBaseController
    {
        private readonly IConfiguration configuration;
        public VersionController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<ApiResult<object>> Get(string no)
        {
            try
            {
                return await Task.Run(() =>
                 {
                     var serverNo = configuration["ApkVersion"];
                     if (serverNo != no)
                     {
                         return ApiResult<object>.Success(new
                         {
                             IsNeedUpdate = true,
                             DownLoadUrl = configuration["ApkUrl"]
                         });

                     }
                     else
                     {
                         return ApiResult<object>.Success(new
                         {
                             IsNeedUpdate = false,
                             DownLoadUrl = ""
                         });
                     }
                 });
            }
            catch (Exception e)
            {
                return ApiResult<object>.Fail(e.Message);
            }
        }
    }
}
