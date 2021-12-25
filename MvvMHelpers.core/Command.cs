using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MvvMHelpers.core
{
    public  class Command : ICommand
    {

        readonly Action<object> _execute;
        readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Use this constructor for commands that have a command parameter.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <param name="notificationSource"></param>
        public Command(Action<object> execute, Func<object, bool> canExecute = null, INotifyPropertyChanged notificationSource = null)
        {
            _execute = execute;
            _canExecute = canExecute ?? (_ => true);
            if (notificationSource != null)
            {
                notificationSource.PropertyChanged += (s, e) => RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Use this constructor for commands that don't have a command parameter.
        /// </summary>
        public Command(Action execute, Func<bool> canExecute = null, INotifyPropertyChanged notificationSource = null)
            : this(_ => execute.Invoke(), _ => (canExecute ?? (() => true)).Invoke(), notificationSource)
        {
        }

        public bool CanExecute(object param = null) => _canExecute.Invoke(param);

        public void Execute(object param = null) => _execute.Invoke(param);

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
