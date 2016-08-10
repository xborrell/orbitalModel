using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.decision;
using satelite.backend.log;
using satelite.backend.orbital;
using satelite.interfaces;

namespace satelite.backend
{
    public class MenteSatelite : IMenteSatelite
    {
        readonly IEnumerable<IDecision> decisiones;
        public IDecision DecisionEnCurso { get; protected set; }

        public MenteSatelite(IEnumerable<IDecision> decisiones)
        {
            this.decisiones = decisiones.OrderBy(d => d.Prioridad);
        }

        public void Pulse(ISateliteData data)
        {
            if (DecisionEnCurso == null)
                ObtieneLaSiguienteDecision(data);

            else if (DecisionEnCurso.DecisionFinalizada)
                FinalizaDecision(data);

            else
                DecisionEnCurso.Actua(data);
        }

        void ObtieneLaSiguienteDecision(ISateliteData data)
        {
            foreach (var decision in decisiones)
            {
                if (decision.DebeActuar(data))
                {
                    Log.Decision(decision.LogData);
                    decision.Inicializar(data);
                    DecisionEnCurso = decision;

                    return;
                }
            }
        }

        private void FinalizaDecision(ISateliteData data)
        {
            Log.Decision( DecisionEnCurso.LogData);
            DecisionEnCurso = null;

            if (data.Actitud != ActitudRotacion.CaidaLibre)
                data.ActitudSolicitada = ActitudRotacion.CaidaLibre;
        }
    }
}