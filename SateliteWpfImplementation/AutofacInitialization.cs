using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using satelite.interfaces;

namespace satelite.implementation.wpf
{
    public class AutofacInitialization : Module, ISateliteModules
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WpfVector>().As<Vector>()
                .WithParameter(new TypedParameter(typeof(float), "x"))
                .WithParameter(new TypedParameter(typeof(float), "y"))
                .WithParameter(new TypedParameter(typeof(float), "z"))
                ;

            builder.RegisterType<WpfVectorTools>().As<IVectorTools>();
            builder.RegisterType<WpfVectorTools>().As<IVectorTools>().SingleInstance();
        }
    }
}
