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
    public class CalcularApoapsis : Decision
    {
        public override bool DebeActuar(ISateliteData data)
        {
            return ((data.Apoapsis < 0) && (data.OrbitaSubiendo == true));
        }

        public CalcularApoapsis(Constantes constantes, IVectorTools vectorTools, int prioridad)
            : base(constantes, vectorTools, prioridad)
        {
            DefinirPaso(new PasoEnfoqueATierra());
            DefinirPaso(new PasoComprobarEnfoque(ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoTomarAltura());
            DefinirPaso(new PasoEsperarApoapsis());
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Registrant Apoapsis"),
                (data) =>
                {
                    data.Apoapsis = data.AlturaDeReferencia;
                    data.OrbitaSubiendo = null;
                    return true;
                }
            ));

            LogData = new LogItem(LogType.Decision, "Calc. Apoapsis", "Calculant Apoapsis");
        }
    }
}