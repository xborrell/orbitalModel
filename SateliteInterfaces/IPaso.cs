using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using satelite.interfaces.log;

namespace satelite.interfaces
{
    public interface IPaso : ILogable
    {
        ISateliteData Data { get; }
        bool PasoFinalizado { get; }
        float SegundosAEsperar { get; set; }

        void Inicializar();
        void Ejecutar();
    }
}