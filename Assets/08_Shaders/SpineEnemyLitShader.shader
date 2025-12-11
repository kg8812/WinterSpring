Shader "Universal Render Pipeline/2D/Custom/EnemyLitShader" {
    Properties{
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "black" {}
        [NoScaleOffset] _MaskTex("Mask", 2D) = "white" {}
        [Toggle(_IS_MASKING)]_IsMasking("Is Masking", Int) = 0
        [HDR] _HDRColor("HDR Bright Power Color", Color) = (1,1,1,1)

        [Enum(Off, 0, On, 1)] _IsDead("Dead", Float) = 0
        [NoScaleOffset] _DissolveTex("Dead Resolve", 2D) = "white" {}
        //_DeadTime("Dead Time", Float) = 1
        _DeadEffectProcess("Dead Effect Process", Range(0, 1)) = 1


        [Enum(Off, 0, On, 1)] _IsHit("Hit", Float) = 0
        _HitEffectDistance("Hit Distance", Float) = 0.1
        [PowerSlider(3.0)] _HitEffectDistancePow("Hit Distance Power", Range(0.01, 1)) = 0.01


        [MaterialToggle]_IsIcy("Is Icy", Int) = 0
        [NoScaleOffset] _IcyTex("Icy Texture", 2D) = "white" {}
        _IcyPower("Icy Power", Range(0, 1)) = 0.5
        [HideInInspector] _StencilRef("Stencil Reference", Float) = 1.0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Compare", Float) = 8 // Set to Always as default
    }

        HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            ENDHLSL

            SubShader{
            // UniversalPipeline tag is required. If Universal render pipeline is not set in the graphics settings
            // this Subshader will fail.
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True" }
            Cull Off
            ZWrite Off

            Stencil {
                Ref[_StencilRef]
                Comp[_StencilComp]
                Pass Keep
            }

            Pass {
                Tags { "LightMode" = "Universal2D" }

                ZWrite Off
                Cull Off
                Blend One OneMinusSrcAlpha

                HLSLPROGRAM
            // Required to compile gles 2.0 with standard srp library
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #pragma shader_feature_local _IS_MASKING
            #pragma multi_compile _ISICY

            struct Attributes {
                float3 positionOS : POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;

            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR0;
                float2 uv : TEXCOORD0;
                float2 lightingUV : TEXCOORD1;

            };

            // Spine related keywords
            #pragma vertex CombinedShapeLightVertex
            #pragma fragment CombinedShapeLightFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
            #define USE_URP
            #include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Common.cginc"
            #include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Skeleton-Tint-Common.cginc"



            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
#if defined (_IS_MASKING)
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);

            half4 _HDRColor;
            int _HDRSwitch;


#endif


            int _IsDead;

            TEXTURE2D(_DissolveTex);
            SAMPLER(sampler_DissolveTex);
            float _DeadEffectProcess;

            int _IsHit;
            float _HitEffectDistance;
            float _HitEffectDistancePow;

            int _IsIcy;
            TEXTURE2D(_IcyTex);
            SAMPLER(sampler_IcyTex);
            half _IcyPower;


            #if USE_SHAPE_LIGHT_TYPE_0
            SHAPE_LIGHT(0)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_1
            SHAPE_LIGHT(1)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_2
            SHAPE_LIGHT(2)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_3
            SHAPE_LIGHT(3)
            #endif

            Varyings CombinedShapeLightVertex(Attributes v)
            {
                Varyings o = (Varyings)0;

                v.positionOS += float3(-1, 1, 0) * _IsHit * _HitEffectDistance * _HitEffectDistancePow * sin(_Time.y * 200);

                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                float4 clipVertex = o.positionCS / o.positionCS.w;
                o.lightingUV = ComputeScreenPos(clipVertex).xy;
                o.color = PMAGammaToTargetSpace(v.color);
                // un-premultiply for additive lights in CombinedShapeLightShared, reapply afterwards
                o.color.rgb = o.color.a == 0 ? o.color.rgb : o.color.rgb / o.color.a;

                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            half4 CombinedShapeLightFragment(Varyings i) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                tex.rgb = tex.a == 0 ? tex.rgb : tex.rgb / tex.a;

                half4 main = tex * i.color;


                if (i.color.a == 0)
                    return half4(main.rgb * main.a, main.a);

                if (_IsIcy == 1) {
                    float4 IcyTex = SAMPLE_TEXTURE2D(_IcyTex, sampler_IcyTex, i.uv);
                    main.rgb = (main.rgb + IcyTex.rgb) * 0.5;
                    main.rgb *= 1 + _IcyPower;
                }


#if defined (_IS_MASKING)
                half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
                main.rgb *= lerp(half4(1, 1, 1, 1), _HDRColor, mask.r).rgb;
#endif

                main.rgb = lerp(main.rgb, half3(1, 0.8, 0.8), _IsHit * _HitEffectDistancePow); // Hit effect

                half4 dissolveTex = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, i.uv);

                dissolveTex.r += _DeadEffectProcess;

                half3 dissolveLine = lerp(main.rgb, half3(1, 1, 1), step(0.9, dissolveTex.r));
                
                half dissolve = lerp(main.a, step(dissolveTex.r, 0.99),_IsDead);

                main.rgb = lerp(main.rgb, dissolveLine, _IsDead);

                SurfaceData2D surfaceData;
                InputData2D inputData;
                surfaceData.albedo = main.rgb;
                surfaceData.alpha = dissolve;
                surfaceData.mask = 1;
                inputData.uv = i.uv;
                inputData.lightingUV = i.lightingUV;
                return half4(CombinedShapeLightShared(surfaceData, inputData).rgb * main.a, main.a);
            }

            ENDHLSL
        }
        }
            FallBack "Universal Render Pipeline/2D/Spine/Skeleton"
}
