using UnityEngine;
using UnityEditor;

/// <summary>
/// WeatherManager 프리팹/씬에서 SnowRoot에 눈 파티클을 생성합니다.
/// 메뉴: Tools > Leafypaw > Setup Snow Effect
/// </summary>
public static class WeatherManagerSnowSetup
{
    [MenuItem("Tools/Leafypaw/Setup Snow Effect")]
    public static void SetupSnowEffect()
    {
        var manager = Object.FindFirstObjectByType<WeatherManager>();
        if (manager == null)
        {
            Debug.LogWarning("씬 또는 프리팹에 WeatherManager가 없습니다.");
            return;
        }

        var weatherRoot = manager.transform.Find("Root/WeatherRoot");
        if (weatherRoot == null)
        {
            Debug.LogWarning("WeatherRoot를 찾을 수 없습니다.");
            return;
        }

        var snowRoot = weatherRoot.Find("SnowRoot");
        if (snowRoot == null)
        {
            Debug.LogWarning("SnowRoot를 찾을 수 없습니다. WeatherManager 프리팹에 SnowRoot가 있는지 확인하세요.");
            return;
        }

        if (snowRoot.childCount > 0)
        {
            Debug.Log("SnowRoot에 이미 자식이 있습니다. 눈 이펙트가 이미 설정된 것 같습니다.");
            return;
        }

        // RainEffect 참고용
        var rainRoot = weatherRoot.Find("RainRoot");
        ParticleSystem rainPs = null;
        ParticleSystemRenderer rainRenderer = null;
        if (rainRoot != null)
        {
            var rainEffect = rainRoot.Find("RainEffect");
            if (rainEffect != null)
            {
                rainPs = rainEffect.GetComponent<ParticleSystem>();
                rainRenderer = rainEffect.GetComponent<ParticleSystemRenderer>();
            }
        }

        var snowEffectGo = new GameObject("SnowEffect");
        snowEffectGo.transform.SetParent(snowRoot, false);
        snowEffectGo.transform.localPosition = new Vector3(0f, 60f, -1f);
        snowEffectGo.transform.localRotation = Quaternion.identity;
        snowEffectGo.transform.localScale = Vector3.one;

        var ps = snowEffectGo.AddComponent<ParticleSystem>();
        var renderer = snowEffectGo.GetComponent<ParticleSystemRenderer>();

        // 메인 모듈 - 눈에 맞게
        var main = ps.main;
        main.duration = 5f;
        //main.looping = true;
        main.startLifetime = 8f;
        main.startSpeed = 1.5f;
        main.startSize = 0.12f;
        main.startColor = new Color(1f, 1f, 1f, 0.9f);
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.playOnAwake = true;

        // Emission
        var emission = ps.emission;
        emission.rateOverTime = 150f;

        // Shape - 박스 (비와 비슷하게 넓게)
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(150f, 1f, 1f);

        // Gravity (눈은 천천히 흩날림)
        main.gravityModifier = 0.15f;

        // Renderer - 비와 같은 머티리얼 사용
        if (rainRenderer != null && rainRenderer.sharedMaterial != null)
            renderer.sharedMaterial = rainRenderer.sharedMaterial;
        else
            renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Particle.mat");
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.minParticleSize = 0.01f;
        renderer.maxParticleSize = 0.2f;

#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfPrefabInstance(manager.gameObject))
            PrefabUtility.RecordPrefabInstancePropertyModifications(manager.gameObject);
        EditorUtility.SetDirty(snowRoot.gameObject);
#endif
        Debug.Log("SnowEffect가 SnowRoot 아래에 생성되었습니다. Apply(WeatherType.Snowy)로 눈을 표시할 수 있습니다.");
    }
}
