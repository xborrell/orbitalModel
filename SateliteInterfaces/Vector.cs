using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace satelite.interfaces
{
    public abstract class Vector
    {
        public abstract float X { get; set; }
        public abstract float Y { get; set; }
        public abstract float Z { get; set; }
        public abstract float Magnitude { get; set; }
        public abstract void Normalize();
        public abstract Vector Clone();

        #region Operadores
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector resultat = v1.Clone();
            resultat.X += v2.X;
            resultat.Y += v2.Y;
            resultat.Z += v2.Z;

            return resultat;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector resultat = v1.Clone();
            resultat.X -= v2.X;
            resultat.Y -= v2.Y;
            resultat.Z -= v2.Z;

            return resultat;
        }

        public static Vector operator *(int valor, Vector vector)
        {
            return vector * valor;
        }

        public static Vector operator *(Vector vector, int valor)
        {
            Vector resultat = vector.Clone();
            resultat.X *= valor;
            resultat.Y *= valor;
            resultat.Z *= valor;

            return resultat;
        }

        public static Vector operator *(float valor, Vector vector)
        {
            return vector * valor;
        }

        public static Vector operator *(Vector vector, float valor)
        {
            Vector resultat = vector.Clone();
            resultat.X *= valor;
            resultat.Y *= valor;
            resultat.Z *= valor;

            return resultat;
        }

        public static Vector operator *(double valor, Vector vector)
        {
            return vector * valor;
        }

        public static Vector operator *(Vector vector, double valor)
        {
            Vector resultat = vector.Clone();
            resultat.X *= (float)valor;
            resultat.Y *= (float)valor;
            resultat.Z *= (float)valor;

            return resultat;
        }

        public static Vector operator /(int valor, Vector vector)
        {
            return vector / valor;
        }

        public static Vector operator /(Vector vector, int valor)
        {
            Vector resultat = vector.Clone();
            resultat.X /= valor;
            resultat.Y /= valor;
            resultat.Z /= valor;

            return resultat;
        }

        public static Vector operator /(float valor, Vector vector)
        {
            return vector / valor;
        }

        public static Vector operator /(Vector vector, float valor)
        {
            Vector resultat = vector.Clone();
            resultat.X /= valor;
            resultat.Y /= valor;
            resultat.Z /= valor;

            return resultat;
        }

        public static Vector operator /(double valor, Vector vector)
        {
            return vector / valor;
        }

        public static Vector operator /(Vector vector, double valor)
        {
            Vector resultat = vector.Clone();
            resultat.X /= (float)valor;
            resultat.Y /= (float)valor;
            resultat.Z /= (float)valor;

            return resultat;
        }
        #endregion
    }
}
