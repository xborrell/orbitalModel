using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class Satelite : ISatelite
    {
        public ISateliteData Data { get; protected set; }
        public IMenteSatelite Mente { get; protected set; }
        public IMotorSatelite Motor { get; protected set; }

        public string Nombre;
        public Vector PosicionInicial;
        public Vector VelocidadInicial;

        CalculadorMovimiento calculadorMovimiento;
        CalculadorRotacion calculadorRotacion;

        public float Altura { get { return Data.Altura; } }
        public float Apoapsis { get { return Data.Apoapsis; } }
        public float Periapsis { get { return Data.Periapsis; } }
        public float Inclinacion { get { return Data.Inclinacion; } }

        public string Accion
        {
            get
            {
                return Mente.DecisionEnCurso == null ? "Pensando" : Mente.DecisionEnCurso.LogData.Titulo;
            }
        }

        public string Actitud
        {
            get
            {
                switch (Data.Actitud)
                {
                    case ActitudRotacion.CaidaLibre: return "Caida Libre";
                    case ActitudRotacion.EnfocadoATierra: return "A Tierra";
                    case ActitudRotacion.Orbital: return "Orbital";
                    case ActitudRotacion.Maniobrando: return "Maniobrando";
                    default: throw new ArgumentException("Actitud de rotacion desconocida");
                }
            }
        }

        public Satelite(
            IVectorTools vectorTools,
            ISateliteData sateliteData,
            CalculadorRotacion calculadorRotacion,
            CalculadorMovimiento calculadorMovimiento,
            IMenteSatelite mente,
            IMotorSatelite motor
            )
        {
            this.calculadorMovimiento = calculadorMovimiento;
            this.calculadorRotacion = calculadorRotacion;
            this.Mente = mente;
            this.Data = sateliteData;
            this.Motor = motor;
        }

        public void Pulse()
        {
            Mente.Pulse();
            Motor.CalcularImpulso();

            calculadorMovimiento.CalcularNuevaPosicion();
            calculadorRotacion.CalcularNuevaRotacion();
        }
    }
}