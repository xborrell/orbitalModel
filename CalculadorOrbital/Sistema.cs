using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend;
using satelite.backend.orbital;
using satelite.interfaces;

namespace CalculadorOrbital
{
    public class Sistema : ISistema
    {
        IToolsFactory factory;
        ISatelite satelite;
        ConversorOrbital conversor;
        Constantes constantes;

        public Sistema(Constantes constantes, IToolsFactory factory, ConversorOrbital conversor)
        {
            this.constantes = constantes;
            this.factory = factory;
            this.conversor = conversor;
        }

        public void Ejecutar()
        {
            var posicionInicial = factory.CreateVector( -822.79774F, -4438.63582F, 5049.31502F);
            var velocidadInicial = factory.CreateVector(7.418175658F, .709253354F, 1.828703177F);
            var satelite = factory.CreateSatelite(posicionInicial, velocidadInicial);

            OrbitalElements elementosIniciales = conversor.Convertir(satelite.Data.Posicion, satelite.Data.Velocidad);
            double remainTimeInSeconds = elementosIniciales.Period.TotalSeconds;
            double deltaTimeInSeconds = constantes.FixedDeltaTime;

            while (remainTimeInSeconds > deltaTimeInSeconds)
            {
                satelite.Pulse();

                remainTimeInSeconds -= deltaTimeInSeconds;
            }

            Console.WriteLine("Periapsis           {0} Km.", satelite.Periapsis);
            Console.WriteLine("Apoapsis            {0} Km.", satelite.Apoapsis);
            Console.WriteLine("Semieje Mayor       {0} Km.", satelite.Data.SemiejeMayor);
            Console.WriteLine("Velocidad Periapsis {0} Km/s", satelite.Data.VelocidadPeriapsis);
        }
    }
}
