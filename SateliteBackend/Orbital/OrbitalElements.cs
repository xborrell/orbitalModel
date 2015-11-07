using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace satelite.backend.orbital
{
    public class OrbitalElements
    {
        Constantes constantes;

        public float MomentoAngular { get; protected set; }
        public float Excentricity { get; protected set; }
        public float Inclination { get; protected set; }
        public float ArgumentOfPeriapsis { get; protected set; }
        public float AngleOfAscendingNode { get; protected set; }
        public float TrueAnomaly { get; protected set; }

        public float SemiejeMayor { get; protected set; }
        public float Periapsis { get; protected set; }
        public float Apoapsis { get; protected set; }
        public TimeSpan Period { get; protected set; }

        public OrbitalElements(Constantes constantes, float momentoAngular, float excentricity, float inclination, float angleOfAscendingNode, float argumentOfPeriapsis, float trueAnomaly)
        {
            this.constantes = constantes;

            MomentoAngular = momentoAngular;
            Excentricity = excentricity;
            Inclination = inclination;
            ArgumentOfPeriapsis = argumentOfPeriapsis;
            AngleOfAscendingNode = angleOfAscendingNode;
            TrueAnomaly = trueAnomaly;

            CalcularValoresDerivados();
        }

        private void CalcularValoresDerivados()
        {
            Periapsis = ((float)Math.Pow(MomentoAngular, 2) / constantes.Mu) * (1 / (1 + Excentricity));

            if (Excentricity < 1) // elipse
            {
                Apoapsis = ((float)Math.Pow(MomentoAngular, 2) / constantes.Mu) * (1 / (1 - Excentricity));
                SemiejeMayor = (Periapsis + Apoapsis) / 2;

                var segundos = constantes.Pi2 * (float)Math.Sqrt(Math.Pow(SemiejeMayor, 3) / constantes.Mu);
                Period = TimeSpan.FromSeconds(segundos);
            }
            else
            {
                Apoapsis = float.PositiveInfinity;
                SemiejeMayor = float.PositiveInfinity;
                Period = TimeSpan.MaxValue;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("( h, e, i, \u03C9, \u03A9, \u03B8 )" );
            sb.AppendFormat("( {0}, {1}, {2}, {3}, {4}, {5} )",
                MomentoAngular, 
                Excentricity, 
                Inclination, 
                AngleOfAscendingNode, 
                ArgumentOfPeriapsis, 
                TrueAnomaly);

            return sb.ToString();
        }

        public string FullDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Momento Angular         (h) = {0} Km", MomentoAngular); sb.AppendLine();
            sb.AppendFormat("Excentricity            (e) = {0}", Excentricity); sb.AppendLine();
            sb.AppendFormat("Inclination             (i) = {0} rad.", Inclination); sb.AppendLine();
            sb.AppendFormat("Argument of periapsis   (\u03A9) = {0} rad.", ArgumentOfPeriapsis); sb.AppendLine();
            sb.AppendFormat("Angle of ascending Node (\u03C9) = {0} rad.", AngleOfAscendingNode); sb.AppendLine();
            sb.AppendFormat("True Anomaly            (\u03B8) = {0} rad.", TrueAnomaly); sb.AppendLine();
            sb.AppendFormat("Semieje Mayor           (a) = {0} Km.", SemiejeMayor); sb.AppendLine();
            sb.AppendFormat("Periapsis                   = {0} Km.", Periapsis); sb.AppendLine();
            sb.AppendFormat("Apoapsis                    = {0} Km.", Apoapsis); sb.AppendLine();
            sb.AppendFormat("Orbital Period          (T) = {0} min.", Period.TotalMinutes ); sb.AppendLine();

            return sb.ToString();
        }
    }
}
