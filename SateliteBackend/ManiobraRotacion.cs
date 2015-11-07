using System.Collections;
using System;
using satelite.interfaces;

namespace satelite.backend
{
    public class ManiobraRotacion : IManiobraRotacion
    {
        IVectorTools vectorTools;

        public bool ManiobraCompletada { get; protected set; }
        public ActitudRotacion SiguienteActitud { get; protected set; }

        private const float velocidadAngularEnGradosPorSegundo = 10.0F;
        private float tiempoParaFinalizarEnSegundos;
        private float tiempoTranscurridoEnSegundos;
        Vector orientacionInicial;
        Vector orientacionFinal;
        Constantes constantes;

        public ManiobraRotacion(Constantes constantes, IVectorTools vectorTools, ActitudRotacion siguienteActitud, ISateliteData sateliteData, Vector orientacionSolicitada)
        {
            this.constantes = constantes;
            this.vectorTools = vectorTools;
            SiguienteActitud = siguienteActitud;

            orientacionInicial = sateliteData.Orientacion;
            orientacionFinal = orientacionSolicitada;

            float anguloEnGrados = vectorTools.AngleTo(orientacionInicial, orientacionFinal);

            tiempoParaFinalizarEnSegundos = anguloEnGrados / velocidadAngularEnGradosPorSegundo;
        }

        public Vector CalcularNuevaOrientacion()
        {
            Vector orientacionCalculada;

            if (!ManiobraCompletada)
            {
                tiempoTranscurridoEnSegundos += constantes.FixedDeltaTime;

                var porcentajeDeRotacion = Math.Min(tiempoTranscurridoEnSegundos / tiempoParaFinalizarEnSegundos, 1);

                orientacionCalculada = vectorTools.Lerp( orientacionInicial, orientacionFinal, porcentajeDeRotacion);

                ManiobraCompletada = porcentajeDeRotacion == 1;
            }
            else
            {
                orientacionCalculada = orientacionFinal;
            }

            return orientacionCalculada;
        }
    }
}