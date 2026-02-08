using System;
using UnityEngine;

public abstract class SceneBootstrapper : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        Debug.Log("SceneBootstrapper Initialize");
        var uIManager = new UIManager();
    }
}
