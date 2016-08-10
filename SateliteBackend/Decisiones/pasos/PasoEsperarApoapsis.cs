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
        public PasoEsperarApoapsis()
        {
            LogData = new LogItem(LogType.Paso, "Esperant Apoapsis");
        }

        override public void Ejecutar(ISateliteData data)
        {
            if (data.AlturaDeReferencia > data.Altura)
            {
                PasoFinalizado = true;
            }
            else
            {
                data.AlturaDeReferencia = data.Altura;
                this.SegundosAEsperar = 30;
            }
        }
    }
}