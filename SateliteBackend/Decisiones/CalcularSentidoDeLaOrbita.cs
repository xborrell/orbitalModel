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
        public override bool DebeActuar(ISateliteData data)
        {
            return (((data.Periapsis < 0) || (data.Apoapsis < 0)) && (data.OrbitaSubiendo == null));
        }

        public override void Inicializar(ISateliteData data)
        {
            base.Inicializar(data);

            data.AlturaDeReferencia = -1;
        }

        public CalcularSentidoDeLaOrbita(Constantes constantes, IVectorTools vectorTools, int prioridad)
            : base(constantes, vectorTools, prioridad)
        {
            DefinirPaso(new PasoEnfoqueATierra());
            DefinirPaso(new PasoComprobarEnfoque(ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoTomarAltura());
            DefinirPaso(new PasoEsperar(constantes, 5, new LogItem(LogType.Paso, "Esperar", "Esperant per evaluar el sentit")));
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Sentit orbita", "Comprobant el sentit de l'orbita"), ComprobarSiLaOrbitaSubeOBaja));

            LogData = new LogItem(LogType.Decision, "Calc. sentit", "Calculant el sentit de l'orbita");
        }

        bool ComprobarSiLaOrbitaSubeOBaja(ISateliteData data)
        {
            if (data.AlturaDeReferencia > data.Altura)
            {
                data.OrbitaSubiendo = false;
                return true;
            }

            if (data.AlturaDeReferencia < data.Altura)
            {
                data.OrbitaSubiendo = true;
                return true;
            }

            SolicitarEspera(5);
            return false;
        }
    }
}