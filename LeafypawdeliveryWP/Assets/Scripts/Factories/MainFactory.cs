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

        public override async UniTask CreateAsync()
        {
            var view = CreateView<MainView>();
            var model = CreateModel<MainModel>();

            var presenter = new MainPresenter(view, model);
        }
    }
}

