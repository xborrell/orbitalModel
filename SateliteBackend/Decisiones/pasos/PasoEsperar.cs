using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;

namespace satelite.backend.decision.paso
{
    public class PasoEsperar : Paso
    {
        Constantes constantes;
        public float TiempoAEsperar { get; protected set; }
        public float TiempoRestante { get; protected set; }

        public PasoEsperar(Constantes constantes, float segundosAEsperar)
            : this(constantes, segundosAEsperar, null) { }

        public PasoEsperar(Constantes constantes, float segundosAEsperar, LogItem logItem)
            : base()
        {
            this.constantes = constantes;
            TiempoRestante = TiempoAEsperar = segundosAEsperar;
            LogData = logItem;
        }

        override public void Ejecutar(ISateliteData data)
        {
            TiempoRestante -= constantes.FixedDeltaTime;

            PasoFinalizado = (TiempoRestante < 0);
        }
    }
}