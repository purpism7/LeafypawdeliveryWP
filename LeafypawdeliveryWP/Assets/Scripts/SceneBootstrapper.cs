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
        
        UIManager uiManager = new UIManager(addressableManager);

        var mainPresenter = await uiManager.OpenAsync<MainPresenter>();
        // MainFactory factory = new MainFactory(addressableManager);
        // var mainPresenter = await factory.CreateAsync<MainPresenter>();
        // factory.CreateAsync().Forget();
    }
}
