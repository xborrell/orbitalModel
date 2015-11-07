using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace satelite.interfaces
{
    public interface IMenteSatelite
    {
        IDecision DecisionEnCurso { get; }
        void Pulse();
    }
}