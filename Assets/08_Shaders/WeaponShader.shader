Shader "Universal Render Pipeline/2D/Custom/Weapon"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _DissolveTex("Dissolve Texture", 2D) = "white" {}

        [NoScaleOffset] _MaskTex("Mask", 2D) = "white" {}
        [HDR] _HDRColor("HDR Bright Power Color", Color) = (1,1,1,1)
        [KeywordEnum(Normal, Bright, Luminescence)] _HDRSwitch("HDR_Switch", Int) = 0


        [KeywordEnum(Appare, Normal, Disappare)] _Condition("Condition", Int) = 0
        [HDR]_AppareColor("Appare Color", Color) = (1,1,1,1)
        [HDR]_DisappareColor("Disappare Color", Color) = (1,1,1,1)
        [HideInInspector]_ApparePow("Appare Power", Range(0,1)) = 0
        [HideInInspector]_DisapparePow("Disappare Power", Range(0,1)) = 0

    // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
    [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
    [HideInInspector] PixelSnap("Pixel snap", Float) = 0
    [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
    [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
    [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
    [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

        SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #if defined(DEBUG_DISPLAY)
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
            #endif

            #pragma vertex UnlitVertex
            #pragma fragment UnlitFragment

            #pragma multi_compile _ DEBUG_DISPLAY
            #pragma multi_compile _CONDITION_APPARE _CONDITION_NORMAL _CONDITION_DISAPPARE

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4  positionCS  : SV_POSITION;
                half4   color       : COLOR;
                float2  uv          : TEXCOORD0;
                #if defined(DEBUG_DISPLAY)
                float3  positionWS  : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);
            half4 _MaskTex_ST;
            TEXTURE2D(_DissolveTex);
            SAMPLER(sampler_DissolveTex);
            half4 _DissolveTex_ST;
            float4 _Color;
            half4 _RendererColor;
            half4 _HDRColor;
            int _HDRSwitch;
            int _Condition;
            half _ApparePow;
            half _DisapparePow;
            float4 _AppareColor;
            float4 _DisappareColor;

            Varyings UnlitVertex(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.positionCS = TransformObjectToHClip(v.positionOS);
                #if defined(DEBUG_DISPLAY)
                o.positionWS = TransformObjectToWorld(v.positionOS);
                #endif
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color * _RendererColor;
                return o;
            }

            half4 UnlitFragment(Varyings i) : SV_Target
            {

                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                half4 mask = lerp(0, SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv), saturate(_HDRSwitch));

                mainTex *= lerp(1, _HDRColor + lerp(0, sin(_Time.y * 2) * 0.5 + 0.5, _HDRSwitch - 1), mask.r);
                
                if (_Condition == 0)
                {
                    mainTex.rgb = lerp(_AppareColor.rgb, mainTex.rgb, _ApparePow);
                }
                else if (_Condition == 2) {
                    float4 DissolveTex = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, i.uv);

                    mainTex.rgb = lerp(mainTex.rgb, _DisappareColor.rgb, saturate(_DisapparePow * 8));

                    mainTex.a *= step(_DisapparePow * 1.001 + DissolveTex.r, mainTex.a);
                }


                #if defined(DEBUG_DISPLAY)
                SurfaceData2D surfaceData;
                InputData2D inputData;
                half4 debugColor = 0;

                InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
                InitializeInputData(i.uv, inputData);
                SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

                if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
                {
                    return debugColor;
                }
                #endif

                return mainTex;
            }
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderType" = "Transparent"}

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #if defined(DEBUG_DISPLAY)
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
            #endif

            #pragma vertex UnlitVertex
            #pragma fragment UnlitFragment

            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4  positionCS      : SV_POSITION;
                float4  color           : COLOR;
                float2  uv              : TEXCOORD0;
                #if defined(DEBUG_DISPLAY)
                float3  positionWS      : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;
            half4 _RendererColor;

            Varyings UnlitVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(attributes);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                #if defined(DEBUG_DISPLAY)
                o.positionWS = TransformObjectToWorld(attributes.positionOS);
                #endif
                o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
                o.color = attributes.color * _Color * _RendererColor;
                return o;
            }

            float4 UnlitFragment(Varyings i) : SV_Target
            {
                float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                #if defined(DEBUG_DISPLAY)
                SurfaceData2D surfaceData;
                InputData2D inputData;
                half4 debugColor = 0;

                InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
                InitializeInputData(i.uv, inputData);
                SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

                if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
                {
                    return debugColor;
                }
                #endif

                return mainTex;
            }
            ENDHLSL
        }
    }

        Fallback "Sprites/Default"
}
