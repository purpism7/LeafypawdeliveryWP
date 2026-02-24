using UnityEngine;

public static class SystemInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoad()
    {

        Debug.Log("[SystemInitializer] Rain runtime install requested.");
    }
}
