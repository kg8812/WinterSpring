Shader "Universal Render Pipeline/2D/Custom/SpineNormalColorShader" {
    Properties{
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "black" {}
        [NoScaleOffset] _MaskTex("Mask", 2D) = "white" {}
        [NoScaleOffset] _NormalTex("Normal", 2D) = "Bump" {}

        [Enum(Off, 0, On, 1)] _DissolveSwitch("Dissolve_Switch", Int) = 0

        [Toggle(_STRAIGHT_ALPHA_INPUT)] _StraightAlphaInput("Straight Alpha Texture", Int) = 0

        [HDR] _HDRColor("Main Color", Color) = (1,1,1,1)

        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Compare", Float) = 8 // Set to Always as default
    }


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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
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

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            float4 _NormalTex_ST;
            int _DissolveSwitch;


            CBUFFER_END

            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _NormalTex;


            half4 _HDRColor;



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
                float FlowSpeed = 1;
                float2 MaskUV = i.uv - float2(0,_Time.x * FlowSpeed);
                float2 NewUV = i.uv + _Time.x * 5;
                float2 NormalUV = UnpackNormalScale(tex2D(_NormalTex, NewUV * 0.5f), 0.06f);
                half4 mask = lerp(0, tex2D(_MaskTex, i.uv), _DissolveSwitch);
                float4 Dissolve = tex2D(_MaskTex, MaskUV);
                float4 tex = tex2D(_MainTex, lerp(i.uv, i.uv + NormalUV, mask.b));
                tex.rgb *= lerp(1, saturate(0.8 - mask.b), mask.b);



                #if !defined(_STRAIGHT_ALPHA_INPUT)
                // un-premultiply for additive lights in CombinedShapeLightShared, reapply afterwards
                tex.rgb = tex.a == 0 ? tex.rgb : tex.rgb / tex.a;
                #endif
                half4 main = tex * i.color * _HDRColor;

                #if !defined(_LIGHT_AFFECTS_ADDITIVE)
                if (i.color.a == 0)
                    return half4(main.rgb * main.a, main.a);
                #endif



                SurfaceData2D surfaceData;
                InputData2D inputData;


                surfaceData.albedo = main.rgb;

                surfaceData.alpha = main.a;
                surfaceData.mask = 1;
                inputData.uv = i.uv;
                inputData.lightingUV = i.lightingUV;
                return half4(CombinedShapeLightShared(surfaceData, inputData).rgb * main.a, main.a * main.r);

            }

            ENDHLSL
        }


    }
            FallBack "Universal Render Pipeline/2D/Spine/Skeleton"
}
