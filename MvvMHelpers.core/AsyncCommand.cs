using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvMHelpers.core
{

    public class AsyncCommand : ICommand
    {
        readonly Func<object?, Task> _execute;
        readonly Func<object?, bool> _canExecute;

        /// <summary>
        /// Use this constructor for commands that have a command parameter.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        /// <param name="notificationSource"></param>
        public AsyncCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null, INotifyPropertyChanged? notificationSource = null)
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
        public AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null, INotifyPropertyChanged? notificationSource = null)
            : this(_ => execute.Invoke(), _ => (canExecute ?? (() => true)).Invoke(), notificationSource)
        {
        }

        public bool CanExecute(object? param = null) => _canExecute.Invoke(param);

        public Task ExecuteAsync(object? param = null) => _execute.Invoke(param);

        public async void Execute(object? param = null)
        {
            // TBD: Consider adding exception-handling logic here.
            // Without such logic, quoting https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming
            // "With async void methods, there is no Task object, so any exceptions thrown out of an async void method will be raised directly on the SynchronizationContext that was active when the async void method started."
            await ExecuteAsync(param);
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }


}
