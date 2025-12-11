Shader "WinterSpring/SnowGlobe"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _NormalTex("Normal Tex", 2D) = "white" {}
        _MaskTex("Mask Tex", 2D) = "white" {}
        _Normal_Panner("Wind Power",Range(-5, 5)) = 0
        _Normal_Strength("Normal Strength", Range(0, 5)) = 0
        _Wind_Direction("Wind Direction",Range(0,360)) = 0

        [Toggle]_Condition("Condition", Int) = 0
        [HDR]_AppareColor("Appare Color", Color) = (1,1,1,1)
        _ApparePow("Appare Power", Range(0,1)) = 0

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
                    TEXTURE2D(_NormalTex);
                    SAMPLER(sampler_NormalTex);
                    half4 _NormalTex_ST;

                    TEXTURE2D(_MaskTex);
                    SAMPLER(sampler_MaskTex);
                    half4 _MaskTex_ST;
                    float4 _Color;
                    int _Condition;
                    half4 _RendererColor;
                    float _Normal_Panner;
                    float _Normal_Strength;
                    float _Wind_Direction;
                    float4 _AppareColor;
                    half _ApparePow;

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
                        float2 uv_mask = i.uv * _MaskTex_ST.xy + _MaskTex_ST.zw;
                        float Radian = _Wind_Direction / 180 * PI;

                        float4 MaskTex = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, uv_mask);

                        float2 uv_Normal_Tex = i.uv * _NormalTex_ST.xy + _NormalTex_ST.zw;
                        float2 PannerPower = _Normal_Panner * (float2(-cos(Radian), -sin(Radian)));
                        float2 panner = _Time.y * PannerPower + uv_Normal_Tex;

                        float3 NormalTex = UnpackNormal(SAMPLE_TEXTURE2D(_NormalTex, sampler_NormalTex, panner));
                        float2 NormalOutput = NormalTex.xy * _Normal_Strength;

                        half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_main + NormalOutput * MaskTex.r);

                        half3 Tex = main.rgb;
                        
                        half Alpha = 0;

                        if (_Condition == 1)
                        {
                            main.rgb = lerp(_AppareColor.rgb, main.rgb, saturate(step(0.5, _ApparePow * 2 - MaskTex.r)));
                            Alpha = lerp(0, main.a, saturate((step(0.5, _ApparePow * 16 - MaskTex.r * 2))));
                        }

                        SurfaceData2D surfaceData;
                        InputData2D inputData;

                        InitializeSurfaceData(main.rgb, Alpha, 1, surfaceData);
                        InitializeInputData(i.uv, i.lightingUV, inputData);

                        return CombinedShapeLightShared(surfaceData, inputData);
                    }
                    ENDHLSL
                }


            }

                Fallback "Sprites/Default"
}
