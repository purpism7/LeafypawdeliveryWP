using Cysharp.Threading.Tasks;
using UnityEngine;

using Common;

namespace Factories
{
    public abstract class Factory
    {
        private readonly AddressableManager _addressableManager = null;

        public abstract System.Type Type { get; } 
        
        public abstract UniTask<TPresenter> CreateAsync<TPresenter>() where TPresenter : class;

        protected Factory(AddressableManager addressableManager)
        {
            _addressableManager = addressableManager;
        }

        protected TView CreateView<TView>() where TView : Entity
        {
            var viewObj = _addressableManager?.Get<TView>();
            return GameObject.Instantiate<TView>(viewObj);
        }

        protected TModel CreateModel<TModel>() where TModel : class, new()
        {
            return new TModel();
        }
    }
}

