using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend.decision.paso;
using satelite.backend.log;
using satelite.interfaces;
using satelite.interfaces.log;

namespace satelite.backend.decision
{
    public class Circularizar : Decision
    {
        const float margenDeCircularización = 0.1F;
        float impulsoNecesario;
        float duracionDelImpulsoEnSegundos;
        float nuevaVelocidad;

        public override bool DebeActuar(ISateliteData data)
        {
            return (data.Periapsis > 0)
                && (data.Apoapsis > 0)
                && (Math.Abs(data.Apoapsis - data.Periapsis) > margenDeCircularización);
        }

        public Circularizar(Constantes constantes, IVectorTools vectorTools, int prioridad)
            : base(constantes, vectorTools, prioridad)
        {
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Calcular impuls", "Calcular el valor del impuls."), CalcularValoresDelImpulso));
            DefinirPaso(new PasoEnfoqueATierra());
            DefinirPaso(new PasoComprobarEnfoque(ActitudRotacion.EnfocadoATierra));
            DefinirPaso(new PasoTomarAltura());
            DefinirPaso(new PasoEsperarPeriapsis());
            DefinirPaso(new PasoEsperarApoapsis());
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Cambiar velocitat"), CambiarVelocidad));

            //DefinirPaso(new Paso( "", EsperarPeriapsis));
            //DefinirPaso(new Paso( "", SolicitarEnfoqueOrbital));
            //DefinirPaso(new Paso( "", ComprobarEnfoqueCorrecto));
            //DefinirPaso(new Paso( "", EsperarMomentoDeIgnicion));
            //DefinirPaso(new Paso( "", EncenderMotor));
            //DefinirPaso(new Paso( "", EsperarDuracionDelImpulso));
            //DefinirPaso(new Paso( "", ApagarMotor));
            DefinirPaso(new PasoGenerico(new LogItem(LogType.Paso, "Resetejar", "Resetejar valors orbitals"), ResetearValoresOrbitales));

            LogData = new LogItem(LogType.Decision, "Orbita circular", "Fer l'orbita circular");
        }

        private bool CambiarVelocidad(ISateliteData data)
        {
            data.Velocidad.Normalize();
            data.Velocidad *= nuevaVelocidad;

            return true;
        }

        public override void Inicializar(ISateliteData data)
        {
            base.Inicializar(data);

            impulsoNecesario = -1;
        }

        bool CalcularValoresDelImpulso(ISateliteData data)
        {
            double radioPeriapsis = data.Periapsis + constantes.EarthRadius;
            double radioApoapsis = data.Apoapsis + constantes.EarthRadius;
            double compartido = Math.Sqrt(constantes.Mu * 2);
            double momentoAngularActual = compartido * Math.Sqrt((radioApoapsis * radioPeriapsis) / (radioApoapsis + radioPeriapsis));
            double momentoAngularDeseado = compartido * Math.Sqrt((radioApoapsis * radioApoapsis) / (radioApoapsis + radioApoapsis));
            double velocidadActualenApoapsis = momentoAngularActual / radioApoapsis;
            double velocidadDeseadaEnApoapsis = momentoAngularDeseado / radioApoapsis;

            nuevaVelocidad = (float)velocidadDeseadaEnApoapsis;

            impulsoNecesario = (float)(velocidadDeseadaEnApoapsis - velocidadActualenApoapsis);
            duracionDelImpulsoEnSegundos = impulsoNecesario / constantes.ImpulsoMaximo;

            if (duracionDelImpulsoEnSegundos > 5) duracionDelImpulsoEnSegundos = 5;

            return true;
        }

        //bool EsperarMomentoDeIgnicion(float deltaTime)
        //{
        //    float tiempoYaGastado = Time.time - marcaDeTiempo;
        //    float tiempoAntesDeApoapsisParaIgnicion = duracionDelImpulsoEnSegundos / 2;
        //    float tiempoAEsperar = duracionDeMediaOrbita - tiempoYaGastado - tiempoAntesDeApoapsisParaIgnicion;

        //    SolicitarEspera(tiempoAEsperar);

        //    return true;
        //}

        //bool EncenderMotor(float deltaTime)
        //{
        //    Data.ImpulsoSolicitado = Config.ImpulsoMaximo;
        //    return true;
        //}

        //bool EsperarDuracionDelImpulso(float deltaTime)
        //{
        //    SolicitarEspera(duracionDelImpulsoEnSegundos);
        //    return true;
        //}

        //bool ApagarMotor(float deltaTime)
        //{
        //    Data.ImpulsoSolicitado = 0;

        //    return true;
        //}

        bool ResetearValoresOrbitales(ISateliteData data)
        {
            data.InvalidateOrbitalValues();

            return true;
        }
    }
}