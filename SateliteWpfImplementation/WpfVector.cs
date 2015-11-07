using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;
using satelite.interfaces;

namespace satelite.implementation.wpf
{
    public class WpfVector : Vector
    {
        Vector3D internalVector;

        public WpfVector(float x, float y, float z)
        {
            internalVector = new Vector3D(x, y, z);
        }

        public override float X
        {
            get
            {
                return (float)internalVector.X;
            }
            set
            {
                internalVector.X = value;
            }
        }

        public override float Y
        {
            get
            {
                return (float)internalVector.Y;
            }
            set
            {
                internalVector.Y = value;
            }
        }

        public override float Z
        {
            get
            {
                return (float)internalVector.Z;
            }
            set
            {
                internalVector.Z = value;
            }
        }

        public override float Magnitude
        {
            get
            {
                return (float)internalVector.Length;
            }
            set
            {
                Normalize();
                internalVector *= value;
            }
        }

        public override void Normalize()
        {
            internalVector.Normalize();
        }

        public override Vector Clone()
        {
            return new WpfVector(X, Y, Z);
        }
    }
}
