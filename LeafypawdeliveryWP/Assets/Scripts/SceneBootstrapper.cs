using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using Common;
using Factories;
using Features.Main;

public abstract class SceneBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        InitializeAsync().Forget();
    }

    protected virtual async UniTask InitializeAsync()
    {
        var addressableManager = new AddressableManager();
        await addressableManager.PreloadAsync();
        
        var uiFactory = new UIFactory(addressableManager);
        UIManager uiManager = new UIManager(uiFactory);

        var mainPresenter = await uiManager.OpenAsync<MainPresenter>();
        // MainFactory factory = new MainFactory(addressableManager);
        // var mainPresenter = await factory.CreateAsync<MainPresenter>();
        // factory.CreateAsync().Forget();
    }
}
