using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.decision.paso;
using satelite.backend.log;
using satelite.interfaces;

namespace satelite.backend.decision
{
    public class PosicionarSateliteEnActitudOrbital : Decision
    {
        public PosicionarSateliteEnActitudOrbital(Constantes constantes, IVectorTools vectorTools, int prioridad)
            : base(constantes, vectorTools, prioridad)
        {
            DefinirPaso(new PasoEnfoqueOrbital());
            DefinirPaso(new PasoComprobarEnfoque(ActitudRotacion.Orbital));

            LogData = new LogItem(0, "Orientació orbital", "Orientar el satel·lit amb l'orbita");
        }

        public override bool DebeActuar(ISateliteData data)
        {
            return data.Actitud != ActitudRotacion.Orbital;
        }
    }
}