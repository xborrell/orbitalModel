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

namespace Xb.Simulador.Viewmodel
{
    public enum TimeIntervals
    {
        Pause,
        X1_4,
        X1_2,
        X1,
        X2,
        X5,
        X10,
        X50,
        X100,
        X500,
        X1000,
    }

    public class MainWindowViewModel : ObservableObject
    {
        public string AlturaReal { get { return model.Satelite.Data.Posicion.Magnitude.ToString("0.000 km."); } }
        public string VelocidadReal { get { return model.Satelite.Data.Velocidad.Magnitude.ToString("0.000 km/s"); } }

        public string Altura { get { return ValorOrbital( model.Satelite.Altura, "0.000"); } }
        public string Apoapsis { get { return ValorOrbital( model.Satelite.Apoapsis, "0.000"); } }
        public string Periapsis { get { return ValorOrbital( model.Satelite.Periapsis, "0.000"); } }
        public string Inclinacion { get { return ValorOrbital( model.Satelite.Inclinacion, "0.00"); } }
        public string Accion { get { return model.Satelite.Accion; } }
        public string Actitud { get { return model.Satelite.Actitud; } }

        public string EllapsedTime { get { return model.EllapsedTime.ToString(@"dd\.hh\:mm\:ss"); } }
        public string AcceleratedTime
        {
            get
            {
                switch (interval)
                {
                    case TimeIntervals.Pause: return "Pause";
                    case TimeIntervals.X1_4 : return "X 1/4";
                    case TimeIntervals.X1_2 : return "X 1/2";
                    case TimeIntervals.X1   : return "X 1";
                    case TimeIntervals.X2   : return "X 2";
                    case TimeIntervals.X5   : return "X 5";
                    case TimeIntervals.X10  : return "X 10";
                    case TimeIntervals.X50  : return "X 50";
                    case TimeIntervals.X100 : return "X 100";
                    case TimeIntervals.X500 : return "X 500";
                    case TimeIntervals.X1000: return "X 1000";
                    default: throw new ArgumentException();
                }
            }
        }
        TimeIntervals interval = TimeIntervals.X1;

        public ObservableCollection<string> Acciones { get; protected set; }
        public ObservableCollection<string> Actitudes { get; protected set; }
        string accionGuardada = string.Empty;
        string actitudGuardada = string.Empty;

        SimulatorLoop model;
        Timer timer;
        object obj = new object();
        private readonly SynchronizationContext uiContext;

        public MainWindowViewModel(SimulatorLoop model)
        {
            Acciones = new ObservableCollection<string>();
            Actitudes = new ObservableCollection<string>();
            uiContext = SynchronizationContext.Current;

            timer = new Timer(timer_Elapsed, null, 2000, 300);
            this.model = model;

            model.Start();
        }

        void timer_Elapsed(object stateInfo)
        {
            if( Monitor.TryEnter( obj ) )
            {
                uiContext.Post(x => refreshValues(), null);

                Monitor.Exit(obj);
            }
        }

        void refreshValues()
        {
            RaisePropertyChanged("EllapsedTime");
            RaisePropertyChanged("EllapsedTime");
            RaisePropertyChanged("AlturaReal");
            RaisePropertyChanged("VelocidadReal");
            RaisePropertyChanged("Altura");
            RaisePropertyChanged("Apoapsis");
            RaisePropertyChanged("Periapsis");
            RaisePropertyChanged("Inclinacion");
            RaisePropertyChanged("Accion");
            RaisePropertyChanged("Actitud");

            if ((accionGuardada != Accion) && (!string.IsNullOrEmpty(Accion)))
            {
                string msg = string.Format("[{1}] {0}", Accion, EllapsedTime);
                Acciones.Insert(0, msg);
                accionGuardada = Accion;
            }

            if ((actitudGuardada != Actitud) && (!string.IsNullOrEmpty(Actitud)))
            {
                string msg = string.Format("[{1}] {0}", Actitud, EllapsedTime);
                Actitudes.Insert(0, msg);
                actitudGuardada = Actitud;
            }
        }

