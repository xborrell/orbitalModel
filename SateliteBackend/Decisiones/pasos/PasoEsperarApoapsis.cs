using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision.paso
{
    public class PasoEsperarApoapsis : Paso
    {
        public PasoEsperarApoapsis(ISateliteData data)
            : base(data)
        {
            LogData = new LogItem(LogType.Paso, "Esperant Apoapsis");
        }

        override public void Ejecutar()
        {
            if (Data.AlturaDeReferencia > Data.Altura)
            {
                PasoFinalizado = true;
            }
            else
            {
                Data.AlturaDeReferencia = Data.Altura;
                this.SegundosAEsperar = 30;
            }
        }
    }
}