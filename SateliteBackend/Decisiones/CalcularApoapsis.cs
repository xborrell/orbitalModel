﻿using System;
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
        public override bool DebeActuar()
        {
            return ((Data.Apoapsis < 0) && (Data.OrbitaSubiendo == true));
        }

        public CalcularApoapsis(Constantes constantes, IVectorTools vectorTools, ISateliteData data, int prioridad)
            : base(constantes, vectorTools, data, prioridad)
        {
            DefinirPaso(new PasoEnfoqueATierra(data));
            DefinirPaso(new PasoComprobarEnfoque(data, ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoTomarAltura(data));
            DefinirPaso(new PasoEsperarApoapsis(data));
            DefinirPaso(new PasoGenerico(data, new LogItem(LogType.Paso, "Registrant Apoapsis"),
                () =>
                {
                    Data.Apoapsis = Data.AlturaDeReferencia;
                    Data.OrbitaSubiendo = null;
                    return true;
                }
            ));

            LogData = new LogItem(LogType.Decision, "Calc. Apoapsis", "Calculant Apoapsis");
        }
    }
}