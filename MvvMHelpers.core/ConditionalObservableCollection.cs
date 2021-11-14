using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MvvMHelpers.core
{
    public class ConditionalObservableCollection<T>
        : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged
        where T : class
    {
        #region private member
        private readonly Predicate<T> _predicate;
        private readonly List<ItemContainer> _internalItems;
        #endregion

        #region properties
        public int Count => _internalItems.Count;
        public int VisibleCount => _internalItems.Count(i => _predicate(i.Item));
        #endregion

        #region ctor
        public ConditionalObservableCollection(Predicate<T> predicate)
        {
            _predicate = predicate;
            _internalItems = new List<ItemContainer>();
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
                if (_predicate(item.Item))
                {
                    yield return item.Item;
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region public methods
        public void Add(T item)
        {
            if (_internalItems.Any(i => i.Item == item))
            {
                return;
            }
            var isVisible = _predicate(item);
            AddItemPropertyChangedHandler(item);
            _internalItems.Add(new ItemContainer(isVisible, item));

            if (isVisible)
            {
                RaiseCollectionAdd(item);
                RaisePropertyChanged(nameof(VisibleCount));
            }
        }
        public void AddRange(IEnumerable<T> items)
        {

            var anyVisible = items.Any(i => _predicate(i));

            foreach (var item in items)
            {
                if (_internalItems.Any(i => i.Item == item))
                {
                    continue;
                }
                AddItemPropertyChangedHandler(item);
                _internalItems.Add(new ItemContainer(_predicate(item), item));
            }

            if (anyVisible)
            {
                RaiseCollectionAddRange(items);
                RaisePropertyChanged(nameof(VisibleCount));
            }
        }
        public void Clear()
        {
            var hadVisible = _internalItems.Any(i => i.Predicate);
            foreach (var item in _internalItems)
            {
                RemoveItemPropertyChangedHandler(item.Item);
            }
            _internalItems.Clear();

            if (hadVisible)
            {
                RaiseCollectionClear();
                RaisePropertyChanged(nameof(VisibleCount));
            }
        }
        public bool Remove(T item)
        {
            var removedVisible = _internalItems.Any(i => i.Item == item && _predicate(i.Item));
            var state = _internalItems.RemoveAll(i => i.Item == item);
            if (removedVisible)
            {
                RemoveItemPropertyChangedHandler(item);
                RaiseCollectionRemove(item);
            }
            if (state >= 0)
            {
                RaisePropertyChanged(nameof(Count));
            }
            return state > 0;
        }
        public bool Contains(T item)
        {
            return _internalItems.Any(i => i.Item == item);
        }
        public void RecalculatePredicate()
        {
            foreach (var item in _internalItems)
            {
                var newPredicate = _predicate(item.Item);
                if (item.Predicate != newPredicate)
                {
                    item.Predicate = newPredicate;
                    if (newPredicate)
                    {
                        RaiseCollectionAdd(item.Item);
                    }
                    else
                    {
                        RaiseCollectionRemove(item.Item);
                    }
                }
            }
        }
        #endregion

        #region private event helper
        private void RaiseCollectionAdd(T item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            RaisePropertyChanged(nameof(Count));
        }
        private void RaiseCollectionAddRange(IEnumerable<T> items)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
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
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void AddItemPropertyChangedHandler(T item)
        {
            if (item is not INotifyPropertyChanged notifyItem)
            {
                return;
            }
            notifyItem.PropertyChanged += HandleItemPropertyChanged;
        }
        private void RemoveItemPropertyChangedHandler(T item)
        {
            if (item is not INotifyPropertyChanged notifyItem)
            {
                return;
            }
            notifyItem.PropertyChanged -= HandleItemPropertyChanged;
        }
        private void HandleItemPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (sender is not T item)
            {
                return;
            }
            var collectionItem = _internalItems.FirstOrDefault(i => i.Item == item);

            var newPredicate = _predicate(item);
            if (newPredicate != collectionItem.Predicate)
            {
                collectionItem.Predicate = newPredicate;
                if (collectionItem.Predicate)
                {
                    RaiseCollectionAdd(collectionItem.Item);
                }
                else
                {
                    RaiseCollectionRemove(collectionItem.Item);
                }
            }
        }
        #endregion


        private class ItemContainer
        {
            public bool Predicate { get; set; }
            public T Item { get; set; }

            public ItemContainer(bool isVisible, T item)
            {
                Predicate = isVisible;
                Item = item;
            }

        }
    }
}





