using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MvvMHelpers.core
{
    public class BaseViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {

        #region property helper
        protected bool Set<T>(ref T storage, T value, Action? changedAction = null, Action<T, T>? changingAction = null, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            this.OnPropertyChanging(propertyName);
            changingAction?.Invoke(storage, value);
            storage = value;
            this.OnPropertyChanged(propertyName);
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
