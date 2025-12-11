Shader "WinterSpring/IcedObject"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}

        [MaterialToggle]_IsIcy("Is Icy", Int) = 0
        [NoScaleOffset] _IcyTex("Icy Texture", 2D) = "white" {}
        _IcyPower("Icy Power", Range(0, 1)) = 0.5

            // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
            [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
            [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
            [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
            [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
            [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

        SubShader
            {
                Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                Cull Off
                ZWrite Off

                Pass
                {
                    Tags { "LightMode" = "Universal2D" }

                    HLSLPROGRAM
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                    #pragma vertex CombinedShapeLightVertex
                    #pragma fragment CombinedShapeLightFragment

                    #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
                    #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
                    #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
                    #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
                    #pragma multi_compile _ DEBUG_DISPLAY

                    struct Attributes
                    {
                        float3 positionOS   : POSITION;
                        float4 color        : COLOR;
                        float2  uv          : TEXCOORD0;
                        UNITY_VERTEX_INPUT_INSTANCE_ID
                    };

                    struct Varyings
                    {
                        float4  positionCS  : SV_POSITION;
                        half4   color       : COLOR;
                        float2  uv          : TEXCOORD0;
                        half2   lightingUV  : TEXCOORD1;
                        #if defined(DEBUG_DISPLAY)
                        float3  positionWS  : TEXCOORD2;
                        #endif
                        UNITY_VERTEX_OUTPUT_STEREO
                    };

                    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

                    TEXTURE2D(_MainTex);
                    SAMPLER(sampler_MainTex);
                    half4 _MainTex_ST;

                    int _IsIcy;
                    TEXTURE2D(_IcyTex);
                    SAMPLER(sampler_IcyTex);
                    half _IcyPower;

                    float4 _Color;
                    half4 _RendererColor;
                    float _Normal_Panner;
                    float _Normal_Strength;
                    float _Wind_Direction;

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
                        UNITY_SETUP_INSTANCE_ID(v);
                        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                        o.positionCS = TransformObjectToHClip(v.positionOS);
                        #if defined(DEBUG_DISPLAY)
                        o.positionWS = TransformObjectToWorld(v.positionOS);
                        #endif
                        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                        o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);

                        o.color = v.color * _Color * _RendererColor;
                        return o;
                    }

                    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

                    half4 CombinedShapeLightFragment(Varyings i) : SV_Target
                    {

                        float2 uv_main = i.uv * _MainTex_ST.xy + _MainTex_ST.zw;

                        half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_main);

                        if (_IsIcy == 1) {
                            float4 IcyTex = SAMPLE_TEXTURE2D(_IcyTex, sampler_IcyTex, i.uv);
                            main.rgb = (main.rgb + IcyTex.rgb) * 0.5;
                            main.rgb *= 1 + _IcyPower;
                        }                   

                        SurfaceData2D surfaceData;
                        InputData2D inputData;

                        InitializeSurfaceData(main.rgb, main.a, 1, surfaceData);
                        InitializeInputData(i.uv, i.lightingUV, inputData);

                        return CombinedShapeLightShared(surfaceData, inputData);
                    }
                    ENDHLSL
                }


            }

                Fallback "Sprites/Default"
}
