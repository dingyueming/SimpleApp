using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Simple.Common
{
    public static class Extensions
    {
        public static IServiceCollection AddMvcDI(this IServiceCollection services)
        {
            return services;
        }

        public static IApplicationBuilder UseMvcDI(this IApplicationBuilder builder)
        {
            DI.ServiceProvider = builder.ApplicationServices;
            return builder;
        }
    }

    public static class DI
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
