using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using satelite.backend.decision.paso;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision
{
    abstract public class Decision : IDecision
    {
        public int Prioridad { get; protected set; }
        protected Constantes constantes;

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
                    Debug.Assert(value.Tipo == LogType.Decision);
                    _logItem = value;
                }
            }
        }
        ILogItem _logItem = null;

        private readonly List<IPaso> pasosAEjecutar = new List<IPaso>();
        private readonly List<IPaso> DefinicionDePasos = new List<IPaso>();
        public IPaso PasoActual => pasosAEjecutar.Count > 0 ? pasosAEjecutar[0] : null;

        public bool DecisionFinalizada { get; protected set; }
        protected IVectorTools VectorTools { get; set; }

        abstract public bool DebeActuar(ISateliteData data);

        protected Decision(Constantes constantes, IVectorTools vectorTools, int prioridad)
        {
            this.constantes = constantes;
            VectorTools = vectorTools;
            Prioridad = prioridad;
        }

        virtual public void Inicializar(ISateliteData data)
        {
            DecisionFinalizada = false;
            pasosAEjecutar.AddRange(DefinicionDePasos);
            pasosAEjecutar.ForEach(x => x.Inicializar(data));

            if (pasosAEjecutar.Count > 0)
                Log.Paso(pasosAEjecutar[0].LogData);
        }

        public string AccionEnCurso
        {
            get
            {
                int index = 0;

                while (index < pasosAEjecutar.Count)
                {
                    ILogItem logData = pasosAEjecutar[index].LogData;
                    if (logData != null)
                        return logData.Titulo;

                    index++;
                }

                return string.Empty;
            }
        }

        protected void DefinirPaso(Paso paso)
        {
            DefinicionDePasos.Add(paso);
        }

        public void Actua(ISateliteData data)
        {
            while (pasosAEjecutar[0].PasoFinalizado)
            {
                bool informarDelCambioDePaso = !(pasosAEjecutar[0] is PasoEsperar);

                pasosAEjecutar.RemoveAt(0);

                if (pasosAEjecutar.Count == 0)
                {
                    DecisionFinalizada = true;
                    return;
                }

                if (informarDelCambioDePaso)
                    Log.Paso(pasosAEjecutar[0].LogData);
            }

            pasosAEjecutar[0].Ejecutar(data);

            var segundosAEsperar = pasosAEjecutar[0].SegundosAEsperar;

            if (segundosAEsperar > 0)
            {
                pasosAEjecutar.Insert(0, new PasoEsperar(constantes, segundosAEsperar));
                pasosAEjecutar[0].SegundosAEsperar = 0;
            }
        }

        protected void SolicitarEspera(float segundosAEsperar)
        {
            pasosAEjecutar.Insert(0, new PasoEsperar(constantes, segundosAEsperar));
        }
    }
}