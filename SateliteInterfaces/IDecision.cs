using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using satelite.interfaces.log;

namespace satelite.interfaces
{
    public interface IDecision : ILogable
    {
        int Prioridad { get; }
        IPaso PasoActual { get; }
        bool DecisionFinalizada { get; }
        string AccionEnCurso { get; }

        bool DebeActuar();
        void Inicializar();

        void Actua();
    }
}