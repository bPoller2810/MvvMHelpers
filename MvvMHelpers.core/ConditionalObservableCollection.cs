using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MvvMHelpers.core
{
    public class ConditionalObservableCollection<T> : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged
        where T : class
    {
        #region private member
        private readonly Predicate<T> _predicate;
        private readonly List<KeyValuePair<bool, T>> _internalItems;
        #endregion

        #region properties
        public int Count => _internalItems.Count;
        public int VisibleCount => _internalItems.Count(i => _predicate(i.Value));
        #endregion

        #region ctor
        public ConditionalObservableCollection(Predicate<T> predicate)
        {
            _predicate = predicate;
            _internalItems = new List<KeyValuePair<bool, T>>();
        }
        #endregion

        #region INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _internalItems)
            {
                if (_predicate(item.Value))
                {
                    yield return item.Value;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IList<T>
        public void Add(T item)
        {
            if (_internalItems.Any(i => i.Value == item))
            {
                return;
            }
            var isVisible = _predicate(item);
            _internalItems.Add(new KeyValuePair<bool, T>(isVisible, item));
            RaiseCollectionAdd(item);
            if (isVisible)
            {
                RaisePropertyChanged(nameof(VisibleCount));
            }
        }
        public int IndexOf(T item)
        {
            var existing = _internalItems.FirstOrDefault(i => i.Value == item);
            return _internalItems.IndexOf(existing);
        }
        public void Insert(int location, T item)
        {
            var old = _internalItems[location];
            var isVisible = _predicate(item);
            _internalItems.Insert(location, new KeyValuePair<bool, T>(isVisible, item));
            RaiseCollectionInsert(item, old.Value);
            if (isVisible)
            {
                RaisePropertyChanged(nameof(VisibleCount));
            }
        }
        public void RemoveAt(int index)
        {
            var removed = _internalItems[index];
            _internalItems.RemoveAt(index);
            RaiseCollectionRemove(removed.Value);
        }
        public T this[int index]
        {
            get => _internalItems[index].Value;
            set => _internalItems[index] = new KeyValuePair<bool, T>(_predicate(value), value);
        }
        #endregion

        #region public methods
        public void Clear()
        {
            var hadVisible = _internalItems.Any(i => i.Key);
            _internalItems.Clear();
            RaiseCollectionClear();
            if (hadVisible)
            {
                RaisePropertyChanged(nameof(VisibleCount));
            }
        }
        public bool Remove(T item)
        {
            var state = _internalItems.RemoveAll(i => i.Value == item);
            RaiseCollectionRemove(item);
            return state > 0;
        }
        public bool Contains(T item)
        {
            return _internalItems.Any(i => i.Value == item);
        }
        #endregion

        #region private event helper
        private void RaiseCollectionAdd(T item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            RaisePropertyChanged(nameof(Count));
        }
        private void RaiseCollectionClear()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            RaisePropertyChanged(nameof(Count));
        }
        private void RaiseCollectionRemove(T item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            RaisePropertyChanged(nameof(Count));
        }
        private void RaiseCollectionInsert(T item, T old)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, old));
            RaisePropertyChanged(nameof(Count));
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}





