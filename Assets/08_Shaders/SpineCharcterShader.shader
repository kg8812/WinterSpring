Shader "Universal Render Pipeline/2D/Custom/SpineCustomLit" {
    Properties{
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "black" {}
        [NoScaleOffset] _MaskTex("Mask", 2D) = "white" {}
        [Toggle(_STRAIGHT_ALPHA_INPUT)] _StraightAlphaInput("Straight Alpha Texture", Int) = 0
        [MaterialToggle(_LIGHT_AFFECTS_ADDITIVE)] _LightAffectsAdditive("Light Affects Additive", Float) = 0
        [Enum(Off, 0, On, 1)] _IsHit("Hit", Float) = 0
        _HitEffectDistance("Hit Distance", Float) = 0.1
        [PowerSlider(3.0)] _HitEffectDistancePow("Hit Distance Power", Range(0.01, 1)) = 0.01
        [Enum(Off, 0, On, 1)] _InvincibleSwitch("Invincible", Float) = 0
        _InvincibleWaveSpeed("Invincible Wave Speed", Float) = 20
        [HideInInspector] _StencilRef("Stencil Reference", Float) = 1.0
        [HDR] _HDRColor("Rim Light Power Color", Color) = (1,1,1,1)
        [Enum(Off, 0, On, 1)] _HDRSwitch("HDR_Switch", Int) = 0
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
            #pragma multi_compile _ _LIGHT_AFFECTS_ADDITIVE

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
            #if defined(_TINT_BLACK_ON)
                float3 darkColor : TEXCOORD2;
            #endif
            };

            // Spine related keywords
            #pragma shader_feature _ _STRAIGHT_ALPHA_INPUT
            #pragma vertex CombinedShapeLightVertex
            #pragma fragment CombinedShapeLightFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
            #define USE_URP
            #include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Common.cginc"
            #include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Skeleton-Tint-Common.cginc"


            half4 _HDRColor;
            
            int _HDRSwitch;
            int _IsHit;
            float _HitEffectDistance;
            float _HitEffectDistancePow;
            int _InvincibleSwitch;
            float _InvincibleWaveSpeed;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);

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

                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            half4 CombinedShapeLightFragment(Varyings i) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                #if !defined(_STRAIGHT_ALPHA_INPUT)
                // un-premultiply for additive lights in CombinedShapeLightShared, reapply afterwards
                tex.rgb = tex.a == 0 ? tex.rgb : tex.rgb / tex.a;
                #endif
                half4 main = tex * i.color;

                #if !defined(_LIGHT_AFFECTS_ADDITIVE)
                if (i.color.a == 0)
                    return half4(main.rgb * main.a, main.a);
                #endif

                half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);

                SurfaceData2D surfaceData;
                InputData2D inputData;

                main.rgb = lerp(main.rgb, half3(1, 0.8, 0.8), _IsHit * _HitEffectDistancePow); // Hit effect
                main = lerp(main , lerp(main, half4(1, 1, 1, 0), sin(_Time.y * _InvincibleWaveSpeed) * 0.25 + 0.25), _InvincibleSwitch); //invincible effect

                surfaceData.albedo = main.rgb;

                surfaceData.alpha = main.a;
                surfaceData.mask = mask * lerp(half4(1,1,1,1), _HDRColor, _HDRSwitch);
                inputData.uv = i.uv;
                inputData.lightingUV = i.lightingUV;
                return half4(CombinedShapeLightShared(surfaceData, inputData).rgb * main.a, main.a);

            }

            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "NormalsRendering"}

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma vertex NormalsRenderingVertex
            #pragma fragment NormalsRenderingFragment

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color		: COLOR;
                float2 uv			: TEXCOORD0;
            };

            struct Varyings
            {
                float4  positionCS		: SV_POSITION;
                float4  color			: COLOR;
                float2	uv				: TEXCOORD0;
                float3  normalWS		: TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings NormalsRenderingVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                o.uv = attributes.uv;
                o.color = attributes.color;
                o.normalWS = TransformObjectToWorldDir(float3(0, 0, -1));
                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

            float4 NormalsRenderingFragment(Varyings i) : SV_Target
            {
                float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                half3 normalTS = half3(0, 0, 1);
                half3 tangentWS = half3(0, 0, 0);
                half3 bitangentWS = half3(0, 0, 0);
                return NormalsRenderingShared(mainTex, normalTS, tangentWS, bitangentWS, i.normalWS);
            }
            ENDHLSL
        }

        }
            FallBack "Universal Render Pipeline/2D/Spine/Skeleton"
}
