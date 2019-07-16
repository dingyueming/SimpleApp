using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Simple.Infrastructure.Tools;

namespace Simple.Infrastructure
{
    public class InfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigTool>().AsImplementedInterfaces();
        }
    }
}
