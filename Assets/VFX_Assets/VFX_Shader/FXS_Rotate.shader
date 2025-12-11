// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify_Shader/Vfx/FXS_Rotate"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_SRC("SRC", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_DST("DST", Float) = 0
		_Rotate("Rotate", Float) = 0.93
		[Toggle(_USE_ROTATE_ON)] _USE_Rotate("USE_Rotate", Float) = 0
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Power("Main_Power", Float) = 1
		_Main_Ins("Main_Ins", Float) = 1
		[Toggle(_USE_INS_CUSTOM_ON)] _USE_INS_Custom("USE_INS_Custom", Float) = 0
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,1)
		_Main_Upanner("Main_Upanner", Float) = 0
		_Main_Vpanner("Main_Vpanner", Float) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		[Toggle(_USE_MASK_ON)] _USE_Mask("USE_Mask", Float) = 0
		[Toggle(_MASK_CUSTOM_ON)] _Mask_Custom("Mask_Custom", Float) = 0
		_Mask_U_Panner("Mask_U_Panner", Float) = 0
		_Mask_V_Panner("Mask_V_Panner", Float) = 0
		_Dissolve_Texture("Dissolve_Texture", 2D) = "white" {}
		_DIsoolve("DIsoolve", Range( -1 , 1)) = 1
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Dissolve_Custom", Float) = 0
		[Toggle(_USE_SMOOTHSTEP_ON)] _USE_SmoothStep("USE_SmoothStep", Float) = 1
		_smoothness("smoothness", Range( 0 , 1)) = 1
		_Dissolve_Upanner("Dissolve_Upanner", Float) = 0
		_Dissolve_Vpanner("Dissolve_Vpanner", Float) = 0

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull [_CullMode]
		HLSLINCLUDE
		#pragma target 2.0
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
		ENDHLSL

		
		Pass
		{
			Name "Sprite Unlit"
			Tags { "LightMode"="Universal2D" }

			Blend [_SRC] [_DST]
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 120110


			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS SHADERPASS_SPRITEUNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USE_ROTATE_ON
			#pragma shader_feature_local _USE_INS_CUSTOM_ON
			#pragma shader_feature_local _USE_MASK_ON
			#pragma shader_feature_local _MASK_CUSTOM_ON
			#pragma shader_feature_local _USE_SMOOTHSTEP_ON
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Tex;
			sampler2D _Mask_Tex;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Main_Color;
			float _CullMode;
			float _Dissolve_Vpanner;
			float _Dissolve_Upanner;
			float _DIsoolve;
			float _Mask_V_Panner;
			float _Mask_U_Panner;
			float _Main_Ins;
			float _Rotate;
			float _Main_Vpanner;
			float _Main_Upanner;
			float _DST;
			float _SRC;
			float _Main_Power;
			float _smoothness;
			CBUFFER_END


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord3 = v.ase_texcoord1;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 appendResult50 = (float2(_Main_Upanner , _Main_Vpanner));
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Main_Tex);
				float2 temp_cast_0 = (0.0).xx;
				float2 texCoord109 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult113 = (float2(( texCoord109.x + ( texCoord109.y * _Rotate ) ) , texCoord109.y));
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch178 = appendResult113;
				#else
				float2 staticSwitch178 = temp_cast_0;
				#endif
				float4 tex2DNode53 = tex2D( _Main_Tex, ( panner52 + staticSwitch178 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				#ifdef _USE_INS_CUSTOM_ON
				float staticSwitch118 = IN.ase_texcoord3.x;
				#else
				float staticSwitch118 = _Main_Ins;
				#endif
				float4 break66 = ( ( ( pow( tex2DNode53 , temp_cast_1 ) * staticSwitch118 ) * _Main_Color ) * IN.color );
				float2 appendResult164 = (float2(_Mask_U_Panner , _Mask_V_Panner));
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 panner166 = ( 1.0 * _Time.y * appendResult164 + uv_Mask_Tex);
				#ifdef _MASK_CUSTOM_ON
				float2 staticSwitch168 = ( IN.texCoord0.z + uv_Mask_Tex );
				#else
				float2 staticSwitch168 = panner166;
				#endif
				#ifdef _USE_MASK_ON
				float staticSwitch171 = tex2D( _Mask_Tex, staticSwitch168 ).r;
				#else
				float staticSwitch171 = 1.0;
				#endif
				float2 temp_cast_2 = (0.0).xx;
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch184 = appendResult113;
				#else
				float2 staticSwitch184 = temp_cast_2;
				#endif
				float2 appendResult148 = (float2(_Dissolve_Upanner , _Dissolve_Vpanner));
				float2 uv_Dissolve_Texture = IN.texCoord0.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner149 = ( 1.0 * _Time.y * appendResult148 + uv_Dissolve_Texture);
				float4 tex2DNode150 = tex2D( _Dissolve_Texture, ( staticSwitch184 + panner149 ) );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch152 = ( IN.ase_texcoord3.y + tex2DNode150.r );
				#else
				float staticSwitch152 = _DIsoolve;
				#endif
				float temp_output_153_0 = ( 1.0 - staticSwitch152 );
				float smoothstepResult156 = smoothstep( temp_output_153_0 , ( temp_output_153_0 + _smoothness ) , tex2DNode150.r);
				#ifdef _USE_SMOOTHSTEP_ON
				float staticSwitch158 = saturate( smoothstepResult156 );
				#else
				float staticSwitch158 = staticSwitch152;
				#endif
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( saturate( ( ( staticSwitch171 * tex2DNode53.r ) * staticSwitch158 ) ) * IN.color.a )));
				
				float4 Color = appendResult67;

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif

				Color *= IN.color * _RendererColor;
				return Color;
			}

			ENDHLSL
		}
		
		Pass
		{
			
			Name "Sprite Unlit Forward"
            Tags { "LightMode"="UniversalForward" }

			Blend [_SRC] [_DST]
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 120110


			#pragma vertex vert
			#pragma fragment frag

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS SHADERPASS_SPRITEFORWARD

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USE_ROTATE_ON
			#pragma shader_feature_local _USE_INS_CUSTOM_ON
			#pragma shader_feature_local _USE_MASK_ON
			#pragma shader_feature_local _MASK_CUSTOM_ON
			#pragma shader_feature_local _USE_SMOOTHSTEP_ON
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Tex;
			sampler2D _Mask_Tex;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Main_Color;
			float _CullMode;
			float _Dissolve_Vpanner;
			float _Dissolve_Upanner;
			float _DIsoolve;
			float _Mask_V_Panner;
			float _Mask_U_Panner;
			float _Main_Ins;
			float _Rotate;
			float _Main_Vpanner;
			float _Main_Upanner;
			float _DST;
			float _SRC;
			float _Main_Power;
			float _smoothness;
			CBUFFER_END


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord3 = v.ase_texcoord1;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.positionOS.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 appendResult50 = (float2(_Main_Upanner , _Main_Vpanner));
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Main_Tex);
				float2 temp_cast_0 = (0.0).xx;
				float2 texCoord109 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult113 = (float2(( texCoord109.x + ( texCoord109.y * _Rotate ) ) , texCoord109.y));
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch178 = appendResult113;
				#else
				float2 staticSwitch178 = temp_cast_0;
				#endif
				float4 tex2DNode53 = tex2D( _Main_Tex, ( panner52 + staticSwitch178 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				#ifdef _USE_INS_CUSTOM_ON
				float staticSwitch118 = IN.ase_texcoord3.x;
				#else
				float staticSwitch118 = _Main_Ins;
				#endif
				float4 break66 = ( ( ( pow( tex2DNode53 , temp_cast_1 ) * staticSwitch118 ) * _Main_Color ) * IN.color );
				float2 appendResult164 = (float2(_Mask_U_Panner , _Mask_V_Panner));
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 panner166 = ( 1.0 * _Time.y * appendResult164 + uv_Mask_Tex);
				#ifdef _MASK_CUSTOM_ON
				float2 staticSwitch168 = ( IN.texCoord0.z + uv_Mask_Tex );
				#else
				float2 staticSwitch168 = panner166;
				#endif
				#ifdef _USE_MASK_ON
				float staticSwitch171 = tex2D( _Mask_Tex, staticSwitch168 ).r;
				#else
				float staticSwitch171 = 1.0;
				#endif
				float2 temp_cast_2 = (0.0).xx;
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch184 = appendResult113;
				#else
				float2 staticSwitch184 = temp_cast_2;
				#endif
				float2 appendResult148 = (float2(_Dissolve_Upanner , _Dissolve_Vpanner));
				float2 uv_Dissolve_Texture = IN.texCoord0.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner149 = ( 1.0 * _Time.y * appendResult148 + uv_Dissolve_Texture);
				float4 tex2DNode150 = tex2D( _Dissolve_Texture, ( staticSwitch184 + panner149 ) );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch152 = ( IN.ase_texcoord3.y + tex2DNode150.r );
				#else
				float staticSwitch152 = _DIsoolve;
				#endif
				float temp_output_153_0 = ( 1.0 - staticSwitch152 );
				float smoothstepResult156 = smoothstep( temp_output_153_0 , ( temp_output_153_0 + _smoothness ) , tex2DNode150.r);
				#ifdef _USE_SMOOTHSTEP_ON
				float staticSwitch158 = saturate( smoothstepResult156 );
				#else
				float staticSwitch158 = staticSwitch152;
				#endif
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( saturate( ( ( staticSwitch171 * tex2DNode53.r ) * staticSwitch158 ) ) * IN.color.a )));
				
				float4 Color = appendResult67;

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif


				#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(IN.positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, IN.positionWS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
				#endif

				Color *= IN.color * _RendererColor;
				return Color;
			}

			ENDHLSL
		}
		
        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }

            Cull Off

            HLSLPROGRAM

			#define ASE_SRP_VERSION 120110


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENESELECTIONPASS 1


            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USE_ROTATE_ON
			#pragma shader_feature_local _USE_INS_CUSTOM_ON
			#pragma shader_feature_local _USE_MASK_ON
			#pragma shader_feature_local _MASK_CUSTOM_ON
			#pragma shader_feature_local _USE_SMOOTHSTEP_ON
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Tex;
			sampler2D _Mask_Tex;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Main_Color;
			float _CullMode;
			float _Dissolve_Vpanner;
			float _Dissolve_Upanner;
			float _DIsoolve;
			float _Mask_V_Panner;
			float _Mask_U_Panner;
			float _Main_Ins;
			float _Rotate;
			float _Main_Vpanner;
			float _Main_Upanner;
			float _DST;
			float _SRC;
			float _Main_Power;
			float _smoothness;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};


            int _ObjectId;
            int _PassValue;

			
			VertexOutput vert(VertexInput v )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				float3 positionWS = TransformObjectToWorld(v.positionOS);
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				float2 appendResult50 = (float2(_Main_Upanner , _Main_Vpanner));
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Main_Tex);
				float2 temp_cast_0 = (0.0).xx;
				float2 texCoord109 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult113 = (float2(( texCoord109.x + ( texCoord109.y * _Rotate ) ) , texCoord109.y));
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch178 = appendResult113;
				#else
				float2 staticSwitch178 = temp_cast_0;
				#endif
				float4 tex2DNode53 = tex2D( _Main_Tex, ( panner52 + staticSwitch178 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				#ifdef _USE_INS_CUSTOM_ON
				float staticSwitch118 = IN.ase_texcoord1.x;
				#else
				float staticSwitch118 = _Main_Ins;
				#endif
				float4 break66 = ( ( ( pow( tex2DNode53 , temp_cast_1 ) * staticSwitch118 ) * _Main_Color ) * IN.ase_color );
				float2 appendResult164 = (float2(_Mask_U_Panner , _Mask_V_Panner));
				float2 uv_Mask_Tex = IN.ase_texcoord.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 panner166 = ( 1.0 * _Time.y * appendResult164 + uv_Mask_Tex);
				#ifdef _MASK_CUSTOM_ON
				float2 staticSwitch168 = ( IN.ase_texcoord.z + uv_Mask_Tex );
				#else
				float2 staticSwitch168 = panner166;
				#endif
				#ifdef _USE_MASK_ON
				float staticSwitch171 = tex2D( _Mask_Tex, staticSwitch168 ).r;
				#else
				float staticSwitch171 = 1.0;
				#endif
				float2 temp_cast_2 = (0.0).xx;
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch184 = appendResult113;
				#else
				float2 staticSwitch184 = temp_cast_2;
				#endif
				float2 appendResult148 = (float2(_Dissolve_Upanner , _Dissolve_Vpanner));
				float2 uv_Dissolve_Texture = IN.ase_texcoord.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner149 = ( 1.0 * _Time.y * appendResult148 + uv_Dissolve_Texture);
				float4 tex2DNode150 = tex2D( _Dissolve_Texture, ( staticSwitch184 + panner149 ) );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch152 = ( IN.ase_texcoord1.y + tex2DNode150.r );
				#else
				float staticSwitch152 = _DIsoolve;
				#endif
				float temp_output_153_0 = ( 1.0 - staticSwitch152 );
				float smoothstepResult156 = smoothstep( temp_output_153_0 , ( temp_output_153_0 + _smoothness ) , tex2DNode150.r);
				#ifdef _USE_SMOOTHSTEP_ON
				float staticSwitch158 = saturate( smoothstepResult156 );
				#else
				float staticSwitch158 = staticSwitch152;
				#endif
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( saturate( ( ( staticSwitch171 * tex2DNode53.r ) * staticSwitch158 ) ) * IN.ase_color.a )));
				
				float4 Color = appendResult67;

				half4 outColor = half4(_ObjectId, _PassValue, 1.0, 1.0);
				return outColor;
			}

            ENDHLSL
        }

		
        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

            Cull Off

            HLSLPROGRAM

			#define ASE_SRP_VERSION 120110


			#pragma vertex vert
			#pragma fragment frag

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX
            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENEPICKINGPASS 1


            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        	#define ASE_NEEDS_FRAG_COLOR
        	#pragma shader_feature_local _USE_ROTATE_ON
        	#pragma shader_feature_local _USE_INS_CUSTOM_ON
        	#pragma shader_feature_local _USE_MASK_ON
        	#pragma shader_feature_local _MASK_CUSTOM_ON
        	#pragma shader_feature_local _USE_SMOOTHSTEP_ON
        	#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Tex;
			sampler2D _Mask_Tex;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Main_Color;
			float _CullMode;
			float _Dissolve_Vpanner;
			float _Dissolve_Upanner;
			float _DIsoolve;
			float _Mask_V_Panner;
			float _Mask_U_Panner;
			float _Main_Ins;
			float _Rotate;
			float _Main_Vpanner;
			float _Main_Upanner;
			float _DST;
			float _SRC;
			float _Main_Power;
			float _smoothness;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            float4 _SelectionID;

			
			VertexOutput vert(VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord = v.ase_texcoord;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS.xyz = vertexValue;
				#else
					v.positionOS.xyz += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS.xyz);
				float3 positionWS = TransformObjectToWorld(v.positionOS);
				o.positionCS = TransformWorldToHClip(positionWS);

				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				float2 appendResult50 = (float2(_Main_Upanner , _Main_Vpanner));
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Main_Tex);
				float2 temp_cast_0 = (0.0).xx;
				float2 texCoord109 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult113 = (float2(( texCoord109.x + ( texCoord109.y * _Rotate ) ) , texCoord109.y));
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch178 = appendResult113;
				#else
				float2 staticSwitch178 = temp_cast_0;
				#endif
				float4 tex2DNode53 = tex2D( _Main_Tex, ( panner52 + staticSwitch178 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				#ifdef _USE_INS_CUSTOM_ON
				float staticSwitch118 = IN.ase_texcoord1.x;
				#else
				float staticSwitch118 = _Main_Ins;
				#endif
				float4 break66 = ( ( ( pow( tex2DNode53 , temp_cast_1 ) * staticSwitch118 ) * _Main_Color ) * IN.ase_color );
				float2 appendResult164 = (float2(_Mask_U_Panner , _Mask_V_Panner));
				float2 uv_Mask_Tex = IN.ase_texcoord.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 panner166 = ( 1.0 * _Time.y * appendResult164 + uv_Mask_Tex);
				#ifdef _MASK_CUSTOM_ON
				float2 staticSwitch168 = ( IN.ase_texcoord.z + uv_Mask_Tex );
				#else
				float2 staticSwitch168 = panner166;
				#endif
				#ifdef _USE_MASK_ON
				float staticSwitch171 = tex2D( _Mask_Tex, staticSwitch168 ).r;
				#else
				float staticSwitch171 = 1.0;
				#endif
				float2 temp_cast_2 = (0.0).xx;
				#ifdef _USE_ROTATE_ON
				float2 staticSwitch184 = appendResult113;
				#else
				float2 staticSwitch184 = temp_cast_2;
				#endif
				float2 appendResult148 = (float2(_Dissolve_Upanner , _Dissolve_Vpanner));
				float2 uv_Dissolve_Texture = IN.ase_texcoord.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner149 = ( 1.0 * _Time.y * appendResult148 + uv_Dissolve_Texture);
				float4 tex2DNode150 = tex2D( _Dissolve_Texture, ( staticSwitch184 + panner149 ) );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch152 = ( IN.ase_texcoord1.y + tex2DNode150.r );
				#else
				float staticSwitch152 = _DIsoolve;
				#endif
				float temp_output_153_0 = ( 1.0 - staticSwitch152 );
				float smoothstepResult156 = smoothstep( temp_output_153_0 , ( temp_output_153_0 + _smoothness ) , tex2DNode150.r);
				#ifdef _USE_SMOOTHSTEP_ON
				float staticSwitch158 = saturate( smoothstepResult156 );
				#else
				float staticSwitch158 = staticSwitch152;
				#endif
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( saturate( ( ( staticSwitch171 * tex2DNode53.r ) * staticSwitch158 ) ) * IN.ase_color.a )));
				
				float4 Color = appendResult67;
				half4 outColor = _SelectionID;
				return outColor;
			}

            ENDHLSL
        }
		
	}
	CustomEditor "ASEMaterialInspector"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.CommentaryNode;188;-1452.718,408.5927;Inherit;False;292;259;Comment;1;119;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;175;-1834.207,1496.886;Inherit;False;2365.389;435.4413;Comment;16;150;151;152;153;154;155;156;157;158;159;145;146;147;148;149;160;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;174;-1763.605,662.8758;Inherit;False;1524;514.9999;Comment;10;162;163;164;165;166;167;168;169;171;170;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;116;-2588.509,211.8766;Inherit;False;775.924;335.1624;Comment;5;113;109;112;111;110;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;115;-2056.008,-211.0663;Inherit;False;692.0001;388.4169;Comment;5;50;52;51;47;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;81;803.287,-374.1421;Inherit;False;232;315;Comment;3;77;79;80;;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;31;-80,64;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;144,-32;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;63;320,-32;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;True;_SRC;10;True;_DST;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;64;320,-32;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;65;320,-32;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;502.3263,-44.63831;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;66;382.8012,-37.94507;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;62;822.8932,-32.27419;Float;False;True;-1;2;ASEMaterialInspector;0;15;Amplify_Shader/Vfx/FXS_Rotate;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;True;True;2;5;True;_SRC;10;True;_DST;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.RangedFloatNode;77;853.287,-324.1421;Inherit;False;Property;_CullMode;CullMode;0;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;853.287,-247.1421;Inherit;False;Property;_SRC;SRC;1;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;857.287,-172.1421;Inherit;False;Property;_DST;DST;2;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;55;-666.3914,-16.3109;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-474.3913,-16.3109;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-298.3913,-16.3109;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;114;-1192.246,58.06351;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;113;-1990.585,331.2296;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;109;-2467.077,261.8766;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;112;-2538.509,395.7867;Inherit;False;Property;_Rotate;Rotate;3;0;Create;True;0;0;0;False;0;False;0.93;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-2271.25,410.039;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;-2119.234,301.1329;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;58;-554.3914,-208.3111;Inherit;False;Property;_Main_Color;Main_Color;9;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-714.3914,96.68916;Inherit;False;Property;_Main_Power;Main_Power;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-814.3915,232.6891;Inherit;False;Property;_Main_Ins;Main_Ins;7;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;53;-986.3915,-33.72773;Inherit;True;Property;_Main_Tex;Main_Tex;5;0;Create;True;0;0;0;False;0;False;-1;a910e3d4643d63d4f82881cf3c011741;a20772110376eb34e9ef28a5e5cad6f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;50;-1798.008,-15.64943;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;52;-1574.008,-79.64942;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1862.008,-161.0663;Inherit;False;0;53;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;-2006.008,-17.06626;Inherit;False;Property;_Main_Upanner;Main_Upanner;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-2006.008,64.35062;Inherit;False;Property;_Main_Vpanner;Main_Vpanner;11;0;Create;True;0;0;0;False;0;False;0;-1.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-193.6053,856.8758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;259.8702,925.7387;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-1713.605,1064.876;Inherit;False;Property;_Mask_V_Panner;Mask_V_Panner;16;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;163;-1713.605,952.8758;Inherit;False;Property;_Mask_U_Panner;Mask_U_Panner;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;164;-1553.605,1000.876;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;166;-1345.605,936.8758;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;167;-1281.605,808.8758;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;168;-1153.605,856.8758;Inherit;False;Property;_Mask_Custom;Mask_Custom;14;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;169;-881.6053,808.8758;Inherit;True;Property;_Mask_Tex;Mask_Tex;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;170;-657.6053,712.8758;Inherit;False;Constant;_Float0;Float 0;35;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;150;-990.8184,1595.327;Inherit;True;Property;_Dissolve_Texture;Dissolve_Texture;17;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;151;-686.8182,1563.327;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;153;-366.8184,1659.327;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;154;-526.8182,1771.327;Inherit;False;Property;_smoothness;smoothness;21;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-238.8185,1739.327;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;156;-110.8185,1643.327;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;157;129.1815,1675.327;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;158;257.1815,1579.327;Inherit;False;Property;_USE_SmoothStep;USE_SmoothStep;20;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;dissolve;step;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-974.8183,1819.327;Inherit;False;Property;_DIsoolve;DIsoolve;18;0;Create;True;0;0;0;False;0;False;1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;145;-1784.207,1674.886;Inherit;False;Property;_Dissolve_Upanner;Dissolve_Upanner;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;146;-1784.207,1754.886;Inherit;False;Property;_Dissolve_Vpanner;Dissolve_Vpanner;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;148;-1576.207,1690.886;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;149;-1400.207,1610.886;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;147;-1704.207,1546.886;Inherit;False;0;150;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;165;-1665.605,824.8758;Inherit;False;0;169;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;176;459.8186,959.8242;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;410.7368,399.4653;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;171;-497.6053,856.8758;Inherit;True;Property;_USE_Mask;USE_Mask;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;152;-574.8182,1595.327;Inherit;False;Property;_Use_Custom;Dissolve_Custom;19;0;Create;False;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Float;Custom;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;160;-1118.67,1614.258;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;186;-1534.552,1345.94;Inherit;False;Constant;_Float2;Float 2;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;184;-1321.329,1415.794;Inherit;False;Property;_USE_Rotate;USE_Rotate;23;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;187;-1041.554,501.1293;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;118;-682.9075,406.8531;Inherit;False;Property;_USE_INS_Custom;USE_INS_Custom;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;USE_Custom;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;178;-1580.04,289.9362;Inherit;False;Property;_USE_Rotate;USE_Rotate;4;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;179;-1784.791,254.2759;Inherit;False;Constant;_Float1;Float 1;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;119;-1402.718,458.5927;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;34;0;59;0
