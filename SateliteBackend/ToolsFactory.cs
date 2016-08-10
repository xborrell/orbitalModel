using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using satelite.interfaces;

namespace satelite.backend
{
    public class ToolsFactory : IToolsFactory
    {
        readonly IComponentContext container;

        public ToolsFactory(IComponentContext c)
        {
            container = c;
        }

        public Vector CreateVector(float x, float y, float z)
        {
            return container.Resolve<Vector>(
                new NamedParameter("x", x),
                new NamedParameter("y", y),
                new NamedParameter("z", z)
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

            return container.Resolve<ISatelite>(
                new NamedParameter("sateliteData", sateliteData)
            );
        }
    }
}
