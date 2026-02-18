using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

using Features.Main;

namespace Factories
{
    public class MainFactory : Factory
    {
        public MainFactory(AddressableManager addressableManager) : base(addressableManager)
        {

        }

        public override Type Type => typeof(MainPresenter);

        public override async UniTask<TPresenter> CreateAsync<TPresenter>()
        {
            var view = CreateView<MainView>();
            var model = CreateModel<MainModel>();
        
            var presenter = new MainPresenter(view, model) as TPresenter;
            
            return presenter;
        }
    }
}

