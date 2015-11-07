using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;

namespace satelite.backend.decision.paso
{
    public class PasoGenerico : Paso
    {
        public Func<bool> Accion { get; protected set; }

        public PasoGenerico(ISateliteData data, LogItem logItem, Func<bool> paso)
            : base(data)
        {
            Accion = paso;
            LogData = logItem;
        }

        override public void Ejecutar()
        {
            if (Accion())
                PasoFinalizado = true;
        }
    }
}