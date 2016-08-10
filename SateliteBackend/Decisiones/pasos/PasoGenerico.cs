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
        public Func<ISateliteData, bool> Accion { get; protected set; }

        public PasoGenerico(LogItem logItem, Func<ISateliteData, bool> paso)
        {
            Accion = paso;
            LogData = logItem;
        }

        override public void Ejecutar(ISateliteData data)
        {
            if (Accion(data))
                PasoFinalizado = true;
        }
    }
}