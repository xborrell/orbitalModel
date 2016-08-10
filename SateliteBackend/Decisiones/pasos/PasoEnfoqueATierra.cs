using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision.paso
{
    public class PasoEnfoqueATierra : Paso
    {
        public PasoEnfoqueATierra()
        {
            LogData = new LogItem(LogType.Paso, "Orientació terra", "Demanar l'orientació a terra.");
        }

        override public void Ejecutar(ISateliteData data)
        {
            data.ActitudSolicitada = ActitudRotacion.EnfocadoATierra;

            PasoFinalizado = true;
        }
    }
}