using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

using Common;

public class AddressableManager
{
    private Dictionary<System.Type, Common.Entity> _entities = null;
  

    public AddressableManager()
    {
        _entities = new();
    }

    public async UniTask PreloadAsync()
    {
        try
        {
            await LoadAssetAsync<GameObject>("default",
                (asyncOperationHandle) =>
                {
                    var gameObj = asyncOperationHandle.Result;
                    if (gameObj)
                    {
                        var entity = gameObj.GetComponent<Common.Entity>();
                        if (entity == null)
                            return;

                        Debug.Log(entity.name);
                        _entities?.TryAdd(entity.GetType(), entity);
                    }
                });
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[AddressableManager] Preload failed: {e.Message}");
        }
    }

    public TEntity Get<TEntity>() where TEntity : Entity
    {
        if (_entities != null)
        {
            if (_entities.TryGetValue(typeof(TEntity), out var entity))
                return entity as TEntity;
        }

        return null;
    }

    private T LoadAssetByNameAsync<T>(string addressableName) where T : Object
    {
        //Debug.Log(addressableName);
        var handler = Addressables.LoadAssetAsync<T>(addressableName);
        if (!handler.IsValid())
            return default(T);

        handler.WaitForCompletion();

        var result = handler.Result;
        // _cachedDic?.Add(addressableName, result);
        Addressables.Release(handler);

        return result;
    }

    private async UniTask LoadAssetAsync<T>(string labelKey, System.Action<AsyncOperationHandle<T>> action)
    {
        var locationAsync = await Addressables.LoadResourceLocationsAsync(labelKey);

        foreach (IResourceLocation resourceLocation in locationAsync)
        {
            var assetAsync = Addressables.LoadAssetAsync<T>(resourceLocation);
            await UniTask.WaitUntil(() => assetAsync.IsDone);

            if (assetAsync.Result != null)
                action(assetAsync);
        }
    }
}
