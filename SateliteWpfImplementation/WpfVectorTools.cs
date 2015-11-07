using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using satelite.backend;
using satelite.interfaces;

namespace satelite.implementation.wpf
{
    public class WpfVectorTools : IVectorTools
    {
        IToolsFactory factory;

        public WpfVectorTools(IToolsFactory factory)
        {
            this.factory = factory;
        }

        public float AngleTo(Vector orientacionInicial, Vector orientacionFinal)
        {
            var v1 = new Vector3D(orientacionInicial.X, orientacionInicial.Y, orientacionInicial.Z);
            var v2 = new Vector3D(orientacionFinal.X, orientacionFinal.Y, orientacionFinal.Z);

            return (float)Vector3D.AngleBetween(v1, v2);
        }

        public Vector Lerp(Vector first, Vector second, float porcentajeDeRotacion)
        {
            return first + ((second - first) * porcentajeDeRotacion);
        }

        public Vector CrossProduct(Vector first, Vector second)
        {
            var v1 = new Vector3D(first.X, first.Y, first.Z);
            var v2 = new Vector3D(second.X, second.Y, second.Z);

            var v3 = Vector3D.CrossProduct(v1, v2);
            return factory.CreateVector((float)v3.X, (float)v3.Y, (float)v3.Z);
        }

        public float DotProduct(Vector first, Vector second)
        {
            var v1 = new Vector3D(first.X, first.Y, first.Z);
            var v2 = new Vector3D(second.X, second.Y, second.Z);

            return (float)Vector3D.DotProduct(v1, v2);
        }
    }
}