WireConnection;34;1;31;0
WireConnection;67;0;66;0
WireConnection;67;1;66;1
WireConnection;67;2;66;2
WireConnection;67;3;177;0
WireConnection;66;0;34;0
WireConnection;62;1;67;0
WireConnection;55;0;53;0
WireConnection;55;1;54;0
WireConnection;57;0;55;0
WireConnection;57;1;118;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;114;0;52;0
WireConnection;114;1;178;0
WireConnection;113;0;110;0
WireConnection;113;1;109;2
WireConnection;111;0;109;2
WireConnection;111;1;112;0
WireConnection;110;0;109;1
WireConnection;110;1;111;0
WireConnection;53;1;114;0
WireConnection;50;0;47;0
WireConnection;50;1;48;0
WireConnection;52;0;51;0
WireConnection;52;2;50;0
WireConnection;172;0;171;0
WireConnection;172;1;53;1
WireConnection;173;0;172;0
WireConnection;173;1;158;0
WireConnection;164;0;163;0
WireConnection;164;1;162;0
WireConnection;166;0;165;0
WireConnection;166;2;164;0
WireConnection;167;0;187;3
WireConnection;167;1;165;0
WireConnection;168;1;166;0
WireConnection;168;0;167;0
WireConnection;169;1;168;0
WireConnection;150;1;160;0
WireConnection;151;0;119;2
WireConnection;151;1;150;1
WireConnection;153;0;152;0
WireConnection;155;0;153;0
WireConnection;155;1;154;0
WireConnection;156;0;150;1
WireConnection;156;1;153;0
WireConnection;156;2;155;0
WireConnection;157;0;156;0
WireConnection;158;1;152;0
WireConnection;158;0;157;0
WireConnection;148;0;145;0
WireConnection;148;1;146;0
WireConnection;149;0;147;0
WireConnection;149;2;148;0
WireConnection;176;0;173;0
WireConnection;177;0;176;0
WireConnection;177;1;31;4
WireConnection;171;1;170;0
WireConnection;171;0;169;1
WireConnection;152;1;159;0
WireConnection;152;0;151;0
WireConnection;160;0;184;0
WireConnection;160;1;149;0
WireConnection;184;1;186;0
WireConnection;184;0;113;0
WireConnection;118;1;56;0
WireConnection;118;0;119;1
WireConnection;178;1;179;0
WireConnection;178;0;113;0
ASEEND*/
//CHKSM=7D16888BC821FE2A9B52E3E083E1248E543533FA