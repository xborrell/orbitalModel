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

        public string AcceleratedTime
        {
            get { return timeManager.ToString(); }
        }

        public ObservableCollection<string> Acciones { get; protected set; }
        public ObservableCollection<string> Actitudes { get; protected set; }
        string accionGuardada = string.Empty;
        string actitudGuardada = string.Empty;

        SateliteDataList model;
        TimeManager timeManager;
        Timer timer;
        object obj = new object();
        private readonly SynchronizationContext uiContext;

        public MainWindowViewModel(SateliteDataList model, TimeManager timeManager)
        {
            ControlesViewModel = new PlayerControlsViewModel(timeManager);

            Acciones = new ObservableCollection<string>();
            Actitudes = new ObservableCollection<string>();
            uiContext = SynchronizationContext.Current;

            timer = new Timer(timer_Elapsed, null, 2000, 300);
            this.model = model;
            this.timeManager = timeManager;
            this.timeManager.Model = this.model;
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
                string msg = string.Format("[{1}] {0}", Accion, timeManager.ActualTime);
                Acciones.Insert(0, msg);
                accionGuardada = Accion;
            }

            if ((actitudGuardada != Actitud) && (!string.IsNullOrEmpty(Actitud)))
            {
                string msg = string.Format("[{1}] {0}", Actitud, timeManager.ActualTime);
                Actitudes.Insert(0, msg);
                actitudGuardada = Actitud;
            }
        }

        private string ValorOrbital(float valor, string mascara)
        {
            return valor < 0 ? "???" : valor.ToString(mascara);
        }
    }
}
