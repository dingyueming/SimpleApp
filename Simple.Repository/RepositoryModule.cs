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
            //注册程序集下所有的服务类
            var assembly = System.Reflection.Assembly.Load("Simple.Repository");
            builder.RegisterAssemblyTypes(assembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces();

        }
    }
}
