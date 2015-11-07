using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.interfaces
{
    public interface IManiobraRotacion
    {
        bool ManiobraCompletada { get; }
        ActitudRotacion SiguienteActitud { get; }
        Vector CalcularNuevaOrientacion();
    }
}