using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFTimer
{
    public class DelegateCommand : ICommand, INotifyPropertyChanged
    {
        Action<object> execute;
        Func<object, bool> canExecute;
        private bool visible;
        public bool Visible {
            get { return visible; }
            set {
                visible = value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged([CallerMemberName] string prop = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        // Событие, необходимое для ICommand
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        // Два конструктора
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public DelegateCommand(Action<object> execute)
        {
            this.execute = execute;
            this.canExecute = this.AlwaysCanExecute;
        }

        public void Execute(object param)
        {
            if(CanExecute(param))
                execute(param);
        }

        public bool CanExecute(object param)
        {
            Visible = canExecute(param);
            return Visible;
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        private bool AlwaysCanExecute(object param)
        {
            return true;
        }
    }
}
