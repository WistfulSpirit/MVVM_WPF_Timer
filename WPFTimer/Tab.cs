using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFTimer
{
    public enum State { 
        None,
        Started, 
        Paused
    }
    public class Tab : INotifyPropertyChanged
    {
        private PausableTimer timer;
        private string title;
        private string curTime;
        private int number;
        private readonly DateTime creationTime;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly TabController tabController;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string ButtonText {
            get {
                return timer.State == State.Started || timer.State == State.Paused ? "Continue" : "Start"; 
            }
        }
        public string Title {
            get => title;
            set {
                title = value;
                OnPropertyChanged();
            }
        }
        public string CurTime {
            get { return curTime; }
            set {
                curTime = value;
                OnPropertyChanged();
            }
        }
        private void StartTimer() {
            timer.Start();
            OnPropertyChanged(nameof(ButtonText));
        }
        private void PauseTimer() {
            timer.Pause();
            OnPropertyChanged(nameof(ButtonText));
        }
        private void ResetTimer() {
            timer.Reset();
            UpdateCurTime();
            OnPropertyChanged(nameof(ButtonText));
        }
        private void UpdateCurTime() {
            CurTime = timer.ElapsedTime.ToString(@"hh\:mm\:ss");
        }

        private void InitTimer() {
            timer = new PausableTimer(DispatcherPriority.Render);
            timer.Interval = new TimeSpan(0,0,0,0,200);
            timer.Tick += Timer_Tick;
            UpdateCurTime();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timer.State == State.Started)
                UpdateCurTime();
        }
        public Tab(int n, TabController tabController) {
            number = n;
            creationTime = DateTime.Now;
            Title = $"Таймер {number} {creationTime.ToString("HH:mm", CultureInfo.CurrentCulture)}";
            this.tabController  = tabController;
            tabController.TabCollection.CollectionChanged += (sender,e) => { UpdateCommands(); };
            InitTimer();
        }

        #region Commands
        public void UpdateCommands()
        {
            pauseCommand?.RaiseCanExecuteChanged();
            startCommand?.RaiseCanExecuteChanged();
            resetCommand?.RaiseCanExecuteChanged();
            removeCommand?.RaiseCanExecuteChanged();
            tabController?.AddTab?.RaiseCanExecuteChanged();
        }

        private DelegateCommand pauseCommand;
        private DelegateCommand startCommand;
        private DelegateCommand resetCommand;
        private DelegateCommand removeCommand;

        public DelegateCommand StartCommand {
            get {
                return startCommand ?? (startCommand = new DelegateCommand((x) => {
                    StartTimer();
                    UpdateCommands();
                },
                (x)=> {
                    return timer.State == State.Paused || timer.State == State.None;
                }));
            }
        }
        public DelegateCommand PauseCommand {
            get {
                return pauseCommand ?? (pauseCommand= new DelegateCommand((x) => {
                    PauseTimer();
                    UpdateCommands();
                },
                (x) => {
                    return timer.State == State.Started;
                }));
            }
        }
        
        public DelegateCommand ResetCommand {
            get {
                return resetCommand ?? (resetCommand = new DelegateCommand((x) => {
                    ResetTimer();
                    UpdateCommands();
                },
                (x) => {
                    return timer.State == State.Paused;
                }));
            }
        }
        public DelegateCommand RemoveCommand {
            get {
                return removeCommand ?? (removeCommand = new DelegateCommand((x) => {
                    tabController.RemoveItem(this);
                },
                (x) => {
                    return tabController.TabCollection.Count > 1 && timer.State == State.None;
                }));
            }
        }
        #endregion
    }
}
