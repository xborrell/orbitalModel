using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision.paso
{
    abstract public class Paso : IPaso
    {
        public ILogItem LogData
        {
            get
            {
                return _logItem;
            }
            protected set
            {
                if (_logItem != value)
                {
                    Debug.Assert(value.Tipo == LogType.Paso);
                    _logItem = value;
                }
            }
        }
        ILogItem _logItem = null;

        public bool PasoFinalizado { get; protected set; }
        public float SegundosAEsperar { get; set; }

        protected Paso()
        {
            PasoFinalizado = false;
            SegundosAEsperar = 0;
        }

        virtual public void Inicializar(ISateliteData data)
        {
            PasoFinalizado = false;
        }

        abstract public void Ejecutar(ISateliteData data);
    }
}