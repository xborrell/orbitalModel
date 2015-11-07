using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.interfaces;

namespace satelite.backend
{
    public class MotorSatelite : IMotorSatelite
    {
        ISateliteData data;
        Action variacionImpulso = null;
        Constantes constantes;

        public MotorSatelite(Constantes constantes, ISateliteData sateliteData)
        {
            this.constantes = constantes;
            this.data = sateliteData;
        }

        public void FullPower()
        {
            CambioDeImpulso(constantes.ImpulsoMaximo);
        }
        public void Stop()
        {
            CambioDeImpulso(0);
        }

        public void CambioDeImpulso(float cambioDeImpulsoPedido)
        {
            data.ImpulsoSolicitado = -1;

            if (cambioDeImpulsoPedido > constantes.ImpulsoMaximo)
                cambioDeImpulsoPedido = constantes.ImpulsoMaximo;

            else if (cambioDeImpulsoPedido < 0)
                cambioDeImpulsoPedido = 0;

            if (cambioDeImpulsoPedido > data.Impulso)
            {
                variacionImpulso = () => Acelerar(cambioDeImpulsoPedido);
            }

            if (cambioDeImpulsoPedido < data.Impulso)
            {
                variacionImpulso = () => Frenar(cambioDeImpulsoPedido);
            }
        }

        public void CalcularImpulso()
        {
            if (data.ImpulsoSolicitado >= 0)
            {
                CambioDeImpulso(data.ImpulsoSolicitado);
            }

            if (variacionImpulso != null)
            {
                variacionImpulso();
            }
        }

        void Acelerar(float aceleracionSolicitada)
        {
            var variacion = constantes.VariacionMaximaDelImpulso * constantes.FixedDeltaTime;

            data.Impulso += variacion;

            if (data.Impulso >= aceleracionSolicitada)
            {
                data.Impulso = aceleracionSolicitada;
                variacionImpulso = null;
            }
        }

        void Frenar(float frenadoSolicitado)
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