        private string ValorOrbital(float valor, string mascara)
        {
            return valor < 0 ? "???" : valor.ToString(mascara);
        }

        #region Slow Time Command
        /// <summary>
        /// Torna el comandament que obre un nou event
        /// </summary>
        public ICommand SlowTimeCommand
        {
            get
            {
                if (_slowTimeCommand == null)
                    _slowTimeCommand = new RelayCommand(() => this.SlowTime(), () => interval != TimeIntervals.Pause);

                return _slowTimeCommand;
            }
        }
        RelayCommand _slowTimeCommand;

        void SlowTime()
        {
            switch (interval)
            {
                case TimeIntervals.X1_4: 
                    interval = TimeIntervals.Pause;
                    model.AcceleratedTime = 0;
                    break;
                case TimeIntervals.X1_2:
                    interval = TimeIntervals.X1_4;
                    model.AcceleratedTime = 0.25;
                    break;
                case TimeIntervals.X1:
                    interval = TimeIntervals.X1_2;
                    model.AcceleratedTime = 0.5;
                    break;
                case TimeIntervals.X2:
                    interval = TimeIntervals.X1;
                    model.AcceleratedTime = 1;
                    break;
                case TimeIntervals.X5:
                    interval = TimeIntervals.X2;
                    model.AcceleratedTime = 2;
                    break;
                case TimeIntervals.X10:
                    interval = TimeIntervals.X5;
                    model.AcceleratedTime = 5;
                    break;
                case TimeIntervals.X50:
                    interval = TimeIntervals.X10;
                    model.AcceleratedTime = 10;
                    break;
                case TimeIntervals.X100:
                    interval = TimeIntervals.X50;
                    model.AcceleratedTime = 50;
                    break;
                case TimeIntervals.X500:
                    interval = TimeIntervals.X100;
                    model.AcceleratedTime = 100;
                    break;
                case TimeIntervals.X1000:
                    interval = TimeIntervals.X500;
                    model.AcceleratedTime = 500;
                    break;
            }

            RaisePropertyChanged("AcceleratedTime");
        }
        #endregion

        #region Fast Time Command
        /// <summary>
        /// Torna el comandament que obre un nou event
        /// </summary>
        public ICommand FastTimeCommand
        {
            get
            {
                if (_fastTimeCommand == null)
                    _fastTimeCommand = new RelayCommand(() => this.FastTime(), () => interval != TimeIntervals.X1000);

                return _fastTimeCommand;
            }
        }
        RelayCommand _fastTimeCommand;

        void FastTime()
        {
            switch (interval)
            {
                case TimeIntervals.Pause:
                    interval = TimeIntervals.X1_4;
                    model.AcceleratedTime = 0.25;
                    break;
                case TimeIntervals.X1_4:
                    interval = TimeIntervals.X1_2;
                    model.AcceleratedTime = 0.5;
                    break;
                case TimeIntervals.X1_2:
                    interval = TimeIntervals.X1;
                    model.AcceleratedTime = 1;
                    break;
                case TimeIntervals.X1:
                    interval = TimeIntervals.X2;
                    model.AcceleratedTime = 2;
                    break;
                case TimeIntervals.X2:
                    interval = TimeIntervals.X5;
                    model.AcceleratedTime = 5;
                    break;
                case TimeIntervals.X5:
                    interval = TimeIntervals.X10;
                    model.AcceleratedTime = 10;
                    break;
                case TimeIntervals.X10:
                    interval = TimeIntervals.X50;
                    model.AcceleratedTime = 50;
                    break;
                case TimeIntervals.X50:
                    interval = TimeIntervals.X100;
                    model.AcceleratedTime = 100;
                    break;
                case TimeIntervals.X100:
                    interval = TimeIntervals.X500;
                    model.AcceleratedTime = 500;
                    break;
                case TimeIntervals.X500:
                    interval = TimeIntervals.X1000;
                    model.AcceleratedTime = 1000;
                    break;
            }

            RaisePropertyChanged("AcceleratedTime");
        }
        #endregion
    }
}
