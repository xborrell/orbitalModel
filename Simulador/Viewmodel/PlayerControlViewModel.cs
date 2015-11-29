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
            get { return timeManager.IsPlaying; }
        }

        public string ActualTime
        {
            get
            {
                return string.Format(@"{0:%d}d. {0:hh\:mm\:ss},{0:ff}", timeManager.ActualTime);
            }
        }

        public string TimeLapse
        {
            get
            {
                switch (timeManager.FactorTemporal)
                {
                    case TimeIntervals.UnDecimo: return "+ 1/10";
                    case TimeIntervals.UnCuarto: return "+ 1/4";
                    case TimeIntervals.XUnMedio: return "+ 1/2";
                    case TimeIntervals.X1: return "+ 1";
                    case TimeIntervals.X2: return "+ 2";
                    case TimeIntervals.X5: return "+ 5";
                    case TimeIntervals.X10: return "+ 10";
                    case TimeIntervals.X50: return "+ 50";
                    case TimeIntervals.X100: return "+ 100";
                    case TimeIntervals.X500: return "+ 500";
                    case TimeIntervals.X1000: return "+ 1000";
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
            Observer.RegisterHandler(n => n.FactorTemporal, n => base.RaisePropertyChanged("TimeLapse"))
                    .RegisterHandler(n => n.IsPlaying, n => base.RaisePropertyChanged("IsPlaying"))
                    .RegisterHandler(n => n.ActualTime, n => base.RaisePropertyChanged("ActualTime"));
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
                timeManager.Pause();
            }
            else
            {
                timeManager.Play();
            }
        }
        #endregion

        #region BackStep Command
        public ICommand BackStepCommand
        {
            get
            {
                if (_backStepCommand == null)
                    _backStepCommand = new RelayCommand(() => this.BackStep(), () => !IsPlaying && timeManager.CanBackStep());

                return _backStepCommand;
            }
        }
        RelayCommand _backStepCommand;

        void BackStep()
        {
            timeManager.BackStep();
        }
        #endregion

        #region NextStep Command
        public ICommand NextStepCommand
        {
            get
            {
                if (nextStepCommand == null)
                    nextStepCommand = new RelayCommand(() => this.NextStep(), () => !IsPlaying && timeManager.CanNextStep());

                return nextStepCommand;
            }
        }
        RelayCommand nextStepCommand;

        void NextStep()
        {
            timeManager.NextStep();
        }
        #endregion

        #region BackPage Command
        public ICommand BackPageCommand
        {
            get
            {
                if (backPageCommand == null)
                    backPageCommand = new RelayCommand(() => this.BackPage(), () => !IsPlaying && timeManager.CanBackPage());

                return backPageCommand;
            }
        }
        RelayCommand backPageCommand;

        void BackPage()
        {
            timeManager.BackPage();
        }
        #endregion

        #region NextPage Command
        public ICommand NextPageCommand
        {
            get
            {
                if (nextPageCommand == null)
                    nextPageCommand = new RelayCommand(() => this.NextPage(), () => !IsPlaying && timeManager.CanNextPage());

                return nextPageCommand;
            }
        }
        RelayCommand nextPageCommand;

        void NextPage()
        {
            timeManager.NextPage();
        }
        #endregion

        #region SlowTime Command
        public ICommand SlowTimeCommand
        {
            get
            {
                if (slowTimeCommand == null)
                    slowTimeCommand = new RelayCommand(() => this.SlowTime(), () => timeManager.CanSlowTime());

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
                    fastTimeCommand = new RelayCommand(() => this.FastTime(), () => timeManager.CanFastTime());

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
