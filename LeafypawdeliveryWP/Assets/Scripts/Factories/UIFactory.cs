using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

using Features.Main;

namespace Factories
{
    public class UIFactory
    {
        // private readonly AddressableManager _addressableManager = null;
        
        private readonly Dictionary<System.Type, Factory> _factories = null;
        // private HashSet<Factory> _factories = null;
        
        public UIFactory(AddressableManager addressableManager)
        {
            // _addressableManager = addressableManager;
            
            _factories = new();
            
            RegisterFactory<MainPresenter>(new MainFactory(addressableManager));
        }

        private void RegisterFactory<TPresenter>(Factory factory)
        {
            _factories[typeof(TPresenter)] = factory;
        }

        public async UniTask<TPresenter> CreateAsync<TPresenter>() where TPresenter : class
        {
            var targetType = typeof(TPresenter);

            if (_factories.TryGetValue(targetType, out var factory))
                return await factory.CreateAsync<TPresenter>();

            Debug.LogError($"{targetType.Name}에 할당된 팩토리를 찾을 수 없습니다.");
            return null;
        }

        // private Factory GetFactory<TPresenter>()
        // {
        //     var presenterType = typeof(TPresenter);
        //
        //     if (_factories.TryGetValue(presenterType, out var cachedFactory))
        //         return cachedFactory;
        //
        //     string presenterName = presenterType.Name;
        //     string factoryName = presenterName.EndsWith("Presenter")
        //         ? presenterName.Replace("Presenter", "Factory")
        //         : presenterName + "Factory";
        //
        //     var factoryType = AppDomain.CurrentDomain
        //         .GetAssemblies()
        //         .SelectMany(a => a.GetTypes())
        //         .FirstOrDefault(t =>
        //             t.Name == factoryName &&
        //             typeof(Factory).IsAssignableFrom(t));
        //
        //     if (factoryType == null)
        //     {
        //         Debug.LogError($"{factoryName} 타입을 찾을 수 없습니다.");
        //         return null;
        //     }
        //
        //     try
        //     {
        //         var factory = Activator.CreateInstance(factoryType, _addressableManager) as Factory;
        //         // _factories[presenterType] = factory;
        //         return factory;
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"{factoryName} 생성 중 오류 발생: {e}");
        //         return null;
        //     }
        // }
    }
}

