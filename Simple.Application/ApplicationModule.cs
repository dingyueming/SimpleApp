using Autofac;
using Simple.Application.SM;
using Simple.Domain;

namespace Simple.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DomainModule>();

            builder.RegisterType<UserService>().AsImplementedInterfaces();
            
        }
    }
}
