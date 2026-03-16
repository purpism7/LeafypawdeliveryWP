Shader "Custom/MovingFogSpread"
{
    Properties
    {
        // 텍스처 및 기본 설정
        [MainTexture] _NoiseTexture ("Noise Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _Density ("Fog Density", Range(0.0, 1.0)) = 0.25
        _Speed ("Fog Speed", Vector) = (0.02, 0.01, 0, 0)
        
        // 포그 형태 조절 (새로 추가된 부분)
        _Spread ("Fog Spread (퍼짐 정도)", Range(0.0, 1.0)) = 0.5
        _Softness ("Edge Softness (부드러움)", Range(0.01, 1.0)) = 0.5
    }

    SubShader
    {
        // 2D 렌더링용 태그 설정
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        // 투명도(알파) 처리
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
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            // 텍스처 샘플러 선언
            TEXTURE2D(_NoiseTexture);
            SAMPLER(sampler_NoiseTexture);

            // 변수 선언 (SRP Batcher 호환)
            CBUFFER_START(UnityPerMaterial)
                float4 _NoiseTexture_ST;
                float4 _BaseColor;
                float _Density;
                float4 _Speed;
                float _Spread;
                float _Softness;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _NoiseTexture);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 시간 흐름에 따른 UV 이동
                float2 uv = input.uv + frac(_Speed.xy * _Time.y);

                // 노이즈 텍스처 샘플링
                float noise = SAMPLE_TEXTURE2D(_NoiseTexture, sampler_NoiseTexture, uv).r;

                // smoothstep을 이용해 안개가 부드럽게 퍼지도록 계산
                float fog = smoothstep(_Spread - _Softness, _Spread + _Softness, noise);

                // 최종 색상 계산 (기본 색상 * 버텍스 컬러)
                half4 finalColor = _BaseColor * input.color;
                
                // 알파값에 포그 계산 결과와 Density 적용
                finalColor.a *= fog * _Density;

                return finalColor;
            }
            ENDHLSL
        }
    }
}