using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision
{
    public class Esperar : Decision
    {
        public Esperar(Constantes constantes, IVectorTools vectorTools, int prioridad, float segundosAEsperar)
            : base(constantes, vectorTools, prioridad)
        {
            SolicitarEspera(segundosAEsperar);

            LogData = new LogItem(LogType.Decision, "Esperant", $"Esperant {segundosAEsperar} segons");
        }

        public override bool DebeActuar(ISateliteData data)
        {
            return true; ;
        }
    }
}