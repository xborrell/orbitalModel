using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision.paso
{
    public class PasoEnfoqueOrbital : Paso
    {
        public PasoEnfoqueOrbital(ISateliteData data)
            : base(data)
        {
            LogData = new LogItem(LogType.Paso, "Orientació orbital", "Demanar l'orientació orbital.");
        }

        override public void Ejecutar()
        {
            Data.ActitudSolicitada = ActitudRotacion.Orbital;

            PasoFinalizado = true;
        }
    }
}