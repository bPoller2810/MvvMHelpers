using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MvvMHelpers.core
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {

        #region property helper
        protected bool Set<T>(ref T storage, T value, Action? changedAction = null, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            OnPropertyChanging(propertyName);
            storage = value;
            OnPropertyChanged(propertyName);
            changedAction?.Invoke();
            return true;
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion

        #region INotifyPropertyChanging
        public event PropertyChangingEventHandler? PropertyChanging;

        public void OnPropertyChanging([CallerMemberName] string? propertyname = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyname));
        }
        #endregion

    }
}
