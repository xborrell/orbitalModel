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

        public PasoComprobarEnfoque(ISateliteData data, ActitudRotacion actitudDeseada)
            : base(data)
        {
            ActitudDeseada = actitudDeseada;
            LogData = new LogItem(LogType.Paso, "Esperar Orientació", "Esperar l'orientació demanada.");
        }

        override public void Ejecutar()
        {
            PasoFinalizado = (Data.Actitud == ActitudDeseada);
        }
    }
}