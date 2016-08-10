using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class CalculadorMovimiento
    {
        readonly Constantes constantes;

        public CalculadorMovimiento(Constantes constantes)
        {
            this.constantes = constantes;
        }

        public void CalcularNuevaPosicion(ISateliteData data)
        {
            Vector fuerzaGravitatoria = CalcularFuerzaGravitatoria(data);
            Vector aceleracionGravitatoria = fuerzaGravitatoria / constantes.SatellitMass;

            data.Velocidad += aceleracionGravitatoria * constantes.FixedDeltaTime;
            Vector desplazamiento = data.Velocidad * constantes.FixedDeltaTime;
            data.Posicion += desplazamiento;
        }

        Vector CalcularFuerzaGravitatoria(ISateliteData data)
        {
            double gravitationModulus = CalcularAtraccionTerrestre(constantes.SatellitMass, data.Posicion.Magnitude);
            Vector gravitationForce = data.Posicion.Clone();
            gravitationForce.Normalize();
            gravitationForce = gravitationForce * gravitationModulus * -1;

            return gravitationForce;
        }

        private double CalcularAtraccionTerrestre(float masaSatelite, float distancia)
        {
            var distancia2 = Math.Pow(distancia, 2);
            var numerador = masaSatelite * constantes.Mu;

            return numerador / distancia2;
        }
    }
}