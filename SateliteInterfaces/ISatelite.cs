using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.interfaces
{
    public interface ISatelite
    {
        float Altura { get; }
        float Apoapsis { get; }
        float Periapsis { get; }
        float Inclinacion { get; }
        ISateliteData Data { get; }
        IMenteSatelite Mente { get; }
        string Accion { get; }
        string Actitud { get; }

        void Pulse();
    }
}