using System;
using Autofac;
using AutoMapper;
using Simple.Repository;

namespace Simple.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmDomainService>().AsImplementedInterfaces();

            //注册automapper
            AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SimpleProfile>();
            });
            builder.RegisterInstance(config);
            builder.RegisterType<Mapper>().AsImplementedInterfaces();

            //注册模块
            builder.RegisterModule<RepositoryModule>();
        }
    }
}
