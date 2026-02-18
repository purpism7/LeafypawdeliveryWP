Shader "Custom/2DFog"
{
    Properties
    {
        [MainTexture] _MainTex("Fog Noise Texture", 2D) = "white" {}
        _SecondTex("Second Noise Texture", 2D) = "white" {}
        
        [HDR] _FogColor("Fog Color", Color) = (1, 1, 1, 1)
        
        _Speed1("Layer 1 Speed (XY)", Vector) = (0.1, 0.05, 0, 0)
        _Speed2("Layer 2 Speed (XY)", Vector) = (-0.05, 0.1, 0, 0)
        
        _Density("Fog Density", Range(0, 5)) = 1.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR; // Sprite Renderer의 컬러 반영용
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float4 color        : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_SecondTex);
            SAMPLER(sampler_SecondTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _SecondTex_ST;
                float4 _FogColor;
                float2 _Speed1;
                float2 _Speed2;
                float _Density;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 시간 흐름에 따른 UV 오프셋 계산 (_Time.y는 초 단위 시간)
                float2 uv1 = IN.uv * _MainTex_ST.xy + _MainTex_ST.zw + (_Speed1 * _Time.y);
                float2 uv2 = IN.uv * _SecondTex_ST.xy + _SecondTex_ST.zw + (_Speed2 * _Time.y);

                // 노이즈 텍스처 샘플링 (R 채널만 사용해도 무방)
                half noise1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv1).r;
                half noise2 = SAMPLE_TEXTURE2D(_SecondTex, sampler_SecondTex, uv2).r;

                // 두 노이즈를 결합하여 풍부한 패턴 생성
                half combinedNoise = noise1 * noise2 * _Density;

                // 최종 컬러 계산 (설정한 색상 * 결합 노이즈 * 스프라이트 컬러)
                half4 finalColor = _FogColor * combinedNoise * IN.color;

                // 결합된 노이즈 값을 알파로 사용하여 투명도 조절
                finalColor.a = saturate(combinedNoise * _FogColor.a * IN.color.a);

                return finalColor;
            }
            ENDHLSL
        }
    }
}