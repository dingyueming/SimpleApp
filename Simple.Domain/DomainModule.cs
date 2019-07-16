using System;
using Autofac;
using Simple.Repository;

namespace Simple.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SmDomainService>().AsImplementedInterfaces();

            builder.RegisterModule<RepositoryModule>();
        }
    }
}
