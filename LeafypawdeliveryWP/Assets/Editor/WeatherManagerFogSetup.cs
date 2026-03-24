using UnityEngine;
using UnityEditor;
using Common;

/// <summary>
/// WeatherManager 프리팹/씬에서 FogRoot에 2D 안개 이펙트(FogEffect)를 생성합니다.
/// 메뉴: Tools > Leafypaw > Setup Fog Effect
/// </summary>
public static class WeatherManagerFogSetup
{
    private const string FogMaterialPath = "Assets/2_Resource/Custom_2DFog.mat";

    [MenuItem("Tools/Leafypaw/Setup Fog Effect")]
    public static void SetupFogEffect()
    {
        var manager = Object.FindFirstObjectByType<WeatherManager>();
        if (manager == null)
        {
            Debug.LogWarning("씬 또는 프리팹에 WeatherManager가 없습니다.");
            return;
        }

        Transform weatherRoot = manager.transform.Find("Root/WeatherRoot");
        if (weatherRoot == null)
        {
            Debug.LogWarning("WeatherRoot를 찾을 수 없습니다. WeatherManager 프리팹에 Root/WeatherRoot가 있는지 확인하세요.");
            return;
        }

        Transform fogRoot = weatherRoot.Find("FogRoot");
        if (fogRoot == null)
        {
            var fogRootGo = new GameObject("FogRoot");
            fogRoot = fogRootGo.transform;
            fogRoot.SetParent(weatherRoot, false);
            fogRoot.localPosition = Vector3.zero;
            fogRoot.localRotation = Quaternion.identity;
            fogRoot.localScale = Vector3.one;
            Debug.Log("FogRoot를 생성했습니다.");
        }

        Transform fogEffectTransform = fogRoot.Find("FogEffect");
        if (fogEffectTransform != null)
        {
            Debug.Log("FogEffect가 이미 FogRoot 아래에 있습니다.");
            AddFogToWeatherListIfNeeded(manager, fogRoot);
            return;
        }

        var fogEffectGo = new GameObject("FogEffect");
        fogEffectTransform = fogEffectGo.transform;
        fogEffectTransform.SetParent(fogRoot, false);
        fogEffectTransform.localPosition = Vector3.zero;
        fogEffectTransform.localRotation = Quaternion.identity;
        fogEffectTransform.localScale = Vector3.one;

        fogEffectGo.AddComponent<MeshFilter>();
        var renderer = fogEffectGo.AddComponent<MeshRenderer>();
        var fogEffect = fogEffectGo.AddComponent<FogEffect>();

        var mat = AssetDatabase.LoadAssetAtPath<Material>(FogMaterialPath);
        if (mat != null)
        {
            renderer.sharedMaterial = mat;
            SerializedObject soFog = new SerializedObject(fogEffect);
            soFog.FindProperty("fogMaterial").objectReferenceValue = mat;
            soFog.ApplyModifiedPropertiesWithoutUndo();
        }
        else
        {
            Debug.LogWarning($"Custom_2DFog 머티리얼을 찾을 수 없습니다: {FogMaterialPath}. FogEffect에서 수동으로 할당하세요.");
        }

        AddFogToWeatherListIfNeeded(manager, fogRoot);

#if UNITY_EDITOR
        if (PrefabUtility.IsPartOfPrefabInstance(manager.gameObject))
            PrefabUtility.RecordPrefabInstancePropertyModifications(manager.gameObject);
        EditorUtility.SetDirty(fogRoot.gameObject);
#endif
        Debug.Log("FogEffect가 FogRoot 아래에 생성되었습니다. Apply(WeatherType.Foggy)로 안개를 표시할 수 있습니다.");
    }

    private static void AddFogToWeatherListIfNeeded(WeatherManager manager, Transform fogRoot)
    {
        var so = new SerializedObject(manager);
        var listProp = so.FindProperty("weatherList");
        int foggyEnumIndex = (int)WeatherType.Foggy;

        for (int i = 0; i < listProp.arraySize; i++)
        {
            var elem = listProp.GetArrayElementAtIndex(i);
            var typeProp = elem.FindPropertyRelative("weatherType");
            if (typeProp != null && typeProp.enumValueIndex == foggyEnumIndex)
            {
                var transformProp = elem.FindPropertyRelative("transform");
                if (transformProp != null && transformProp.objectReferenceValue != fogRoot)
                {
                    transformProp.objectReferenceValue = fogRoot;
                    so.ApplyModifiedPropertiesWithoutUndo();
                }
                return;
            }
        }

        listProp.InsertArrayElementAtIndex(listProp.arraySize);
        var newElem = listProp.GetArrayElementAtIndex(listProp.arraySize - 1);
        newElem.FindPropertyRelative("weatherType").enumValueIndex = foggyEnumIndex;
        newElem.FindPropertyRelative("transform").objectReferenceValue = fogRoot;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
