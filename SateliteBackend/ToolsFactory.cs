using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using satelite.interfaces;

namespace satelite.backend
{
    public class ToolsFactory : IToolsFactory
    {
        ILifetimeScope container;

        public ToolsFactory(ILifetimeScope c)
        {
            container = c;
        }

        public Vector CreateVector(float x, float y, float z)
        {
            return container.Resolve<Vector>(
                new TypedParameter(typeof(float), x),
                new TypedParameter(typeof(float), y),
                new TypedParameter(typeof(float), z)
            );
        }

        public IManiobraRotacion CreateManiobra(ActitudRotacion siguienteActitud, ISateliteData sateliteData, Vector orientacionSolicitada)
        {
            return container.Resolve<IManiobraRotacion>(
                new NamedParameter("siguienteActitud", siguienteActitud),
                new NamedParameter("sateliteData", sateliteData),
                new NamedParameter("orientacionSolicitada", orientacionSolicitada)
            );
        }

        public ISatelite CreateSatelite(Vector posicionInicial, Vector velocidadInicial)
        {
            var sateliteData = container.Resolve<ISateliteData>(
                new NamedParameter("posicion", posicionInicial),
                new NamedParameter("velocidad", velocidadInicial)
                );

            var calculadorRotacion = container.Resolve<CalculadorRotacion>(
                new NamedParameter("sateliteData", sateliteData)
                );

            var calculadorMovimiento = container.Resolve<CalculadorMovimiento>(
                new NamedParameter("sateliteData", sateliteData)
                );

            var decisiones = container.Resolve<IEnumerable<IDecision>>(
                new NamedParameter("sateliteData", sateliteData)
                );

            var mente = container.Resolve<IMenteSatelite>(
                new NamedParameter("sateliteData", sateliteData),
                new NamedParameter("decisiones", decisiones)
            );

            var motor = container.Resolve<IMotorSatelite>(
                new NamedParameter("sateliteData", sateliteData)
            );

            return container.Resolve<ISatelite>(
                new NamedParameter("sateliteData", sateliteData),
                new NamedParameter("calculadorRotacion", calculadorRotacion),
                new NamedParameter("calculadorMovimiento", calculadorMovimiento),
                new NamedParameter("mente", mente),
                new NamedParameter("motor", motor)
            );
        }
    }
}
