using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision.paso
{
    public class PasoComprobarEnfoque : Paso
    {
        public ActitudRotacion ActitudDeseada { get; protected set; }

        public PasoComprobarEnfoque(ActitudRotacion actitudDeseada)
        {
            ActitudDeseada = actitudDeseada;
            LogData = new LogItem(LogType.Paso, "Esperar Orientació", "Esperar l'orientació demanada.");
        }

        override public void Ejecutar(ISateliteData data)
        {
            PasoFinalizado = (data.Actitud == ActitudDeseada);
        }
    }
}