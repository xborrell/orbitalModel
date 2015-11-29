using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using satelite.backend;
using satelite.interfaces;
using Xb.Simulador.Model;
using Xb.Simulador.View;
using Xb.Simulador.Viewmodel;

namespace Xb.Simulador
{
    public class AutofacInitialization : Module, ISateliteModules
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<SateliteDataList>().AsSelf().SingleInstance();
            builder.RegisterType<TimeManager>().AsSelf();
            builder.RegisterType<SateliteDataCache>().AsSelf()
                .WithParameter(new TypedParameter(typeof(ISateliteData), "data"))
                ;
        }
    }
}
