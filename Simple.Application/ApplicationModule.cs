using Autofac;
using Simple.Application.SystemModule;

namespace Simple.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().AsImplementedInterfaces();

            //builder.RegisterModule<ApplicationModule>();
        }
    }
}
