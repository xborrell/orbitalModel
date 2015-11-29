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
        private TimeSpan intervalo = new TimeSpan(0, 0, 0, 0, 100);
        private TimeSpan pageSize = new TimeSpan(0, 0, 5);
        Task task;

        public TimeSpan ActualTime
        {
            get { return actualTime; }
            protected set
            {
                if (actualTime != value)
                {
                    actualTime = value;
                    RaisePropertyChanged("ActualTime");
                }
            }
        }
        TimeSpan actualTime = new TimeSpan();

        public double AcceleratedTime { get; set; }

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
        TimeIntervals factorTemporal = TimeIntervals.X1;

        public bool IsPlaying
        {
            get { return isPlaying; }
            protected set
            {
                if (isPlaying != value)
                {
                    isPlaying = value;
                    RaisePropertyChanged("IsPlaying");
                }
            }
        }
        bool isPlaying = false;
        public SateliteDataList Model { get; set; }

        public TimeManager()
        {
            AjustarAceleracionTemporal();
        }

        #region Play
        public bool CanPlay()
        {
            return !IsPlaying;
        }

        public void Play()
        {
            if (CanPlay())
            {
                IsPlaying = true;

                task = Task.Run((Action)internalPlay);
            }
        }

        void internalPlay()
        {
            DateTime startTime = DateTime.Now;

            while (IsPlaying)
            {
                DateTime checkTime = DateTime.Now;

                double segundosDeIntervaloCorregidos = intervalo.TotalSeconds / AcceleratedTime;

                TimeSpan intervaloCorregido = TimeSpan.FromSeconds(segundosDeIntervaloCorregidos);

                if (checkTime > startTime + intervaloCorregido)
                {
                    startTime = DateTime.Now;
                    TiempoSolicitado(ActualTime + intervalo);
                }
            }
        }
        #endregion

        #region Pause
        public bool CanPause()
        {
            return IsPlaying;
        }

        public void Pause()
        {
            if (CanPause())
            {
                IsPlaying = false;
            }
        }
        #endregion

        #region Slow Time
        public bool CanSlowTime()
        {
            return (FactorTemporal != TimeIntervals.UnDecimo);
        }

        public void SlowTime()
        {
            if (CanSlowTime())
            {
                FactorTemporal = FactorTemporal - 1;
                AjustarAceleracionTemporal();
            }
        }
        #endregion

        #region Fast Time
        public bool CanFastTime()
        {
            return (FactorTemporal != TimeIntervals.X1000);
        }

        public void FastTime()
        {
            if (CanFastTime())
            {
                FactorTemporal = FactorTemporal + 1;
                AjustarAceleracionTemporal();
            }
        }
        #endregion

        #region Next Step
        public bool CanNextStep()
        {
            return true;
        }

        public void NextStep()
        {
            TiempoSolicitado(ActualTime.Add(intervalo));
        }
        #endregion

        #region Back Step
        public bool CanBackStep()
        {
            return intervalo <= ActualTime;
        }

        public void BackStep()
        {
            if (CanBackStep())
            {
                TiempoSolicitado(ActualTime.Subtract(intervalo));
            }
        }
        #endregion

        #region Back Page
        public bool CanBackPage()
        {
            return CanBackStep();
        }

        public void BackPage()
        {
            if (CanBackPage())
            {
                TimeSpan time = ActualTime.Subtract(pageSize);
                if (time.TotalMilliseconds < 0)
                {
                    time = new TimeSpan();
                }
                TiempoSolicitado(time);
            }
        }
        #endregion

        #region Next Page
        public bool CanNextPage()
        {
            return true;
        }

        public void NextPage()
        {
            if (CanNextPage())
            {
                TiempoSolicitado(ActualTime.Add(pageSize));
            }
        }
        #endregion

        void TiempoSolicitado(TimeSpan tiempoSolicitado)
        {
            ActualTime = tiempoSolicitado;
            Model.GoToTime(ActualTime);
        }

        void AjustarAceleracionTemporal()
        {
            switch (FactorTemporal)
            {
                case TimeIntervals.UnDecimo: AcceleratedTime = 0.1; break;
                case TimeIntervals.UnCuarto: AcceleratedTime = 0.25; break;
                case TimeIntervals.XUnMedio: AcceleratedTime = 0.5; break;
                case TimeIntervals.X1: AcceleratedTime = 1; break;
                case TimeIntervals.X2: AcceleratedTime = 2; break;
                case TimeIntervals.X5: AcceleratedTime = 5; break;
                case TimeIntervals.X10: AcceleratedTime = 10; break;
                case TimeIntervals.X50: AcceleratedTime = 50; break;
                case TimeIntervals.X100: AcceleratedTime = 100; break;
                case TimeIntervals.X500: AcceleratedTime = 500; break;
                case TimeIntervals.X1000: AcceleratedTime = 1000; break;
                default: throw new ArgumentException();
            }
        }
    }
}
