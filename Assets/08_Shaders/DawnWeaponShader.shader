Shader "Universal Render Pipeline/2D/Custom/DawnWeapon"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _DissolveTex("Dissolve Texture", 2D) = "white" {}
        _NormalTex("Normal Tex", 2D) = "white" {}
        _Normal_Panner("Normal panner",Range(-5, 5)) = 0
        _Normal_Strength("Normal_Strength", Range(0, 5)) = 0


        [KeywordEnum(Appare, Normal, Disappare)] _Condition("Condition", Int) = 0
        [HDR]_AppareColor("Appare Color", Color) = (1,1,1,1)
        [HDR]_DisappareColor("Disappare Color", Color) = (1,1,1,1)
        [HideInInspector]_ApparePow("Appare Power", Range(0,1)) = 0
        [HideInInspector]_DisapparePow("Disappare Power", Range(0,1)) = 0


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
            TEXTURE2D(_DissolveTex);
            SAMPLER(sampler_DissolveTex);
            half4 _DissolveTex_ST;
            TEXTURE2D(_NormalTex);
            SAMPLER(sampler_NormalTex);
            half4 _NormalTex_ST;
            float4 _Color;
            half4 _RendererColor;
            float _Normal_Panner;
            float _Normal_Strength;
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
                float2 uv_main = i.uv * _MainTex_ST.xy + _MainTex_ST.zw;

                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_main);


                float2 uv_Normal_Tex = i.uv * _NormalTex_ST.xy + _NormalTex_ST.zw;
                float2 PannerPower = (float2(_Normal_Panner, 0));
                float2 panner = float2(-1, 0) * _Time.y * PannerPower + uv_Normal_Tex;


                float3 NormalTex = UnpackNormal(SAMPLE_TEXTURE2D(_NormalTex, sampler_NormalTex, panner));
                float2 NormalOutput = NormalTex.xy * _Normal_Strength;

                float4 addNormalMainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv_main + NormalOutput * mainTex.g);

                //mainTex.a = lerp( addNormalMainTex, mainTex, mainTex.g);
                mainTex.a *= step(0.8, mainTex.g);

                mainTex = max(addNormalMainTex, mainTex);

                if (_Condition == 0)
                {
                    mainTex.rgb = lerp(_AppareColor.rgb, mainTex.rgb, _ApparePow);
                }
                else if (_Condition == 2) {
                    float4 DissolveTex = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, i.uv);

                    mainTex.rgb = lerp(mainTex.rgb, _DisappareColor.rgb, saturate(_DisapparePow * 8));

                    mainTex.a *= step(_DisapparePow * 1.001 + DissolveTex.r, mainTex.a);
                }


                return mainTex;
            }
            ENDHLSL
        }

    }

}
