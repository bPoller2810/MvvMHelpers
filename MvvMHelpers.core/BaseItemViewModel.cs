using System;

namespace MvvMHelpers.core
{
    public abstract class BaseItemViewModel<TModel> : BaseViewModel
    {
        private TModel? _item;

        /// <summary>
        /// The model item that will be set in ConfigureItem
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public TModel Item
        {
            get => _item ?? throw new InvalidOperationException("Item is not yet initialized");
            protected set => _item = value;
        }

        /// <summary>
        /// <para>A way to initialize this ViewModel when the model gets applied</para>
        /// - Always call base.ConfigureItem(model) first!!!
        /// </summary>
        /// <param name="model"></param>
        public virtual void ConfigureItem(TModel model)
        {
            _item = model;
        }


    }
}
