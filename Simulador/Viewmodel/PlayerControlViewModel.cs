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
    public class PlayerControlsViewModel : ObservableObject
    {
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

        public string TimeLapse
        {
            get
            {
                switch (timeManager.FactorTemporal)
                {
                    case TimeIntervals.Menos1000: return "- 1000";
                    case TimeIntervals.Menos500: return "- 500";
                    case TimeIntervals.Menos100: return "- 100";
                    case TimeIntervals.Menos50: return "- 50";
                    case TimeIntervals.Menos10: return "- 10";
                    case TimeIntervals.Menos5: return "- 5";
                    case TimeIntervals.Menos2: return "- 2";
                    case TimeIntervals.Menos1: return "- 1";
                    case TimeIntervals.MenosMedio: return "- 1/2";
                    case TimeIntervals.MenosCuarto: return "- 1/4";
                    case TimeIntervals.Cero: return "Pause";
                    case TimeIntervals.MasCuarto: return "+ 1/4";
                    case TimeIntervals.MasMedio: return "+ 1/2";
                    case TimeIntervals.Mas1: return "+ 1";
                    case TimeIntervals.Mas2: return "+ 2";
                    case TimeIntervals.Mas5: return "+ 5";
                    case TimeIntervals.Mas10: return "+ 10";
                    case TimeIntervals.Mas50: return "+ 50";
                    case TimeIntervals.Mas100: return "+ 100";
                    case TimeIntervals.Mas500: return "+ 500";
                    case TimeIntervals.Mas1000: return "+ 1000";
                    default: throw new ArgumentException();
                }
            }
        }

        TimeManager timeManager;
        protected PropertyObserver<TimeManager> Observer { get; private set; }

        public PlayerControlsViewModel(TimeManager timeManager)
        {
            this.timeManager = timeManager;

            Observer = new PropertyObserver<TimeManager>(this.timeManager);
            Observer.RegisterHandler(n => n.FactorTemporal, n => base.RaisePropertyChanged("TimeLapse"));
        }

        #region Play/Pause Command
        public ICommand PlayPauseCommand
        {
            get
            {
                if (playPauseCommand == null)
                    playPauseCommand = new RelayCommand(() => this.PlayPause());

                return playPauseCommand;
            }
        }
        RelayCommand playPauseCommand;

        void PlayPause()
        {
            if (IsPlaying)
            {
                IsPlaying = false;
            }
            else
            {
                IsPlaying = true;
            }
        }
        #endregion

        #region BackStep Command
        public ICommand BackStepCommand
        {
            get
            {
                if (_backStepCommand == null)
                    _backStepCommand = new RelayCommand(() => this.BackStep(), () => !IsPlaying);

                return _backStepCommand;
            }
        }
        RelayCommand _backStepCommand;

        void BackStep()
        {
        }
        #endregion

        #region NextStep Command
        public ICommand NextStepCommand
        {
            get
            {
                if (nextStepCommand == null)
                    nextStepCommand = new RelayCommand(() => this.NextStep(), () => !IsPlaying);

                return _backStepCommand;
            }
        }
        RelayCommand nextStepCommand;

        void NextStep()
        {
        }
        #endregion

        #region BackPage Command
        public ICommand BackPageCommand
        {
            get
            {
                if (backPageCommand == null)
                    backPageCommand = new RelayCommand(() => this.BackPage(), () => !IsPlaying);

                return backPageCommand;
            }
        }
        RelayCommand backPageCommand;

        void BackPage()
        {
        }
        #endregion

        #region NextPage Command
        public ICommand NextPageCommand
        {
            get
            {
                if (nextPageCommand == null)
                    nextPageCommand = new RelayCommand(() => this.NextPage(), () => !IsPlaying);

                return nextPageCommand;
            }
        }
        RelayCommand nextPageCommand;

        void NextPage()
        {
        }
        #endregion

        #region SlowTime Command
        public ICommand SlowTimeCommand
        {
            get
            {
                if (slowTimeCommand == null)
                    slowTimeCommand = new RelayCommand(() => this.SlowTime(), () => IsPlaying && timeManager.CanSlowTime());

                return slowTimeCommand;
            }
        }
        RelayCommand slowTimeCommand;

        void SlowTime()
        {
            timeManager.SlowTime();
        }
        #endregion

        #region FastTime Command
        public ICommand FastTimeCommand
        {
            get
            {
                if (fastTimeCommand == null)
                    fastTimeCommand = new RelayCommand(() => this.FastTime(), () => IsPlaying && timeManager.CanFastTime());

                return fastTimeCommand;
            }
        }
        RelayCommand fastTimeCommand;

        void FastTime()
        {
            timeManager.FastTime();
        }
        #endregion
    }
}
