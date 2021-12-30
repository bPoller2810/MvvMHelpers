using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvvMHelpers.core
{
    public abstract class BaseListViewModel<TModel, TViewModel>
        where TViewModel : BaseItemViewModel<TModel>, new()
    {

        #region properties
        public ConditionalObservableCollection<TViewModel> Items { get; }
        #endregion

        #region abstract / virtual
        protected virtual Predicate<TViewModel> ItemsFilter => _ => true;
        protected abstract Task<IEnumerable<TModel>> DataRequest { get; }
        protected abstract void ItemsLoaded(Exception? exception = null);
        protected abstract object?[]? ItemArguments { get; }
        #endregion

        #region ctor
        public BaseListViewModel()
        {
            Items = new ConditionalObservableCollection<TViewModel>(ItemsFilter);
        }
        #endregion

        #region public methods
        public async Task LoadItems()
        {
            try
            {
                var data = await DataRequest;
                Items.Clear();
                foreach (var item in data)
                {
                    var vm = new TViewModel();
                    vm.ConfigureItem(item);
                    Items.Add(vm);
                }
                ItemsLoaded();
            }
            catch (Exception exception)
            {
                ItemsLoaded(exception);
            }
        }
        #endregion

    }
}
