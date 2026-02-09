using UnityEngine;
using Cysharp.Threading.Tasks;

public static class SystemInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoad()
    {
        // Data Load 
    }
}
