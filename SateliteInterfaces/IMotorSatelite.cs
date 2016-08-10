using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.interfaces;

namespace satelite.backend
{
    public interface IMotorSatelite
    {
        void FullPower(ISateliteData data);
        void Stop(ISateliteData data);
        void CambioDeImpulso(ISateliteData data, float cambioDeImpulsoPedido);
        void CalcularImpulso(ISateliteData data);
    }
}