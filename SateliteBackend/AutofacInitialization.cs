using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using satelite.backend.decision;
using satelite.backend.orbital;
using satelite.interfaces;

namespace satelite.backend
{
    public class AutofacInitialization : Module, ISateliteModules
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ToolsFactory>().As<IToolsFactory>().SingleInstance();
            builder.RegisterType<CalculadorRotacion>().AsSelf();
            builder.RegisterType<CalculadorMovimiento>().AsSelf();
            builder.RegisterType<Constantes>().AsSelf().SingleInstance();
            builder.RegisterType<ManiobraRotacion>().AsSelf();
            builder.RegisterType<ConversorOrbital>().AsSelf().SingleInstance();
            builder.RegisterType<ManiobraRotacion>().As<IManiobraRotacion>();
            builder.RegisterType<SateliteData>().As<ISateliteData>();
            builder.RegisterType<Satelite>().As<ISatelite>();
            builder.RegisterType<MenteSatelite>().As<IMenteSatelite>();
            builder.RegisterType<MotorSatelite>().As<IMotorSatelite>();

            builder.Register((c, p) => new CalcularSentidoDeLaOrbita(
                                            c.Resolve<Constantes>(),
                                            c.Resolve<IVectorTools>(),
                                            0
                                            ))
                   .As<IDecision>();

            builder.Register((c, p) => new CalcularApoapsis(
                                            c.Resolve<Constantes>(),
                                            c.Resolve<IVectorTools>(),
                                            1
                                            ))
                   .As<IDecision>();

            builder.Register((c, p) => new CalcularPeriapsis(
                                            c.Resolve<Constantes>(),
                                            c.Resolve<IVectorTools>(),
                                            2
                                            ))
                   .As<IDecision>();

            builder.Register((c, p) => new CalcularInclinacion(
                                            c.Resolve<Constantes>(),
                                            c.Resolve<IVectorTools>(),
                                            3,
                                            c.Resolve<ConversorOrbital>()
                                            ))
                   .As<IDecision>();

            builder.Register((c, p) => new Circularizar(
                                            c.Resolve<Constantes>(),
                                            c.Resolve<IVectorTools>(),
                                            4
                                            ))
                   .As<IDecision>();

            builder.Register((c, p) => new Esperar(
                                            c.Resolve<Constantes>(),
                                            c.Resolve<IVectorTools>(),
                                            5,
                                            60F
                                            ))
                   .As<IDecision>();
        }
    }
}
