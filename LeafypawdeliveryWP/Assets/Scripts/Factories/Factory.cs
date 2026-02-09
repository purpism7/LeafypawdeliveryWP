using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Factories
{
    public abstract class Factory
    {
        private readonly AddressableManager _addressableManager = null;

        public abstract UniTask CreateAsync();

        public Factory(AddressableManager addressableManager)
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

