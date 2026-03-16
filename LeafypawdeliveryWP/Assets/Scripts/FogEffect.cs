using UnityEngine;

/// <summary>
/// 2D 안개 효과. Custom/2DFog 쉐이더 머티리얼을 사용해 카메라 뷰를 덮는 풀스크린 안개를 그립니다.
/// WeatherManager의 FogRoot 아래에 두고 Apply(WeatherType.Foggy)로 표시합니다.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FogEffect : MonoBehaviour
{
    [SerializeField] private Material fogMaterial = null;
    [SerializeField] private Camera targetCamera = null;
    [Tooltip("카메라 앞쪽으로 얼마나 떨어져 그릴지 (Z)")]
    [SerializeField] private float distanceFromCamera = 10f;
    [Tooltip("뷰보다 살짝 크게 그려 경계가 안 보이게")]
    [SerializeField] private float sizePadding = 2f;

    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        if (fogMaterial != null)
            _meshRenderer.sharedMaterial = fogMaterial;
        else
            Debug.LogWarning("FogEffect: Fog Material이 할당되지 않았습니다. Custom_2DFog 머티리얼을 할당하세요.");

        if (targetCamera == null)
            targetCamera = Camera.main;

        // 2D에서 안개가 맨 앞에 그려지도록 정렬 (다른 렌더러보다 위에)
        _meshRenderer.sortingOrder = 32767;

        BuildQuadMesh();
    }

    private void Start()
    {
        RefreshSizeAndPosition();
    }

    private void LateUpdate()
    {
        RefreshSizeAndPosition();
    }

    /// <summary>
    /// UV가 (0,0)~(1,1)인 단순 Quad 메시 생성 (쉐이더 노이즈용)
    /// </summary>
    private void BuildQuadMesh()
    {
        var mesh = new Mesh { name = "FogQuad" };
        mesh.SetVertices(new[]
        {
            new Vector3(-0.5f, -0.5f, 0f),
            new Vector3( 0.5f, -0.5f, 0f),
            new Vector3( 0.5f,  0.5f, 0f),
            new Vector3(-0.5f,  0.5f, 0f),
        });
        mesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 3 }, 0);
        mesh.SetUVs(0, new[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f),
        });
        mesh.RecalculateBounds();
        _meshFilter.sharedMesh = mesh;
    }

    private void RefreshSizeAndPosition()
    {
        if (targetCamera == null)
            return;

        if (!targetCamera.orthographic)
        {
            Debug.LogWarning("FogEffect: 오쏘그래픽 카메라에서만 뷰 크기가 계산됩니다.");
            return;
        }

        float orthoSize = targetCamera.orthographicSize + sizePadding;
        float aspect = targetCamera.aspect;
        float width = orthoSize * 2f * aspect;
        float height = orthoSize * 2f;

        transform.position = targetCamera.transform.position + Vector3.forward * distanceFromCamera;
        transform.rotation = Quaternion.identity;
        transform.localScale = new Vector3(width, height, 1f);
    }
}
