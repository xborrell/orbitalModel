using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace satelite.interfaces
{
    public interface IToolsFactory
    {
        Vector CreateVector(float x, float y, float z);
        IManiobraRotacion CreateManiobra(ActitudRotacion siguienteActitud, ISateliteData sateliteData, Vector orientacionSolicitada);
        ISatelite CreateSatelite(Vector posicion, Vector velocidad);
    }
}
