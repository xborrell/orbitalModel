using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xb.Simulador.Model;
using Xb.Simulador.Tools;

namespace Xb.Simulador.Model
{
    public class TimeManager : ObservableObject
    {
        public TimeIntervals FactorTemporal
        {
            get { return factorTemporal; }
            protected set
            {
                if (factorTemporal != value)
                {
                    factorTemporal = value;
                    RaisePropertyChanged("FactorTemporal");
                }
            }
        }
        TimeIntervals factorTemporal = TimeIntervals.Cero;

        SimulatorLoop model;

        public TimeManager(SimulatorLoop model)
        {
            this.model = model;
            SincronizarModelo();
        }

        public bool CanSlowTime()
        {
            return (FactorTemporal != TimeIntervals.Menos1000);
        }

        public void SlowTime()
        {
            if (CanSlowTime())
            {
                FactorTemporal = FactorTemporal - 1;
                SincronizarModelo();
            }
        }

        public bool CanFastTime()
        {
            return (FactorTemporal != TimeIntervals.Mas1000);
        }

        public void FastTime()
        {
            if (CanFastTime())
            {
                FactorTemporal = FactorTemporal + 1;
                SincronizarModelo();
            }
        }

        public void StopTime()
        {
            if (FactorTemporal != TimeIntervals.Cero)
            {
                FactorTemporal = TimeIntervals.Cero;
                SincronizarModelo();
            }
        }

        void SincronizarModelo()
        {
            switch (FactorTemporal)
            {
                case TimeIntervals.Menos1000: model.AcceleratedTime = -1000; break;
                case TimeIntervals.Menos500: model.AcceleratedTime = -500; break;
                case TimeIntervals.Menos100: model.AcceleratedTime = -100; break;
                case TimeIntervals.Menos50: model.AcceleratedTime = -50; break;
                case TimeIntervals.Menos10: model.AcceleratedTime = -10; break;
                case TimeIntervals.Menos5: model.AcceleratedTime = -5; break;
                case TimeIntervals.Menos2: model.AcceleratedTime = -2; break;
                case TimeIntervals.Menos1: model.AcceleratedTime = -1; break;
                case TimeIntervals.MenosMedio: model.AcceleratedTime = -0.5; break;
                case TimeIntervals.MenosCuarto: model.AcceleratedTime = -0.25; break;
                case TimeIntervals.Cero: model.AcceleratedTime = 0; break;
                case TimeIntervals.MasCuarto: model.AcceleratedTime = 0.25; break;
                case TimeIntervals.MasMedio: model.AcceleratedTime = 0.5; break;
                case TimeIntervals.Mas1: model.AcceleratedTime = 1; break;
                case TimeIntervals.Mas2: model.AcceleratedTime = 2; break;
                case TimeIntervals.Mas5: model.AcceleratedTime = 5; break;
                case TimeIntervals.Mas10: model.AcceleratedTime = 10; break;
                case TimeIntervals.Mas50: model.AcceleratedTime = 50; break;
                case TimeIntervals.Mas100: model.AcceleratedTime = 100; break;
                case TimeIntervals.Mas500: model.AcceleratedTime = 500; break;
                case TimeIntervals.Mas1000: model.AcceleratedTime = 1000; break;
                default: throw new ArgumentException();
            }
        }
    }
}
