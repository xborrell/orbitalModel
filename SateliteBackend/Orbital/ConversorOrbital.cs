using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using satelite.backend;
using satelite.interfaces;

namespace satelite.backend.orbital
{
    public class ConversorOrbital
    {
        IVectorTools vectorTools;
        Constantes constantes;

        public ConversorOrbital(Constantes constantes, IVectorTools vectorTools)
        {
            this.vectorTools = vectorTools;
            this.constantes = constantes;
        }

        public OrbitalElements Convertir(Vector posicion, Vector velocidad)
        {
            float vr = (float)Math.Round((posicion.X * velocidad.X + posicion.Y * velocidad.Y + posicion.Z * velocidad.Z) / posicion.Magnitude, 4);
            Vector angularMomentum = vectorTools.CrossProduct( posicion, velocidad);
            float inclination = (float)Math.Acos(angularMomentum.Z / angularMomentum.Magnitude);
            Vector nodeLine = vectorTools.CrossProduct( constantes.XAxis, angularMomentum);
            float angleOfAscendingNode = NormalizarCuadrante(nodeLine.Y < 0, Math.Acos(nodeLine.X / nodeLine.Magnitude));
            Vector excentricityVector = (1 / constantes.Mu) * (posicion * (Math.Pow(velocidad.Magnitude, 2) - (constantes.Mu / posicion.Magnitude))) - (velocidad * posicion.Magnitude * vr);
            double excentricity = excentricityVector.Magnitude;
            float argumentOfPeriapsis = NormalizarCuadrante(excentricityVector.Z < 0, Math.Acos(vectorTools.DotProduct( nodeLine, excentricityVector) / (nodeLine.Magnitude * excentricity)));
            float trueAnomaly = NormalizarCuadrante(vr < 0, Math.Acos(vectorTools.DotProduct(excentricityVector, posicion) / (excentricity * posicion.Magnitude)));

            return new OrbitalElements(constantes, (float)angularMomentum.Magnitude, (float)excentricity, inclination, angleOfAscendingNode, argumentOfPeriapsis, trueAnomaly);
        }

        public OrbitalElements Convertir(OrbitalState elementos)
        {
            return Convertir(elementos.Posicion, elementos.Velocidad);
        }


        #region Conversion a Orbital State

        //public OrbitalState Convertir(OrbitalElements elementos)
        //{
        //    //1. Calculate position vector { } r x in perifocal coordinates using Equation 4.45.
        //    var x = (Math.Pow(elementos.MomentoAngular, 2) / Constantes.Mu) * (1 / (1 + (elementos.Excentricity * Math.Cos(elementos.TrueAnomaly))));
        //    Vector3 p = new Vector3((float)(x * Math.Cos(elementos.TrueAnomaly)), (float)(x * Math.Sin(elementos.TrueAnomaly)), 0F);

        //    //2. Calculate velocity vector { } v x in perifocal coordinates using Equation 4.46.
        //    var x1 = Constantes.Mu / elementos.MomentoAngular;
        //    float vx = (float)(x1 * Math.Sin(elementos.TrueAnomaly) * -1);
        //    float vy = (float)(x1 * (elementos.Excentricity + Math.Cos(elementos.TrueAnomaly)));
        //    float vz = 0F;
        //    Vector3 v = new Vector3(vx, vy, vz);

        //    //3. Calculate the matrix [Q]xX of the transformation from perifocal to geocentric equatorial coordinates using Equation 4.49.
        //    var matrizX = new Matrix(3, 3);
        //    matrizX[0, 0] = (float)Math.Cos(elementos.ArgumentOfPeriapsis);
        //    matrizX[0, 1] = (float)Math.Sin(elementos.ArgumentOfPeriapsis);
        //    matrizX[0, 2] = 0;
        //    matrizX[1, 0] = -1F * (float)Math.Sin(elementos.ArgumentOfPeriapsis);
        //    matrizX[1, 1] = (float)Math.Cos(elementos.ArgumentOfPeriapsis);
        //    matrizX[1, 2] = 0;
        //    matrizX[2, 0] = 0;
        //    matrizX[2, 1] = 0;
        //    matrizX[2, 2] = 1;

