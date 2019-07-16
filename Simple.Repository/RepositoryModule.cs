using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Simple.Repository.SM;

namespace Simple.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbContext>().AsImplementedInterfaces().PropertiesAutowired();
            builder.RegisterType<DbContextFactory>().AsImplementedInterfaces().PropertiesAutowired();
            //注册此程序集下的类型
            builder.RegisterType<AuthRepository>().AsImplementedInterfaces();
        }
    }
}
