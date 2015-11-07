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
    public class CalcularSentidoDeLaOrbita : Decision
    {
        public override bool DebeActuar()
        {
            return (((Data.Periapsis < 0) || (Data.Apoapsis < 0)) && (Data.OrbitaSubiendo == null));
        }

        public override void Inicializar()
        {
            base.Inicializar();

            Data.AlturaDeReferencia = -1;
        }

        public CalcularSentidoDeLaOrbita(Constantes constantes, IVectorTools vectorTools, ISateliteData data, int prioridad)
            : base(constantes, vectorTools, data, prioridad)
        {
            DefinirPaso(new PasoEnfoqueATierra(data));
            DefinirPaso(new PasoComprobarEnfoque(data, ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoTomarAltura(data));
            DefinirPaso(new PasoEsperar(constantes, data, 5, new LogItem(LogType.Paso, "Esperar", "Esperant per evaluar el sentit")));
            DefinirPaso(new PasoGenerico(data, new LogItem(LogType.Paso, "Sentit orbita", "Comprobant el sentit de l'orbita"), ComprobarSiLaOrbitaSubeOBaja));

            LogData = new LogItem(LogType.Decision, "Calc. sentit", "Calculant el sentit de l'orbita");
        }

        bool ComprobarSiLaOrbitaSubeOBaja()
        {
            if (Data.AlturaDeReferencia > Data.Altura)
            {
                Data.OrbitaSubiendo = false;
                return true;
            }

            if (Data.AlturaDeReferencia < Data.Altura)
            {
                Data.OrbitaSubiendo = true;
                return true;
            }

            SolicitarEspera(5);
            return false;
        }
    }
}