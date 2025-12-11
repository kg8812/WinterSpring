Shader "Universal Render Pipeline/2D/Custom/JururuBoss" {
    Properties{
        _Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.1
        [NoScaleOffset] _MainTex("Main Texture", 2D) = "black" {}
        [NoScaleOffset] _MaskTex("Mask", 2D) = "white" {}
        [NoScaleOffset] _NormalTex("Normal", 2D) = "Bump" {}
        [Enum(Off, 0, On, 1)] _DissolveSwitch("Dissolve_Switch", Int) = 0
        [Toggle(_STRAIGHT_ALPHA_INPUT)] _StraightAlphaInput("Straight Alpha Texture", Int) = 0
        [NoScaleOffset] _IcyTex("Icy Texture", 2D) = "white" {}
        [MaterialToggle]_IsIcy("Is Icy", Int) = 0
        _IcyPower("Icy Power", Range(0, 1)) = 0.5
        [HideInInspector] _StencilRef("Stencil Reference", Float) = 1.0
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp("Stencil Comparison", Float) = 8 // Set to Always as default
    }

        SubShader{
            // Universal Pipeline tag is required. If Universal render pipeline is not set in the graphics settings
            // this Subshader will fail.
            Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 100
            Cull Off
            ZWrite Off
            Blend One OneMinusSrcAlpha

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

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing

            //--------------------------------------
            // Spine related keywords
            #pragma shader_feature _ _STRAIGHT_ALPHA_INPUT
            #pragma vertex vert
            #pragma fragment frag

            #undef LIGHTMAP_ON

            #define USE_URP
            #define fixed4 half4
            #define fixed3 half3
            #define fixed half


            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

            ////////////////////////////////////////
            // Defines
            //
            #undef LIGHTMAP_ON

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            float4 _NormalTex_ST;
            half _Cutoff;
            half4 _Color;
            half4 _Black;

            CBUFFER_END

            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _NormalTex;
            int _DissolveSwitch;
            int _IsIcy;

            TEXTURE2D(_IcyTex);
            SAMPLER(sampler_IcyTex);
            half _IcyPower;



            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Common.cginc"
            #include "Packages/com.esotericsoftware.spine.urp-shaders/Shaders/Include/SpineCoreShaders/Spine-Skeleton-Tint-Common.cginc"

            struct appdata {
                float3 pos : POSITION;
                half4 color : COLOR;
                float2 uv0 : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput {
                half4 color : COLOR0;
                float2 uv0 : TEXCOORD0;
                float4 pos : SV_POSITION;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            VertexOutput vert(appdata v) {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float3 positionWS = TransformObjectToWorld(v.pos);
                o.pos = TransformWorldToHClip(positionWS);
                o.uv0 = v.uv0;
                o.color = PMAGammaToTargetSpace(v.color);

                return o;
            }

            half4 frag(VertexOutput i) : SV_Target{
                float FlowSpeed = 1;
                float2 MaskUV = i.uv0 - float2(0,_Time.x * FlowSpeed);
                float2 NewUV = i.uv0 + _Time.x * 5;
                float2 NormalUV = UnpackNormalScale(tex2D(_NormalTex, NewUV * 0.5f), 0.06f);
                half4 mask = lerp(0, tex2D(_MaskTex, i.uv0), _DissolveSwitch);
                float4 Dissolve = tex2D(_MaskTex, MaskUV);
                float4 texColor = tex2D(_MainTex, lerp(i.uv0, i.uv0 + NormalUV, mask.b));

                texColor.rgb *= lerp(1, saturate(0.8 - mask.b), mask.b);
                texColor.rgb *= lerp(1, Dissolve.g * float3(1.5, 1, 1), mask.r);


                if (_IsIcy == 1) {
                    float4 IcyTex = SAMPLE_TEXTURE2D(_IcyTex, sampler_IcyTex, i.uv0);
                    texColor.rgb = lerp(texColor.rgb, IcyTex.rgb, texColor.r);
                    texColor.rgb *= 1 + _IcyPower;
                }

                return (texColor * i.color);

            }
            ENDHLSL
        }
        }

            FallBack "Universal Render Pipeline/2D/Sprite-Unlit-Default"
}