        //    var matrizY = new Matrix(3, 3);
        //    matrizY[0, 0] = 1;
        //    matrizY[0, 1] = 0;
        //    matrizY[0, 2] = 0;
        //    matrizY[1, 0] = 0;
        //    matrizY[1, 1] = (float)Math.Cos(elementos.Inclination);
        //    matrizY[1, 2] = (float)Math.Sin(elementos.Inclination);
        //    matrizY[2, 0] = 0;
        //    matrizY[2, 1] = -1 * (float)Math.Sin(elementos.Inclination);
        //    matrizY[2, 2] = (float)Math.Cos(elementos.Inclination);

        //    var matrizZ = new Matrix(3, 3);
        //    matrizZ[0, 0] = (float)Math.Cos(elementos.AngleOfAscendingNode);
        //    matrizZ[0, 1] = (float)Math.Sin(elementos.AngleOfAscendingNode);
        //    matrizZ[0, 2] = 0;
        //    matrizZ[1, 0] = -1F * (float)Math.Sin(elementos.AngleOfAscendingNode);
        //    matrizZ[1, 1] = (float)Math.Cos(elementos.AngleOfAscendingNode);
        //    matrizZ[1, 2] = 0;
        //    matrizZ[2, 0] = 0;
        //    matrizZ[2, 1] = 0;
        //    matrizZ[2, 2] = 1;

        //    var directionCosineMatrix = matrizX * matrizY * matrizZ;
        //    var transformationMatrix = Matrix.Transpose(directionCosineMatrix);

        //    //4. Transform { } r x and { } v x into the geocentric frame by means of Equation 4.51.
        //    var matrizP = new Matrix(3, 1);
        //    matrizP[0, 0] = p.x;
        //    matrizP[1, 0] = p.y;
        //    matrizP[2, 0] = p.z;

        //    var p1 = transformationMatrix * matrizP;
        //    Vector3 p2 = new Vector3((float)p1[0, 0], (float)p1[1, 0], (float)p1[2, 0]);

        //    var matrizV = new Matrix(3, 1);
        //    matrizV[0, 0] = v.x;
        //    matrizV[1, 0] = v.y;
        //    matrizV[2, 0] = v.z;

        //    var v1 = transformationMatrix * matrizV;
        //    Vector3 v2 = new Vector3((float)v1[0, 0], (float)v1[1, 0], (float)v1[2, 0]);

        //    return new OrbitalState(p2, v2);
        //}

        ///// <summary>
        ///// Metodo de transformación modelado a partir del procedimiento encontrado en modelica.org, página 508
        ///// https://www.modelica.org/events/modelica2008/Proceedings/sessions/session4d1.pdf
        ///// </summary>
        ///// <param name="elementos"></param>
        ///// <returns></returns>
        //public OrbitalState Convertir2(OrbitalElements elementos)
        //{
        //    var a = elementos.SemiejeMayor;
        //    var e = elementos.Excentricity;
        //    var ta = elementos.TrueAnomaly;

        //    // Calcular elementos orbitales auxiliares
        //    var E = (a * (1 - Math.Pow(e, 2))) / (1 + (e * Math.Cos(ta))); // excentric anomaly

        //    var p1x = (a * Math.Cos(E)) - (a * e);
        //    var p1y = a * Math.Sin(E) * Math.Sqrt(1 - Math.Pow(e, 2));
        //    Vector3 p1 = new Vector3((float)p1x, (float)p1y, 0);




        //    //var n = Math.Sqrt(Constantes.Mu / Math.Pow(a, 3)); // mean motion


        //    //var r = Math.Sqrt(Math.Pow(p1.x, 2) + Math.Pow(p1.y, 2));

        //    //var vAux = Math.Pow(a, 2) * n / r;
        //    //var v1x = -vAux * Math.Sin(E);
        //    //var v1y = vAux * Math.Sqrt(1 - Math.Pow(e, 2)) * Math.Cos(E);

