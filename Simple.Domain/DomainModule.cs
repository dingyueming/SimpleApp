using System;
using Autofac;
namespace Simple.Domain
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<UserService>().AsImplementedInterfaces();

            //builder.RegisterModule<ApplicationModule>();
        }
    }
}
