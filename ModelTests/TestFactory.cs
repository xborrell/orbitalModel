using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.implementation.wpf;
using satelite.interfaces;

namespace satelite.backend
{
    public class TestFactory : IToolsFactory
    {
        public Vector CreateVector(float x, float y, float z)
        {
            return new WpfVector( x,y,z);
        }

        public IManiobraRotacion CreateManiobra(ActitudRotacion siguienteActitud, ISateliteData sateliteData, Vector orientacionSolicitada)
        {
            return null;
        }

        public ISatelite CreateSatelite(Vector posicionInicial, Vector velocidadInicial)
        {
            return null;
        }
    }
}
