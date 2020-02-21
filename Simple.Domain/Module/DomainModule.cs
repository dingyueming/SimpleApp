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
            //注册模块
            builder.RegisterModule<RepositoryModule>();

            //注册程序集下所有的服务类
            //builder.RegisterType<SmDomainService>().AsImplementedInterfaces();
            //var assembly = System.Reflection.Assembly.GetEntryAssembly();
            var assembly = System.Reflection.Assembly.Load("Simple.Domain");
            builder.RegisterAssemblyTypes(assembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces();

            //注册automapper
            IConfigurationProvider config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            builder.RegisterInstance(config);
            builder.RegisterType<Mapper>().AsImplementedInterfaces();

        }
    }
}
