using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class Constantes
    {
        public float FixedDeltaTime { get { return 0.1F; } }                          // segundos
        public float Mu { get { return 398600F; } }                                  // Km3 / segundo2
        public float RadianToDegreeCoeficient { get { return 180 / (float)Math.PI; } }
        public float DegreeToRadianCoeficient { get { return (float)Math.PI / 180; } }
        public float Pi2 { get { return (float)(2 * Math.PI); } }
        public float ConstanteGravitacionUniversal { get { return 6.674E-11F; } }    //6,6722464010713090056913290927352e-20
        public float EarthRadius { get { return 6378.280F; } }                    // KM
        public float EarthMass { get { return 5.974E24F; } }                         // KG
        public float SatellitMass { get { return 750F; } }                          // KG
        public float ImpulsoMaximo { get { return .01F; } }                         // Km/s2
        public float VariacionMaximaDelImpulso { get { return .014F; } }              // Km/s2
        public Vector XAxis { get; protected set; }
        public Vector YAxis { get; protected set; }
        public Vector ZAxis { get; protected set; }
        public Vector Zero { get; protected set; }

        public Constantes(IToolsFactory factory)
        {
            XAxis = factory.CreateVector(1, 0, 0);
            YAxis = factory.CreateVector(0, 1, 0);
            ZAxis = factory.CreateVector(0, 0, 1);
            Zero = factory.CreateVector(0, 0, 0);
        }
    }
}
