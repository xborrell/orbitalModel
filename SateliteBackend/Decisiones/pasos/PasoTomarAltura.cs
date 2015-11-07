using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision.paso
{
    public class PasoTomarAltura : Paso
    {
        public PasoTomarAltura(ISateliteData data)
            : base(data)
        {
            LogData = new LogItem(LogType.Paso, "Alçada referencia", "Mesurar l'Alçada de Referencia");
        }

        override public void Ejecutar()
        {
            Data.AlturaDeReferencia = Data.Altura;
            PasoFinalizado = true;
        }
    }
}