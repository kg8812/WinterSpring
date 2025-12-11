Shader "Universal Render Pipeline/2D/Custom/JururuBoss_Lit"
{
    Properties
    {
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "black" {}
        [NoScaleOffset] _MaskTex("Mask", 2D) = "white" {}
        [NoScaleOffset] _NormalTex("Normal", 2D) = "Bump" {}
        [Enum(Off, 0, On, 1)] _DissolveSwitch("Dissolve_Switch", Int) = 0
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

            Blend One OneMinusSrcAlpha
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


                CBUFFER_START(UnityPerMaterial)

                float4 _MainTex_ST;
                float4 _MaskTex_ST;
                float4 _NormalTex_ST;
                int _DissolveSwitch;


                CBUFFER_END

                sampler2D _MainTex;
                sampler2D _MaskTex;
                sampler2D _NormalTex;

                float4 _Color;
                half4 _RendererColor;

                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
                //#include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Common.cginc"


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
                    float FlowSpeed = 1;
                    float2 MaskUV = i.uv - float2(0,_Time.x * FlowSpeed);
                    float2 NewUV = i.uv + _Time.x * 5;
                    float2 NormalUV = UnpackNormalScale(tex2D(_NormalTex, NewUV * 0.5f), 0.06f);
                    half4 mask = lerp(0, tex2D(_MaskTex, i.uv), _DissolveSwitch);
                    float4 Dissolve = tex2D(_MaskTex, MaskUV);
                    float4 texColor = tex2D(_MainTex, lerp(i.uv, i.uv + NormalUV, mask.b));
                    texColor.rgb *= lerp(1, saturate(0.8 - mask.b), mask.b);
                    texColor.rgb *= lerp(1, Dissolve.g * float3(1.5, 1, 1), mask.r);

                    SurfaceData2D surfaceData;
                    InputData2D inputData;

                    InitializeSurfaceData(texColor.rgb, texColor.a, surfaceData);
                    InitializeInputData(i.uv, i.lightingUV, inputData);

                    return CombinedShapeLightShared(surfaceData, inputData) * i.color;
                }
                ENDHLSL
            }

        }

            Fallback "Sprites/Default"
}
