using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using satelite.interfaces;
using Xb.Simulador.Model;
using Xb.Simulador.View;
using Xb.Simulador.Viewmodel;

namespace satelite.implementation.wpf
{
    public class AutofacInitialization : Module, ISateliteModules
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<SimulatorLoop>().AsSelf();
        }
    }
}