        //    //Vector3 v1 = new Vector3((float)p1x, (float)p1y, 0);






        //    //// Rotación sobre el eje Z del angulo AngleOfAscendingNode
        //    //var firstRotation = new Matrix(3, 3);
        //    //firstRotation[0, 0] = (float)Math.Cos(elementos.AngleOfAscendingNode);
        //    //firstRotation[0, 1] = (float)Math.Sin(elementos.AngleOfAscendingNode);
        //    //firstRotation[0, 2] = 0;
        //    //firstRotation[1, 0] = -1 * (float)Math.Sin(elementos.AngleOfAscendingNode);
        //    //firstRotation[1, 1] = (float)Math.Cos(elementos.AngleOfAscendingNode);
        //    //firstRotation[1, 2] = 0;
        //    //firstRotation[2, 0] = 0;
        //    //firstRotation[2, 1] = 0;
        //    //firstRotation[2, 2] = 1;

        //    //// rotacion sobre el eje X del angulo Inclination
        //    //var secondRotation = new Matrix(3, 3);
        //    //secondRotation[0, 0] = 1;
        //    //secondRotation[0, 1] = 0;
        //    //secondRotation[0, 2] = 0;
        //    //secondRotation[1, 0] = 0;
        //    //secondRotation[1, 1] = (float)Math.Cos(elementos.Inclination);
        //    //secondRotation[1, 2] = (float)Math.Sin(elementos.Inclination);
        //    //secondRotation[2, 0] = 0;
        //    //secondRotation[2, 1] = -1 * (float)Math.Sin(elementos.Inclination);
        //    //secondRotation[2, 2] = (float)Math.Cos(elementos.Inclination);

        //    //// rotacion sobre el eje Z del angulo ArgumentOfPeriapsis
        //    //var thirdRotation = new Matrix(3, 3);
        //    //thirdRotation[0, 0] = (float)Math.Cos(elementos.ArgumentOfPeriapsis);
        //    //thirdRotation[0, 1] = (float)Math.Sin(elementos.ArgumentOfPeriapsis);
        //    //thirdRotation[0, 2] = 0;
        //    //thirdRotation[1, 0] = -1F * (float)Math.Sin(elementos.ArgumentOfPeriapsis);
        //    //thirdRotation[1, 1] = (float)Math.Cos(elementos.ArgumentOfPeriapsis);
        //    //thirdRotation[1, 2] = 0;
        //    //thirdRotation[2, 0] = 0;
        //    //thirdRotation[2, 1] = 0;
        //    //thirdRotation[2, 2] = 1;

        //    //var rotationMatrix = firstRotation * secondRotation * thirdRotation;

        //    ////Rotate p1 to p with rotationMatrix
        //    //var matrizP = new Matrix(3, 1);
        //    //matrizP[0, 0] = p1.x;
        //    //matrizP[1, 0] = p1.y;
        //    //matrizP[2, 0] = p1.z;

        //    //var pMatrix = rotationMatrix * matrizP;
        //    //Vector3 p = new Vector3((float)matrizP[0, 0], (float)matrizP[1, 0], (float)matrizP[2, 0]);

        //    ////Rotate v1 to v with rotationMatrix
        //    //var matrizV = new Matrix(3, 1);
        //    //matrizV[0, 0] = v1.x;
        //    //matrizV[1, 0] = v1.y;
        //    //matrizV[2, 0] = v1.z;

        //    //var vMatrix = rotationMatrix * matrizV;
        //    //Vector3 v = new Vector3((float)matrizV[0, 0], (float)matrizV[1, 0], (float)matrizV[2, 0]);

        //    return new OrbitalState(p1, new Vector3());
        //}
        #endregion

        float NormalizarCuadrante(bool invertirCuadrante, double angulo)
        {
            return NormalizarCuadrante(invertirCuadrante, (float)angulo);
        }

        float NormalizarCuadrante(bool invertirCuadrante, float angulo)
        {
            return invertirCuadrante ? constantes.Pi2 - angulo : angulo;
        }
    }
}
