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

            //注册程序集下所有的服务类
            var assembly = System.Reflection.Assembly.Load("Simple.Application");
            builder.RegisterAssemblyTypes(assembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces();

        }
    }
}
