using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace satelite.interfaces
{
    public interface IVectorTools
    {
        float AngleTo(Vector orientacionInicial, Vector orientacionFinal);
        Vector Lerp(Vector orientacionInicial, Vector orientacionFinal, float porcentajeDeRotacion);
        Vector CrossProduct(Vector first, Vector second);
        float DotProduct(Vector first, Vector second);
    }
}
