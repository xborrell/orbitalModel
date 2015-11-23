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
    public class MainWindowViewModel : ObservableObject
    {
        public PlayerControlsViewModel ControlesViewModel { get; protected set; }

        public string AlturaReal { get { return model.Satelite.Data.Posicion.Magnitude.ToString("0.000 km."); } }
        public string VelocidadReal { get { return model.Satelite.Data.Velocidad.Magnitude.ToString("0.000 km/s"); } }

        public string Altura { get { return ValorOrbital(model.Satelite.Altura, "0.000"); } }
        public string Apoapsis { get { return ValorOrbital(model.Satelite.Apoapsis, "0.000"); } }
        public string Periapsis { get { return ValorOrbital(model.Satelite.Periapsis, "0.000"); } }
        public string Inclinacion { get { return ValorOrbital(model.Satelite.Inclinacion, "0.00"); } }
        public string Accion { get { return model.Satelite.Accion; } }
        public string Actitud { get { return model.Satelite.Actitud; } }

        public string ActualTime { get { return model.ActualTime.ToString(@"dd\.hh\:mm\:ss"); } }
        public string AcceleratedTime
        {
            get { return timeManager.ToString(); }
        }

        public ObservableCollection<string> Acciones { get; protected set; }
        public ObservableCollection<string> Actitudes { get; protected set; }
        string accionGuardada = string.Empty;
        string actitudGuardada = string.Empty;

        SimulatorLoop model;
        TimeManager timeManager;
        Timer timer;
        object obj = new object();
        private readonly SynchronizationContext uiContext;

        public MainWindowViewModel(SimulatorLoop model, TimeManager timeManager)
        {
            ControlesViewModel = new PlayerControlsViewModel(timeManager);

            Acciones = new ObservableCollection<string>();
            Actitudes = new ObservableCollection<string>();
            uiContext = SynchronizationContext.Current;

            timer = new Timer(timer_Elapsed, null, 2000, 300);
            this.model = model;
            this.timeManager = timeManager;

            //model.Start();
        }

        void timer_Elapsed(object stateInfo)
        {
            if (Monitor.TryEnter(obj))
            {
                uiContext.Post(x => refreshValues(), null);

                Monitor.Exit(obj);
            }
        }

        void refreshValues()
        {
            RaisePropertyChanged("AcceleratedTime");
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
                string msg = string.Format("[{1}] {0}", Accion, ActualTime);
                Acciones.Insert(0, msg);
                accionGuardada = Accion;
            }

            if ((actitudGuardada != Actitud) && (!string.IsNullOrEmpty(Actitud)))
            {
                string msg = string.Format("[{1}] {0}", Actitud, ActualTime);
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
                    _slowTimeCommand = new RelayCommand(() => this.SlowTime(), () => timeManager.FactorTemporal != TimeIntervals.Menos1000);

                return _slowTimeCommand;
            }
        }
        RelayCommand _slowTimeCommand;

        void SlowTime()
        {
            timeManager.SlowTime();
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
                    _fastTimeCommand = new RelayCommand(() => this.FastTime(), () => timeManager.FactorTemporal != TimeIntervals.Mas1000);

                return _fastTimeCommand;
            }
        }
        RelayCommand _fastTimeCommand;

        void FastTime()
        {
            timeManager.FastTime();
        }
        #endregion
    }
}
