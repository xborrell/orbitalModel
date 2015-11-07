using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class CalculadorRotacion
    {
        IToolsFactory factory;
        IManiobraRotacion maniobra = null;

        protected ISateliteData data;

        public CalculadorRotacion(IToolsFactory factory, ISateliteData sateliteData)
        {
            this.factory = factory;
            this.data = sateliteData;
        }

        public void CalcularNuevaRotacion()
        {
            if (data.ActitudSolicitada != ActitudRotacion.Ninguna)
            {
                GestionarCambioDeRotacion();
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
                case ActitudRotacion.EnfocadoATierra: data.Orientacion = CalcularOrientacionATierra(); break;
                case ActitudRotacion.Orbital: data.Orientacion = CalcularOrientacionOrbital(); break;
                default: throw new ArgumentException("Actitud no implementada en CalculadorRotacion2");
            }
        }

        private void GestionarCambioDeRotacion()
        {
            if (data.ActitudSolicitada != data.Actitud)
            {
                switch (data.ActitudSolicitada)
                {
                    case ActitudRotacion.CaidaLibre: RotacionLibre(); break;
                    case ActitudRotacion.EnfocadoATierra: OrientacionATierra(); break;
                    case ActitudRotacion.Orbital: OrientacionOrbital(); break;
                }
            }
            data.ActitudSolicitada = ActitudRotacion.Ninguna;
        }

        private void RotacionLibre()
        {
            data.Actitud = ActitudRotacion.CaidaLibre;
        }

        private void OrientacionOrbital()
        {
            data.Actitud = ActitudRotacion.Maniobrando;
            Vector orientacion = CalcularOrientacionOrbital();

            maniobra = factory.CreateManiobra(ActitudRotacion.Orbital, data, orientacion);
        }

        private void OrientacionATierra()
        {
            data.Actitud = ActitudRotacion.Maniobrando;
            Vector orientacion = CalcularOrientacionATierra();

            maniobra = factory.CreateManiobra(ActitudRotacion.EnfocadoATierra, data, orientacion);
        }

        Vector CalcularOrientacionATierra()
        {
            Vector forward = data.Posicion;
            forward.Normalize();
            forward *= -1;

            return forward;
        }

        Vector CalcularOrientacionOrbital()
        {

            Vector forward = data.Velocidad;
            forward.Normalize();

            return forward;
        }
    }
}