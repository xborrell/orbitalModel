using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class CalculadorRotacion
    {
        readonly IToolsFactory factory;
        IManiobraRotacion maniobra = null;

        public CalculadorRotacion(IToolsFactory factory)
        {
            this.factory = factory;
        }

        public void CalcularNuevaRotacion(ISateliteData data)
        {
            if (data.ActitudSolicitada != ActitudRotacion.Ninguna)
            {
                GestionarCambioDeRotacion(data);
            }

            if ((maniobra != null) && (maniobra.ManiobraCompletada))
            {
                data.Actitud = maniobra.SiguienteActitud;
                maniobra = null;
            }

            switch (data.Actitud)
            {
                case ActitudRotacion.CaidaLibre: break;
                case ActitudRotacion.Maniobrando: data.Orientacion = maniobra.CalcularNuevaOrientacion(); break;
                case ActitudRotacion.EnfocadoATierra: data.Orientacion = CalcularOrientacionATierra(data); break;
                case ActitudRotacion.Orbital: data.Orientacion = CalcularOrientacionOrbital(data); break;
                default: throw new ArgumentException("Actitud no implementada en CalculadorRotacion2");
            }
        }

        private void GestionarCambioDeRotacion(ISateliteData data)
        {
            if (data.ActitudSolicitada != data.Actitud)
            {
                switch (data.ActitudSolicitada)
                {
                    case ActitudRotacion.CaidaLibre: RotacionLibre(data); break;
                    case ActitudRotacion.EnfocadoATierra: OrientacionATierra(data); break;
                    case ActitudRotacion.Orbital: OrientacionOrbital(data); break;
                }
            }
            data.ActitudSolicitada = ActitudRotacion.Ninguna;
        }

        private void RotacionLibre(ISateliteData data)
        {
            data.Actitud = ActitudRotacion.CaidaLibre;
        }

        private void OrientacionOrbital(ISateliteData data)
        {
            data.Actitud = ActitudRotacion.Maniobrando;
            Vector orientacion = CalcularOrientacionOrbital(data);

            maniobra = factory.CreateManiobra(ActitudRotacion.Orbital, data, orientacion);
        }

        private void OrientacionATierra(ISateliteData data)
        {
            data.Actitud = ActitudRotacion.Maniobrando;
            Vector orientacion = CalcularOrientacionATierra(data);

            maniobra = factory.CreateManiobra(ActitudRotacion.EnfocadoATierra, data, orientacion);
        }

        Vector CalcularOrientacionATierra(ISateliteData data)
        {
            Vector forward = data.Posicion.Clone();
            forward.Normalize();
            forward *= -1;

            return forward;
        }

        Vector CalcularOrientacionOrbital(ISateliteData data)
        {

            Vector forward = data.Velocidad.Clone();
            forward.Normalize();

            return forward;
        }
    }
}