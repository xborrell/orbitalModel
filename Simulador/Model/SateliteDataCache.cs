using System.Collections;
using System;
using satelite.interfaces;

namespace Xb.Simulador.Model
{
    public class SateliteDataCache
    {
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

        public SateliteDataCache(ISateliteData data)
        {
            this.Velocidad = data.Velocidad;
            this.Posicion = data.Posicion;
            this.Orientacion = data.Orientacion;
            this.Camara = data.Camara;
            this.Actitud = data.Actitud;
            this.ActitudSolicitada = data.ActitudSolicitada;
            this.Impulso = data.Impulso;
            this.ImpulsoSolicitado = data.ImpulsoSolicitado;
            this.AlturaDeReferencia = data.AlturaDeReferencia;
            this.Apoapsis = data.Apoapsis;
            this.Periapsis = data.Periapsis;
            this.Inclinacion = data.Inclinacion;
            this.OrbitaSubiendo = data.OrbitaSubiendo;
        }

        public void SetValues(ISateliteData data)
        {
            data.Velocidad = this.Velocidad;
            data.Posicion = this.Posicion;
            data.Orientacion = this.Orientacion;
            data.Camara = this.Camara;
            data.Actitud = this.Actitud;
            data.ActitudSolicitada = this.ActitudSolicitada;
            data.Impulso = this.Impulso;
            data.ImpulsoSolicitado = this.ImpulsoSolicitado;
            data.AlturaDeReferencia = this.AlturaDeReferencia;
            data.Apoapsis = this.Apoapsis;
            data.Periapsis = this.Periapsis;
            data.Inclinacion = this.Inclinacion;
            data.OrbitaSubiendo = this.OrbitaSubiendo;
        }
    }
}