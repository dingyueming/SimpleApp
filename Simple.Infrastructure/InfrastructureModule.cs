using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Simple.Infrastructure.Dapper.Contrib;

namespace Simple.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContext>().AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterType<DbContextFactory>().PropertiesAutowired();

            builder.RegisterType<ConnectionFactory>().PropertiesAutowired();
        }
    }
}
