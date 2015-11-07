using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.interfaces;

namespace satelite.backend.orbital
{
    public class OrbitalState : IEquatable<OrbitalState>
    {
        public readonly Vector Posicion;
        public readonly Vector Velocidad;

        public OrbitalState(Vector posicion, Vector velocidad)
        {
            Posicion = posicion;
            Velocidad = velocidad;
        }

        public override string ToString()
        {
            return string.Format("Posicion {0}, Velocidad {1}", Posicion, Velocidad);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OrbitalState);
        }

        public bool Equals(OrbitalState other)
        {
            return Posicion == other.Posicion && Velocidad == other.Velocidad;
        }

        public override int GetHashCode()
        {
            return Posicion.GetHashCode() + Velocidad.GetHashCode();
        }
    }
}
