using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using Factories;

public abstract class SceneBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        InitializeAsync().Forget();
    }

    protected virtual async UniTask InitializeAsync()
    {
        var addressableManager = new AddressableManager();

        if(addressableManager != null)
            await addressableManager.PreloadAsync();

        MainFactory factory = new MainFactory(addressableManager);
        factory.CreateAsync().Forget();
    }
}
