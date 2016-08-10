using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.decision.paso;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision
{
    public class CalcularPeriapsis : Decision
    {
        public override bool DebeActuar(ISateliteData data)
        {
            return ((data.Periapsis < 0) && (data.OrbitaSubiendo == false));
        }

        public CalcularPeriapsis(Constantes constantes, IVectorTools vectorTools, int prioridad)
            : base(constantes, vectorTools, prioridad)
        {
            DefinirPaso(new PasoEnfoqueATierra());
            DefinirPaso(new PasoComprobarEnfoque(ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoTomarAltura());
            DefinirPaso(new PasoEsperarPeriapsis());
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Registrant Periapsis"),
                (data) =>
                {
                    data.Periapsis = data.AlturaDeReferencia;
                    data.OrbitaSubiendo = null;
                    return true;
                }
            ));

            LogData = new LogItem(LogType.Decision, "Calc. Periapsis", "Calculant Periapsis");
        }
    }
}