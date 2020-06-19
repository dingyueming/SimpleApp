using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simple.Application;
using Simple.Infrastructure;
using Simple.Web.Other;
using Simple.Web.Extension.ServiceExpend;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Simple.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options =>
            {
                //options.Filters.Add<ActionFilterAttribute>()
            }).AddJsonOptions(option =>
            {
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region SignalR
            services.AddSignalR().AddJsonProtocol(option => { option.PayloadSerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; });
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<MapHub>();

            #endregion

            #region Ids4（暂不用）

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = "Cookies";
            //        options.DefaultChallengeScheme = "oidc";
            //    })
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.SignInScheme = "Cookies";

            //        options.Authority = "http://localhost:6543";
            //        options.RequireHttpsMetadata = false;

            //        options.ClientId = "mvc";
            //        options.ClientSecret = "secret";
            //        options.ResponseType = "code id_token";

            //        options.SaveTokens = true;
            //        options.GetClaimsFromUserInfoEndpoint = true;

            //        options.Scope.Add("api1");
            //        options.Scope.Add("offline_access");

            //        options.ClaimActions.MapJsonKey("website", "website");
            //    });

            #endregion

            #region Cookie登陆

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    //需要自定义登陆以及登出页面修改这里
                    //o.LoginPath = new PathString("/Account/Login");
                    //o.AccessDeniedPath = new PathString("/Error/Forbidden");
                    o.ExpireTimeSpan = DateTimeOffset.UtcNow.AddMinutes(60) - DateTimeOffset.UtcNow;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = "client",//Audience
                        ValidIssuer = "server",//Issuer，这两项和前面签发jwt的设置一致
                        ClockSkew = TimeSpan.Zero,//校验时间是否过期时，设置的时钟偏移量
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"])),//拿到SecurityKey
                    };
                });

            #endregion

            #region 注册配置文件 Microsoft.Extensions.Configuration.Json

            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            #endregion

            #region AutoFac
            ContainerBuilder builder = new ContainerBuilder();
            //将services中的服务填充到autoFac中.
            builder.Populate(services);
            //新模块组件注册
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<InfrastructureModule>();
            //创建容器.
            var autoFacContainer = builder.Build();
            //使用容器创建 autoFacServiceProvider 
            var autofacServicePorovider = new AutofacServiceProvider(autoFacContainer);
            //IServiceProvider
            ServiceLocator.SetServices(autofacServicePorovider);

            Task.Run(async () =>
            {
                var hub = autofacServicePorovider.GetService<MapHub>();
                //监听位置信息
                await hub.GetData();
            });

            return autofacServicePorovider;
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSignalR(route =>
            {
                route.MapHub<MapHub>("/mapHub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=index}/{id?}");
            });
        }
    }
}
