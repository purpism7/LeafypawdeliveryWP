using Cysharp.Threading.Tasks;
using UnityEngine;

using Features.Main;

public class GameSceneBootstrapper : SceneBootstrapper
{
    protected override async UniTask InitializeAsync()
    {
        await base.InitializeAsync();

        Debug.Log("GameSceneBootstrapper: Preload 끝난 뒤 진입");
        //_entityFactory?.Create<MainView, MainPresenter, MainModel>(null);

        await UniTask.CompletedTask;
    }
}
