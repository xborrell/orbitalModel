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
        IEnumerable<IDecision> decisiones;
        public IDecision DecisionEnCurso { get; protected set; }
        ISateliteData data;

        public MenteSatelite(ISateliteData sateliteData, IEnumerable<IDecision> decisiones)
        {
            this.data = data;
            this.decisiones = decisiones.OrderBy(d => d.Prioridad);

            ObtieneLaSiguienteDecision();
        }

        public void Pulse()
        {
            if (DecisionEnCurso == null)
                ObtieneLaSiguienteDecision();

            else if (DecisionEnCurso.DecisionFinalizada)
                FinalizaDecision();

            else
                DecisionEnCurso.Actua();
        }

        void ObtieneLaSiguienteDecision()
        {
            foreach (Decision decision in decisiones)
            {
                if (decision.DebeActuar())
                {
                    Log.Decision(decision.LogData);
                    decision.Inicializar();
                    DecisionEnCurso = decision;

                    return;
                }
            }
        }

        private void FinalizaDecision()
        {
            Log.Decision( DecisionEnCurso.LogData);
            DecisionEnCurso = null;

            if (data.Actitud != ActitudRotacion.CaidaLibre)
                data.ActitudSolicitada = ActitudRotacion.CaidaLibre;
        }
    }
}