using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Simple.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContext>().AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterType<DbContextFactory>().AsImplementedInterfaces().PropertiesAutowired();
        }
    }
}
