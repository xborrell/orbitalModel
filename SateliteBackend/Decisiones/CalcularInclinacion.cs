using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.decision.paso;
using satelite.backend.log;
using satelite.backend.orbital;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision
{
    public class CalcularInclinacion : Decision
    {
        ConversorOrbital conversor;

        public override bool DebeActuar()
        {
            return Data.Inclinacion < 0;
        }

        public CalcularInclinacion(Constantes constantes, IVectorTools vectorTools, ISateliteData data, int prioridad, ConversorOrbital conversor)
            : base(constantes, vectorTools, data, prioridad)
        {
            this.conversor = conversor;

            DefinirPaso(new PasoEnfoqueATierra(data));
            DefinirPaso(new PasoComprobarEnfoque(data, ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoGenerico(data, new LogItem(LogType.Paso, "Calc. inclinació", "Calcular l'inclinació"), Calcular));

            LogData = new LogItem(LogType.Decision, "Calc. Inclinació", "Calculant Inclinació");
        }

        bool Calcular()
        {
            OrbitalElements elementos = conversor.Convertir(Data.Posicion, Data.Velocidad);

            Data.Inclinacion = elementos.Inclination;
            return true;
        }
    }
}