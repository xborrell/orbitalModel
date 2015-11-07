using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.interfaces
{
    public interface ISateliteData
    {
        Vector Velocidad { get; set; }
        Vector Posicion { get; set; }
        Vector Orientacion { get; set; }
        Vector Camara { get; set; }
        ActitudRotacion Actitud { get; set; }
        ActitudRotacion ActitudSolicitada { get; set; }
        float Impulso { get; set; }
        float ImpulsoSolicitado { get; set; }
        float AlturaDeReferencia { get; set; }
        float Apoapsis { get; set; }
        float Periapsis { get; set; }
        float Inclinacion { get; set; }
        float SemiejeMayor { get; }
        bool? OrbitaSubiendo { get; set; }
        float Altura { get; }
        float VelocidadPeriapsis { get; }
        void InvalidateOrbitalValues();
    }
}