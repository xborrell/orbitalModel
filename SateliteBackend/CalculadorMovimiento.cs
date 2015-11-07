using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class CalculadorMovimiento
    {
        ISateliteData _data;
        Constantes constantes;

        public CalculadorMovimiento(Constantes constantes, ISateliteData sateliteData)
        {
            _data = sateliteData;
            this.constantes = constantes;
        }

        public void CalcularNuevaPosicion()
        {
            Vector fuerzaGravitatoria = CalcularFuerzaGravitatoria();
            Vector aceleracionGravitatoria = fuerzaGravitatoria / constantes.SatellitMass;

            _data.Velocidad += aceleracionGravitatoria * constantes.FixedDeltaTime;
            Vector desplazamiento = _data.Velocidad * constantes.FixedDeltaTime;
            _data.Posicion += desplazamiento;
        }

        Vector CalcularFuerzaGravitatoria()
        {
            double gravitationModulus = CalcularAtraccionTerrestre(constantes.SatellitMass, _data.Posicion.Magnitude);
            Vector gravitationForce = _data.Posicion.Clone();
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