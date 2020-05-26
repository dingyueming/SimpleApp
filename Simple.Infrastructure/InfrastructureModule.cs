using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Simple.Infrastructure.Dapper.Contrib;
using Simple.Infrastructure.Tools;

namespace Simple.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionFactory>().PropertiesAutowired();
            builder.RegisterType<RedisHelper>().PropertiesAutowired();
        }
    }
}
