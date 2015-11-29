using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CalculadorOrbital;
using System.Windows.Media.Media3D;
using satelite.interfaces;
using satelite.backend;
using satelite.implementation.wpf;
using Autofac;

namespace Xb.Simulador.Model
{
    public class SateliteDataList
    {
        public ISatelite Satelite { get; protected set; }
        List<SateliteDataCache> cache = new List<SateliteDataCache>();

        Vector posicionInicial;
        Vector velocidadInicial;

        IComponentContext container;

        public SateliteDataList(IComponentContext c, IToolsFactory factory, Constantes constantes)
        {
            container = c;

            posicionInicial = factory.CreateVector(-822.79774F, -4438.63582F, 5049.31502F);
            velocidadInicial = factory.CreateVector(7.418175658F, .709253354F, 1.828703177F);

            Satelite = factory.CreateSatelite(posicionInicial, velocidadInicial);
            GuardarEnCache(0);
        }

        public void GoToTime(TimeSpan actualTime)
        {
            int index = GetIndex(actualTime);

            RedimensionarCache(index);

            GoToTime(index);
        }

        private void GoToTime(int index)
        {
            if (cache[index] != null)
            {
                cache[index].SetValues(Satelite.Data);
            }
            else
            {
                GoToTime(index - 1);
                Satelite.Pulse();
                GuardarEnCache(index);
            }
        }

        private void GuardarEnCache(int index)
        {
            RedimensionarCache(index);

            cache[index] = container.Resolve<SateliteDataCache>(
                new TypedParameter(typeof(ISateliteData), Satelite.Data)
            );
        }

        private int GetIndex(TimeSpan actualTime)
        {
            return (int)Math.Truncate(Math.Round(actualTime.TotalSeconds, 1) * 10);
        }

        private void RedimensionarCache(int index)
        {
            while (cache.Count <= index)
            {
                cache.Add(null);
            }
        }
    }
}
