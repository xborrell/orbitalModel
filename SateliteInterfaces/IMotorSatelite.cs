using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.interfaces;

namespace satelite.backend
{
    public interface IMotorSatelite
    {
        void FullPower();
        void Stop();
        void CambioDeImpulso(float cambioDeImpulsoPedido);
        void CalcularImpulso();
    }
}