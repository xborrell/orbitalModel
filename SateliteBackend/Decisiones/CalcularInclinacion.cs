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
        readonly ConversorOrbital conversor;

        public override bool DebeActuar(ISateliteData data)
        {
            return data.Inclinacion < 0;
        }

        public CalcularInclinacion(Constantes constantes, IVectorTools vectorTools, int prioridad, ConversorOrbital conversor)
            : base(constantes, vectorTools, prioridad)
        {
            this.conversor = conversor;

            DefinirPaso(new PasoEnfoqueATierra());
            DefinirPaso(new PasoComprobarEnfoque(ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Calc. inclinació", "Calcular l'inclinació"), Calcular));

            LogData = new LogItem(LogType.Decision, "Calc. Inclinació", "Calculant Inclinació");
        }

        bool Calcular(ISateliteData data)
        {
            var elementos = conversor.Convertir(data.Posicion, data.Velocidad);

            data.Inclinacion = elementos.Inclination;
            return true;
        }
    }
}