// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXShader/Slash"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_Sword_Texture("Sword_Texture", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 50)) = 0
		_Sword_UOffset("Sword_UOffset", Range( -1 , 1)) = 0.82
		_Mask_Ragne("Mask_Ragne", Range( 1 , 10)) = 2.78
		_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_Main_Color("Main_Color", Color) = (0,0,0,0)
		_Main_Ins("Main_Ins", Range( 1 , 10)) = 1
		_Main_Pow("Main_Pow", Range( 1 , 10)) = 1
		_Main_UPanner("Main_UPanner", Float) = 1
		_Main_VPanner("Main_VPanner", Float) = 0
		_Noise_Texture("Noise_Texture", 2D) = "bump" {}
		_UVNoise_Str("UVNoise_Str", Range( 0 , 1)) = 0
		_Noise_UPanner("Noise_UPanner", Float) = 1
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Emi_Offset("Emi_Offset", Range( -1 , 1)) = 0
		_Emi_Range("Emi_Range", Range( 1 , 10)) = 2
		[HDR]_Emi_Color("Emi_Color", Color) = (0,0,0,0)
		_Dissove_Texture("Dissove_Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 0
		_Diss_VPanner("Diss_VPanner", Float) = 0
		_Diss_UPanner("Diss_UPanner", Float) = 0
		_Head_Range("Head_Range", Range( -1 , 1)) = 0
		_Hade_Ins("Hade_Ins", Float) = 0
		_Fade_Distance("Fade_Distance", Float) = 0
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
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

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 120110
			#define REQUIRE_DEPTH_TEXTURE 1


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
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Noise_Texture;
			sampler2D _Sword_Texture;
			sampler2D _Dissove_Texture;
			uniform float4 _CameraDepthTexture_TexelSize;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissove_Texture_ST;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Emi_Color;
			float4 _Sword_Texture_ST;
			float4 _Main_Color;
			float _Emi_Offset;
			float _Diss_VPanner;
			float _Diss_UPanner;
			float _Opacity;
			float _Mask_Ragne;
			float _Sword_UOffset;
			float _Main_Ins;
			float _Hade_Ins;
			float _Dissolve;
			float _Head_Range;
			float _Emi_Range;
			float _UVNoise_Str;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Main_VPanner;
			float _Main_UPanner;
			float _Main_Pow;
			float _Fade_Distance;
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
				float4 ase_texcoord4 : TEXCOORD4;
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

				float4 ase_clipPos = TransformObjectToHClip((v.positionOS).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
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

				float2 texCoord11 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch41 = IN.ase_texcoord3.y;
				#else
				float staticSwitch41 = _Emi_Offset;
				#endif
				float temp_output_57_0 = saturate( ( ( 1.0 - texCoord11.x ) + staticSwitch41 ) );
				float2 appendResult34 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult7 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.texCoord0.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner8 = ( 1.0 * _Time.y * appendResult7 + uv_Noise_Texture);
				float2 temp_output_20_0 = ( (UnpackNormalScale( tex2D( _Noise_Texture, panner8 ), 1.0f )).xy * _UVNoise_Str );
				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner42 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_20_0 + uv_Main_Texture ));
				float4 tex2DNode46 = tex2D( _Main_Texture, panner42 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch69 = IN.ase_texcoord3.z;
				#else
				float staticSwitch69 = _Main_Ins;
				#endif
				float4 break89 = ( ( ( pow( ( temp_output_57_0 * ( temp_output_57_0 + tex2DNode46.r ) ) , _Emi_Range ) * _Emi_Color ) + ( ( pow( saturate( ( tex2DNode46.r + saturate( ( ( texCoord11.y + _Head_Range ) * _Hade_Ins ) ) ) ) , _Main_Pow ) * staticSwitch69 ) * _Main_Color ) ) * IN.color );
				float2 uv_Sword_Texture = IN.texCoord0.xy * _Sword_Texture_ST.xy + _Sword_Texture_ST.zw;
				#ifdef _USE_CUSTOM_ON
				float staticSwitch26 = IN.ase_texcoord3.x;
				#else
				float staticSwitch26 = _Sword_UOffset;
				#endif
				float2 appendResult43 = (float2(staticSwitch26 , 0.0));
				float2 appendResult37 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissove_Texture = IN.texCoord0.xy * _Dissove_Texture_ST.xy + _Dissove_Texture_ST.zw;
				float2 panner48 = ( 1.0 * _Time.y * appendResult37 + uv_Dissove_Texture);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch56 = IN.ase_texcoord3.w;
				#else
				float staticSwitch56 = _Dissolve;
				#endif
				float4 screenPos = IN.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth73 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Fade_Distance ) );
				float4 appendResult88 = (float4(break89.r , break89.g , break89.b , ( IN.color.a * ( saturate( ( ( ( tex2D( _Sword_Texture, (( temp_output_20_0 + uv_Sword_Texture )*1.0 + appendResult43) ).r * pow( ( ( saturate( ( 1.0 - texCoord11.x ) ) * texCoord11.x ) * 4.0 ) , _Mask_Ragne ) ) * _Opacity ) * saturate( ( tex2D( _Dissove_Texture, panner48 ).r + staticSwitch56 ) ) ) ) * saturate( distanceDepth73 ) ) )));
				
				float4 Color = appendResult88;

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

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

			#define ASE_SRP_VERSION 120110
			#define REQUIRE_DEPTH_TEXTURE 1


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
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Noise_Texture;
			sampler2D _Sword_Texture;
			sampler2D _Dissove_Texture;
			uniform float4 _CameraDepthTexture_TexelSize;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissove_Texture_ST;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Emi_Color;
			float4 _Sword_Texture_ST;
			float4 _Main_Color;
			float _Emi_Offset;
			float _Diss_VPanner;
			float _Diss_UPanner;
			float _Opacity;
			float _Mask_Ragne;
			float _Sword_UOffset;
			float _Main_Ins;
			float _Hade_Ins;
			float _Dissolve;
			float _Head_Range;
			float _Emi_Range;
			float _UVNoise_Str;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Main_VPanner;
			float _Main_UPanner;
			float _Main_Pow;
			float _Fade_Distance;
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
				float4 ase_texcoord4 : TEXCOORD4;
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

				float4 ase_clipPos = TransformObjectToHClip((v.positionOS).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
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

				float2 texCoord11 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch41 = IN.ase_texcoord3.y;
				#else
				float staticSwitch41 = _Emi_Offset;
				#endif
				float temp_output_57_0 = saturate( ( ( 1.0 - texCoord11.x ) + staticSwitch41 ) );
				float2 appendResult34 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult7 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.texCoord0.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner8 = ( 1.0 * _Time.y * appendResult7 + uv_Noise_Texture);
				float2 temp_output_20_0 = ( (UnpackNormalScale( tex2D( _Noise_Texture, panner8 ), 1.0f )).xy * _UVNoise_Str );
				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner42 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_20_0 + uv_Main_Texture ));
				float4 tex2DNode46 = tex2D( _Main_Texture, panner42 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch69 = IN.ase_texcoord3.z;
				#else
				float staticSwitch69 = _Main_Ins;
				#endif
				float4 break89 = ( ( ( pow( ( temp_output_57_0 * ( temp_output_57_0 + tex2DNode46.r ) ) , _Emi_Range ) * _Emi_Color ) + ( ( pow( saturate( ( tex2DNode46.r + saturate( ( ( texCoord11.y + _Head_Range ) * _Hade_Ins ) ) ) ) , _Main_Pow ) * staticSwitch69 ) * _Main_Color ) ) * IN.color );
				float2 uv_Sword_Texture = IN.texCoord0.xy * _Sword_Texture_ST.xy + _Sword_Texture_ST.zw;
				#ifdef _USE_CUSTOM_ON
				float staticSwitch26 = IN.ase_texcoord3.x;
				#else
				float staticSwitch26 = _Sword_UOffset;
				#endif
				float2 appendResult43 = (float2(staticSwitch26 , 0.0));
				float2 appendResult37 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissove_Texture = IN.texCoord0.xy * _Dissove_Texture_ST.xy + _Dissove_Texture_ST.zw;
				float2 panner48 = ( 1.0 * _Time.y * appendResult37 + uv_Dissove_Texture);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch56 = IN.ase_texcoord3.w;
				#else
				float staticSwitch56 = _Dissolve;
				#endif
				float4 screenPos = IN.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth73 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Fade_Distance ) );
				float4 appendResult88 = (float4(break89.r , break89.g , break89.b , ( IN.color.a * ( saturate( ( ( ( tex2D( _Sword_Texture, (( temp_output_20_0 + uv_Sword_Texture )*1.0 + appendResult43) ).r * pow( ( ( saturate( ( 1.0 - texCoord11.x ) ) * texCoord11.x ) * 4.0 ) , _Mask_Ragne ) ) * _Opacity ) * saturate( ( tex2D( _Dissove_Texture, panner48 ).r + staticSwitch56 ) ) ) ) * saturate( distanceDepth73 ) ) )));
				
				float4 Color = appendResult88;

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
			#define REQUIRE_DEPTH_TEXTURE 1


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
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Noise_Texture;
			sampler2D _Sword_Texture;
			sampler2D _Dissove_Texture;
			uniform float4 _CameraDepthTexture_TexelSize;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissove_Texture_ST;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Emi_Color;
			float4 _Sword_Texture_ST;
			float4 _Main_Color;
			float _Emi_Offset;
			float _Diss_VPanner;
			float _Diss_UPanner;
			float _Opacity;
			float _Mask_Ragne;
			float _Sword_UOffset;
			float _Main_Ins;
			float _Hade_Ins;
			float _Dissolve;
			float _Head_Range;
			float _Emi_Range;
			float _UVNoise_Str;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Main_VPanner;
			float _Main_UPanner;
			float _Main_Pow;
			float _Fade_Distance;
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
				float4 ase_texcoord2 : TEXCOORD2;
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

				float4 ase_clipPos = TransformObjectToHClip((v.positionOS).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
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
				float2 texCoord11 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch41 = IN.ase_texcoord1.y;
				#else
				float staticSwitch41 = _Emi_Offset;
				#endif
				float temp_output_57_0 = saturate( ( ( 1.0 - texCoord11.x ) + staticSwitch41 ) );
				float2 appendResult34 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult7 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.ase_texcoord.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner8 = ( 1.0 * _Time.y * appendResult7 + uv_Noise_Texture);
				float2 temp_output_20_0 = ( (UnpackNormalScale( tex2D( _Noise_Texture, panner8 ), 1.0f )).xy * _UVNoise_Str );
				float2 uv_Main_Texture = IN.ase_texcoord.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner42 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_20_0 + uv_Main_Texture ));
				float4 tex2DNode46 = tex2D( _Main_Texture, panner42 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch69 = IN.ase_texcoord1.z;
				#else
				float staticSwitch69 = _Main_Ins;
				#endif
				float4 break89 = ( ( ( pow( ( temp_output_57_0 * ( temp_output_57_0 + tex2DNode46.r ) ) , _Emi_Range ) * _Emi_Color ) + ( ( pow( saturate( ( tex2DNode46.r + saturate( ( ( texCoord11.y + _Head_Range ) * _Hade_Ins ) ) ) ) , _Main_Pow ) * staticSwitch69 ) * _Main_Color ) ) * IN.ase_color );
				float2 uv_Sword_Texture = IN.ase_texcoord.xy * _Sword_Texture_ST.xy + _Sword_Texture_ST.zw;
				#ifdef _USE_CUSTOM_ON
				float staticSwitch26 = IN.ase_texcoord1.x;
				#else
				float staticSwitch26 = _Sword_UOffset;
				#endif
				float2 appendResult43 = (float2(staticSwitch26 , 0.0));
				float2 appendResult37 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissove_Texture = IN.ase_texcoord.xy * _Dissove_Texture_ST.xy + _Dissove_Texture_ST.zw;
				float2 panner48 = ( 1.0 * _Time.y * appendResult37 + uv_Dissove_Texture);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch56 = IN.ase_texcoord1.w;
				#else
				float staticSwitch56 = _Dissolve;
				#endif
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth73 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Fade_Distance ) );
				float4 appendResult88 = (float4(break89.r , break89.g , break89.b , ( IN.ase_color.a * ( saturate( ( ( ( tex2D( _Sword_Texture, (( temp_output_20_0 + uv_Sword_Texture )*1.0 + appendResult43) ).r * pow( ( ( saturate( ( 1.0 - texCoord11.x ) ) * texCoord11.x ) * 4.0 ) , _Mask_Ragne ) ) * _Opacity ) * saturate( ( tex2D( _Dissove_Texture, panner48 ).r + staticSwitch56 ) ) ) ) * saturate( distanceDepth73 ) ) )));
				
				float4 Color = appendResult88;

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
			#define REQUIRE_DEPTH_TEXTURE 1


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
        	#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Noise_Texture;
			sampler2D _Sword_Texture;
			sampler2D _Dissove_Texture;
			uniform float4 _CameraDepthTexture_TexelSize;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissove_Texture_ST;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Emi_Color;
			float4 _Sword_Texture_ST;
			float4 _Main_Color;
			float _Emi_Offset;
			float _Diss_VPanner;
			float _Diss_UPanner;
			float _Opacity;
			float _Mask_Ragne;
			float _Sword_UOffset;
			float _Main_Ins;
			float _Hade_Ins;
			float _Dissolve;
			float _Head_Range;
			float _Emi_Range;
			float _UVNoise_Str;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Main_VPanner;
			float _Main_UPanner;
			float _Main_Pow;
			float _Fade_Distance;
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
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            float4 _SelectionID;

			
			VertexOutput vert(VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float4 ase_clipPos = TransformObjectToHClip((v.positionOS).xyz);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord2 = screenPos;
				
				o.ase_texcoord.xy = v.ase_texcoord.xy;
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
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
				float2 texCoord11 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch41 = IN.ase_texcoord1.y;
				#else
				float staticSwitch41 = _Emi_Offset;
				#endif
				float temp_output_57_0 = saturate( ( ( 1.0 - texCoord11.x ) + staticSwitch41 ) );
				float2 appendResult34 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult7 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.ase_texcoord.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner8 = ( 1.0 * _Time.y * appendResult7 + uv_Noise_Texture);
				float2 temp_output_20_0 = ( (UnpackNormalScale( tex2D( _Noise_Texture, panner8 ), 1.0f )).xy * _UVNoise_Str );
				float2 uv_Main_Texture = IN.ase_texcoord.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner42 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_20_0 + uv_Main_Texture ));
				float4 tex2DNode46 = tex2D( _Main_Texture, panner42 );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch69 = IN.ase_texcoord1.z;
				#else
				float staticSwitch69 = _Main_Ins;
				#endif
				float4 break89 = ( ( ( pow( ( temp_output_57_0 * ( temp_output_57_0 + tex2DNode46.r ) ) , _Emi_Range ) * _Emi_Color ) + ( ( pow( saturate( ( tex2DNode46.r + saturate( ( ( texCoord11.y + _Head_Range ) * _Hade_Ins ) ) ) ) , _Main_Pow ) * staticSwitch69 ) * _Main_Color ) ) * IN.ase_color );
				float2 uv_Sword_Texture = IN.ase_texcoord.xy * _Sword_Texture_ST.xy + _Sword_Texture_ST.zw;
				#ifdef _USE_CUSTOM_ON
				float staticSwitch26 = IN.ase_texcoord1.x;
				#else
				float staticSwitch26 = _Sword_UOffset;
				#endif
				float2 appendResult43 = (float2(staticSwitch26 , 0.0));
				float2 appendResult37 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissove_Texture = IN.ase_texcoord.xy * _Dissove_Texture_ST.xy + _Dissove_Texture_ST.zw;
				float2 panner48 = ( 1.0 * _Time.y * appendResult37 + uv_Dissove_Texture);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch56 = IN.ase_texcoord1.w;
				#else
				float staticSwitch56 = _Dissolve;
				#endif
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth73 = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH( ase_screenPosNorm.xy ),_ZBufferParams);
				float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z,_ZBufferParams ) ) / ( _Fade_Distance ) );
				float4 appendResult88 = (float4(break89.r , break89.g , break89.b , ( IN.ase_color.a * ( saturate( ( ( ( tex2D( _Sword_Texture, (( temp_output_20_0 + uv_Sword_Texture )*1.0 + appendResult43) ).r * pow( ( ( saturate( ( 1.0 - texCoord11.x ) ) * texCoord11.x ) * 4.0 ) , _Mask_Ragne ) ) * _Opacity ) * saturate( ( tex2D( _Dissove_Texture, panner48 ).r + staticSwitch56 ) ) ) ) * saturate( distanceDepth73 ) ) )));
				
				float4 Color = appendResult88;
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
Node;AmplifyShaderEditor.CommentaryNode;3;-2811.961,-654.4334;Inherit;False;1239;450.9995;UV_Noise;9;20;13;12;9;8;7;6;5;4;;0.08018869,0.2667602,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-2757.961,-457.6339;Inherit;False;Property;_Noise_UPanner;Noise_UPanner;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2761.961,-338.6338;Inherit;False;Property;_Noise_VPanner;Noise_VPanner;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;7;-2567.061,-457.9338;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;10;-2160.356,958.8893;Inherit;False;1304.511;484.0015;Mask;8;54;52;45;40;36;28;15;11;;1,0.9176218,0,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2110.356,1097.59;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-2241.86,-319.4338;Inherit;False;Property;_UVNoise_Str;UVNoise_Str;11;0;Create;True;0;0;0;False;0;False;0;0.322;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1415.559,272.6348;Inherit;False;Property;_Head_Range;Head_Range;21;0;Create;True;0;0;0;False;0;False;0;-0.317;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-1796.845,1008.889;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;16;-1448.911,-749.743;Inherit;False;1645.766;578.739;Emi;12;82;75;74;72;67;60;57;47;41;39;25;24;;0.8808007,0,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;17;-813.8536,965.9315;Inherit;False;1143.327;466.0005;Dissolve;10;66;59;56;53;49;48;44;37;33;30;;0.9056604,0.0555358,0.6059473,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1790.767,705.7242;Inherit;False;Property;_Sword_UOffset;Sword_UOffset;2;0;Create;True;0;0;0;False;0;False;0.82;-0.287;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1807.961,-538.6337;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1611.553,89.84808;Inherit;False;Property;_Main_UPanner;Main_UPanner;8;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1608.553,188.848;Inherit;False;Property;_Main_VPanner;Main_VPanner;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1398.911,-424.574;Inherit;False;Property;_Emi_Offset;Emi_Offset;14;0;Create;True;0;0;0;False;0;False;0;-0.425;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1448.161,-211.1336;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1529.267,790.3239;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;-1657.846,1018.889;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-950.9797,473.0952;Inherit;False;Property;_Hade_Ins;Hade_Ins;22;0;Create;True;0;0;0;False;0;False;0;2.89;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-733.8536,1224.931;Inherit;False;Property;_Diss_UPanner;Diss_UPanner;20;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1132.049,313.5057;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-738.8536,1315.932;Inherit;False;Property;_Diss_VPanner;Diss_VPanner;19;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-1397.553,128.8481;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-880.3924,327.8278;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1549.846,1084.889;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;37;-575.8534,1275.932;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-1338.781,362.4433;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;39;-1322.199,-699.7432;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1496.847,1298.89;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;42;-1364.553,-24.15196;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;-1255.022,707.2283;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1335.847,1099.889;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-1192.095,-119.5382;Inherit;True;Property;_Main_Texture;Main_Texture;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1128.911,-696.5732;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;48;-536.8534,1095.932;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-363.8539,1247.931;Inherit;False;Property;_Dissolve;Dissolve;18;0;Create;True;0;0;0;False;0;False;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;50;-765.816,327.8279;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;51;-1201.955,545.9229;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1343.847,1326.891;Inherit;False;Property;_Mask_Ragne;Mask_Ragne;3;0;Create;True;0;0;0;False;0;False;2.78;2;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;53;-378.851,1044.447;Inherit;True;Property;_Dissove_Texture;Dissove_Texture;17;0;Create;True;0;0;0;False;0;False;-1;None;93afafdc5fb3623489ffd517d3f11574;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;54;-1115.847,1099.889;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;-958.7878,545.5727;Inherit;True;Property;_Sword_Texture;Sword_Texture;0;0;Create;True;0;0;0;False;0;False;-1;None;a65b8c233e6a15e428893f6251eda034;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;57;-920.9113,-689.5731;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-882.4407,-94.67505;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-70.85394,1060.932;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-775.7549,-425.0043;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-610.7772,570.3372;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-606.431,781.272;Inherit;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;0;False;0;False;0;2;0;50;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;64;-654.3104,-96.72101;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1128.907,175.8431;Inherit;False;Property;_Main_Pow;Main_Pow;7;0;Create;True;0;0;0;False;0;False;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;66;131.4732,1058.605;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-550.5343,-415.5839;Inherit;False;Property;_Emi_Range;Emi_Range;15;0;Create;True;0;0;0;False;0;False;2;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-304.6215,566.8708;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;70;-636.596,-24.47295;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;62.30596,454.744;Inherit;False;Property;_Fade_Distance;Fade_Distance;23;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-585.1456,-688.254;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;73;267.348,443.4956;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;74;-315.1467,-675.2542;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;75;1.447563,-441.3568;Inherit;False;Property;_Emi_Color;Emi_Color;16;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.6039216,0.5372549,0.4941176,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-64.92719,564.7219;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-419.7348,-81.53455;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;78;-458.1015,232.5949;Inherit;False;Property;_Main_Color;Main_Color;5;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;79;525.306,461.744;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;80;150.5596,575.7221;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-238.1171,-41.08861;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-13.14619,-679.2542;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;678.3065,359.744;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-9.372017,-32.48875;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;85;-37.45136,216.6437;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;89;544.0177,-36.043;Inherit;True;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;240.9432,-29.50193;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;88;960.4343,-34.83604;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;750.2748,173.7198;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;26;-1487.691,675.9833;Inherit;False;Property;_Use_Custom;Use_Custom;24;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;56;-115.2316,1340.503;Inherit;False;Property;_Use_Custom;Use_Custom;25;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;69;-443.06,138.6926;Inherit;False;Property;_Use_Custom;Use_Custom;26;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;41;-1108.981,-416.8266;Inherit;False;Property;_Use_Custom;Use_Custom;27;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;18;-2823.78,263.4482;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;62;-813.6316,222.4879;Inherit;False;Property;_Main_Ins;Main_Ins;6;0;Create;True;0;0;0;False;0;False;1;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-781.2447,1004.678;Inherit;False;0;53;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-1604.58,413.9745;Inherit;True;0;55;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1743.553,-107.152;Inherit;False;0;46;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;13;-2009.96,-538.6337;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;9;-2303.96,-540.6337;Inherit;True;Property;_Noise_Texture;Noise_Texture;10;0;Create;True;0;0;0;False;0;False;-1;None;c2a2220383ca55a489a953b9b7498ec4;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;8;-2539.26,-604.4335;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-2749.961,-603.6337;Inherit;False;0;9;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;91;1287.573,-0.58901;Float;False;True;-1;2;ASEMaterialInspector;0;15;FXShader/Slash;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;92;1287.573,-0.58901;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;93;1287.573,-0.58901;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;94;1287.573,-0.58901;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;7;0;4;0
WireConnection;7;1;5;0
WireConnection;15;0;11;1
WireConnection;20;0;13;0
WireConnection;20;1;12;0
WireConnection;25;0;20;0
WireConnection;25;1;21;0
WireConnection;28;0;15;0
WireConnection;32;0;11;2
WireConnection;32;1;14;0
WireConnection;34;0;22;0
WireConnection;34;1;23;0
WireConnection;35;0;32;0
WireConnection;35;1;29;0
WireConnection;36;0;28;0
WireConnection;36;1;11;1
WireConnection;37;0;30;0
WireConnection;37;1;33;0
WireConnection;38;0;20;0
WireConnection;38;1;31;0
WireConnection;39;0;11;1
WireConnection;42;0;25;0
WireConnection;42;2;34;0
WireConnection;43;0;26;0
WireConnection;43;1;27;0
WireConnection;45;0;36;0
WireConnection;45;1;40;0
WireConnection;46;1;42;0
WireConnection;47;0;39;0
WireConnection;47;1;41;0
WireConnection;48;0;44;0
WireConnection;48;2;37;0
WireConnection;50;0;35;0
WireConnection;51;0;38;0
WireConnection;51;2;43;0
WireConnection;53;1;48;0
WireConnection;54;0;45;0
WireConnection;54;1;52;0
WireConnection;55;1;51;0
WireConnection;57;0;47;0
WireConnection;58;0;46;1
WireConnection;58;1;50;0
WireConnection;59;0;53;1
WireConnection;59;1;56;0
WireConnection;60;0;57;0
WireConnection;60;1;46;1
WireConnection;61;0;55;1
WireConnection;61;1;54;0
WireConnection;64;0;58;0
WireConnection;66;0;59;0
WireConnection;68;0;61;0
WireConnection;68;1;63;0
WireConnection;70;0;64;0
WireConnection;70;1;65;0
WireConnection;72;0;57;0
WireConnection;72;1;60;0
WireConnection;73;0;71;0
WireConnection;74;0;72;0
WireConnection;74;1;67;0
WireConnection;76;0;68;0
WireConnection;76;1;66;0
WireConnection;77;0;70;0
WireConnection;77;1;69;0
WireConnection;79;0;73;0
WireConnection;80;0;76;0
WireConnection;81;0;77;0
WireConnection;81;1;78;0
WireConnection;82;0;74;0
WireConnection;82;1;75;0
WireConnection;83;0;80;0
WireConnection;83;1;79;0
WireConnection;84;0;82;0
WireConnection;84;1;81;0
WireConnection;89;0;86;0
WireConnection;86;0;84;0
WireConnection;86;1;85;0
WireConnection;88;0;89;0
WireConnection;88;1;89;1
WireConnection;88;2;89;2
WireConnection;88;3;87;0
WireConnection;87;0;85;4
WireConnection;87;1;83;0
WireConnection;26;1;19;0
WireConnection;26;0;18;1
WireConnection;56;1;49;0
WireConnection;56;0;18;4
WireConnection;69;1;62;0
WireConnection;69;0;18;3
WireConnection;41;1;24;0
WireConnection;41;0;18;2
WireConnection;13;0;9;0
WireConnection;9;1;8;0
WireConnection;8;0;6;0
WireConnection;8;2;7;0
WireConnection;91;1;88;0
ASEEND*/
//CHKSM=CEB97882FEAAFAEDEC007A6E300DB7167CDDC425