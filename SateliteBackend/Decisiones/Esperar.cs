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
        public Esperar(Constantes constantes, IVectorTools vectorTools, ISateliteData data, int prioridad, float segundosAEsperar)
            : base(constantes, vectorTools, data, prioridad)
        {
            SolicitarEspera(segundosAEsperar);

            LogData = new LogItem(LogType.Decision, "Esperant", string.Format("Esperant {0} segons", segundosAEsperar));
        }

        public override bool DebeActuar()
        {
            return true; ;
        }
    }
}