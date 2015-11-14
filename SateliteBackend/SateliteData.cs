using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class SateliteData : ISateliteData
    {
        Constantes constantes;

        public Vector Velocidad { get; set; }
        public Vector Posicion { get; set; }
        public Vector Orientacion { get; set; }
        public Vector Camara { get; set; }
        public ActitudRotacion Actitud { get; set; }
        public ActitudRotacion ActitudSolicitada { get; set; }
        public float Impulso { get; set; }
        public float ImpulsoSolicitado { get; set; }
        public float AlturaDeReferencia { get; set; }
        public float Apoapsis { get; set; }
        public float Periapsis { get; set; }
        public float Inclinacion { get; set; }
        public float SemiejeMayor { get { return (Apoapsis + Periapsis) / 2; } }
        public bool? OrbitaSubiendo { get; set; }

        public float Altura
        {
            get
            {
                if (Actitud == ActitudRotacion.EnfocadoATierra)
                {
                    float alturaAbsoluta = Posicion.Magnitude;
                    float alturaSobreElSuelo = alturaAbsoluta - constantes.EarthRadius;

                    return alturaSobreElSuelo;
                }

                return -1;
            }
        }

        public float VelocidadPeriapsis
        {
            get
            {
                double rp = Periapsis + constantes.EarthRadius;
                double ra = Apoapsis + constantes.EarthRadius;
                double h = Math.Sqrt(constantes.Mu * 2) * Math.Sqrt((ra * rp) / (ra + rp));

                return (float)(h / rp);
            }
        }

        public SateliteData() { }

        public SateliteData(Constantes constantes, Vector posicion, Vector velocidad)
        {
            this.constantes = constantes;

            Velocidad = velocidad;
            Posicion = posicion;

            Orientacion = constantes.XAxis;
            Orientacion.Normalize();

            Camara = posicion.Clone();
            Camara.Normalize();
            Camara = Camara * 10;

            Actitud = ActitudRotacion.CaidaLibre;
            ActitudSolicitada = ActitudRotacion.Ninguna;
            Impulso = 0;
            ImpulsoSolicitado = -1;

            InvalidateOrbitalValues();
        }

        public void InvalidateOrbitalValues()
        {
            Apoapsis = -1;
            Periapsis = -1;
            Inclinacion = -1;
            OrbitaSubiendo = null;
        }

        public void AsignarConstantes(Constantes constantes)
        {
            if (this.constantes != null)
                throw new ArgumentException("Constantes ya asignadas");

            this.constantes = constantes;
        }
    }
}