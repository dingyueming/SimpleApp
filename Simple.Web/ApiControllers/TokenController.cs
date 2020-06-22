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
    public class TokenController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        public TokenController(IUserService userService, IConfiguration configuration)
        {
            this.userService = userService;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<ApiResult<object>> Get(string uid, string pwd)
        {
            try
            {
                var users = await userService.GetAllUsers();
                var loginUser = users.FirstOrDefault(x => x.UsersName == uid.Trim().ToLower() && x.Password == pwd.Trim().ToLower());
                if (loginUser == null)
                {
                    return ApiResult<object>.Fail("用户名密码错误");
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.NameIdentifier, loginUser.UsersId.ToString()) };//创建声明
                    var now = DateTime.Now;
                    var ex = now + TimeSpan.FromMinutes(30);//过期时间设置为30分钟
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecurityKey"]));//获取密钥
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//加密方式
                    var token = new SecurityTokenDescriptor
                    {
                        Issuer = "server",
                        Audience = "client",
                        Expires = ex,
                        IssuedAt = now,
                        SigningCredentials = creds,
                        Subject = new ClaimsIdentity(claims)
                    };
                    return ApiResult<object>.Success(new { token = new JwtSecurityTokenHandler().CreateEncodedJwt(token) });
                }
            }
            catch (Exception e)
            {
                return ApiResult<object>.Fail(e.Message);
            }
        }
    }
}
