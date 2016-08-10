using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.interfaces;

namespace satelite.backend
{
    public class MotorSatelite : IMotorSatelite
    {
        Action variacionImpulso = null;
        Constantes constantes;

        public MotorSatelite(Constantes constantes)
        {
            this.constantes = constantes;
        }

        public void FullPower(ISateliteData data)
        {
            CambioDeImpulso(data, constantes.ImpulsoMaximo);
        }
        public void Stop(ISateliteData data)
        {
            CambioDeImpulso(data, 0);
        }

        public void CambioDeImpulso(ISateliteData data, float cambioDeImpulsoPedido)
        {
            data.ImpulsoSolicitado = -1;

            if (cambioDeImpulsoPedido > constantes.ImpulsoMaximo)
                cambioDeImpulsoPedido = constantes.ImpulsoMaximo;

            else if (cambioDeImpulsoPedido < 0)
                cambioDeImpulsoPedido = 0;

            if (cambioDeImpulsoPedido > data.Impulso)
            {
                variacionImpulso = () => Acelerar(data, cambioDeImpulsoPedido);
            }

            if (cambioDeImpulsoPedido < data.Impulso)
            {
                variacionImpulso = () => Frenar(data, cambioDeImpulsoPedido);
            }
        }

        public void CalcularImpulso(ISateliteData data)
        {
            if (data.ImpulsoSolicitado >= 0)
            {
                CambioDeImpulso(data, data.ImpulsoSolicitado);
            }

            variacionImpulso?.Invoke();
        }

        void Acelerar(ISateliteData data, float aceleracionSolicitada)
        {
            var variacion = constantes.VariacionMaximaDelImpulso * constantes.FixedDeltaTime;

            data.Impulso += variacion;

            if (data.Impulso >= aceleracionSolicitada)
            {
                data.Impulso = aceleracionSolicitada;
                variacionImpulso = null;
            }
        }

        void Frenar(ISateliteData data, float frenadoSolicitado)
        {
            var variacion = constantes.VariacionMaximaDelImpulso * constantes.FixedDeltaTime;

            data.Impulso -= variacion;

            if (data.Impulso <= frenadoSolicitado)
            {
                data.Impulso = frenadoSolicitado;
                variacionImpulso = null;
            }
        }
    }
}