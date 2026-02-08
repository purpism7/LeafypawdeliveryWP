using UnityEngine;

public static class SystemInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoad()
    {
        Debug.Log("씬이 로드되기 직전에 실행됩니다 (데이터 캐싱 등)");
        
        var addressalbeManager = new AddressableManager();
    }
}
