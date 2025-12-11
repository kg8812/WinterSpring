// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXShader/Post_Fire_2"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_Main_Color01("Main_Color01", Color) = (0.8301887,0.1527234,0.2274782,0)
		_Main_UPanner("Main_UPanner", Float) = 0
		_Main_VPanner("Main_VPanner", Float) = 0
		_Noise_Texture("Noise_Texture", 2D) = "bump" {}
		_Noise_Str("Noise_Str", Float) = 0.18
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Dissolve_Texture("Dissolve_Texture", 2D) = "white" {}
		_Diss_UPanner("Diss_UPanner", Float) = 0
		_Diss_VPanner("Diss_VPanner", Float) = 0
		_Dissolve("Dissolve", Float) = -0.11
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0
		_Circle("Circle", 2D) = "white" {}

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
			sampler2D _Dissolve_Texture;
			sampler2D _Circle;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color01;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Circle_ST;
			float _Main_UPanner;
			float _Main_VPanner;
			float _Noise_UPanner;
			float _Noise_VPanner;
			float _Noise_Str;
			float _Diss_UPanner;
			float _Diss_VPanner;
			float _Dissolve;
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

				float2 appendResult26 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult23 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.texCoord0.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner10 = ( 1.0 * _Time.y * appendResult23 + uv_Noise_Texture);
				float2 temp_output_7_0 = (( UnpackNormalScale( tex2D( _Noise_Texture, panner10 ), 1.0f ) * _Noise_Str )).xy;
				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner3 = ( 1.0 * _Time.y * appendResult26 + ( temp_output_7_0 + uv_Main_Texture ));
				float2 appendResult34 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissolve_Texture = IN.texCoord0.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner30 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_7_0 + uv_Dissolve_Texture ));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch48 = IN.ase_texcoord3.x;
				#else
				float staticSwitch48 = _Dissolve;
				#endif
				float temp_output_62_0 = ( tex2D( _Main_Texture, panner3 ).r * saturate( ( tex2D( _Dissolve_Texture, panner30 ).r + staticSwitch48 ) ) );
				float4 break20 = ( ( _Main_Color01 * temp_output_62_0 ) * IN.color );
				float2 uv_Circle = IN.texCoord0.xy * _Circle_ST.xy + _Circle_ST.zw;
				float4 appendResult19 = (float4(break20.r , break20.g , break20.b , ( temp_output_62_0 * IN.color.a * saturate( ( IN.ase_texcoord3.y + tex2D( _Circle, uv_Circle ).r ) ) )));
				
				float4 Color = appendResult19;

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
			sampler2D _Dissolve_Texture;
			sampler2D _Circle;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color01;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Circle_ST;
			float _Main_UPanner;
			float _Main_VPanner;
			float _Noise_UPanner;
			float _Noise_VPanner;
			float _Noise_Str;
			float _Diss_UPanner;
			float _Diss_VPanner;
			float _Dissolve;
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

				float2 appendResult26 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult23 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.texCoord0.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner10 = ( 1.0 * _Time.y * appendResult23 + uv_Noise_Texture);
				float2 temp_output_7_0 = (( UnpackNormalScale( tex2D( _Noise_Texture, panner10 ), 1.0f ) * _Noise_Str )).xy;
				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner3 = ( 1.0 * _Time.y * appendResult26 + ( temp_output_7_0 + uv_Main_Texture ));
				float2 appendResult34 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissolve_Texture = IN.texCoord0.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner30 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_7_0 + uv_Dissolve_Texture ));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch48 = IN.ase_texcoord3.x;
				#else
				float staticSwitch48 = _Dissolve;
				#endif
				float temp_output_62_0 = ( tex2D( _Main_Texture, panner3 ).r * saturate( ( tex2D( _Dissolve_Texture, panner30 ).r + staticSwitch48 ) ) );
				float4 break20 = ( ( _Main_Color01 * temp_output_62_0 ) * IN.color );
				float2 uv_Circle = IN.texCoord0.xy * _Circle_ST.xy + _Circle_ST.zw;
				float4 appendResult19 = (float4(break20.r , break20.g , break20.b , ( temp_output_62_0 * IN.color.a * saturate( ( IN.ase_texcoord3.y + tex2D( _Circle, uv_Circle ).r ) ) )));
				
				float4 Color = appendResult19;

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
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Noise_Texture;
			sampler2D _Dissolve_Texture;
			sampler2D _Circle;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color01;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Circle_ST;
			float _Main_UPanner;
			float _Main_VPanner;
			float _Noise_UPanner;
			float _Noise_VPanner;
			float _Noise_Str;
			float _Diss_UPanner;
			float _Diss_VPanner;
			float _Dissolve;
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
				float2 appendResult26 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult23 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.ase_texcoord.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner10 = ( 1.0 * _Time.y * appendResult23 + uv_Noise_Texture);
				float2 temp_output_7_0 = (( UnpackNormalScale( tex2D( _Noise_Texture, panner10 ), 1.0f ) * _Noise_Str )).xy;
				float2 uv_Main_Texture = IN.ase_texcoord.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner3 = ( 1.0 * _Time.y * appendResult26 + ( temp_output_7_0 + uv_Main_Texture ));
				float2 appendResult34 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissolve_Texture = IN.ase_texcoord.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner30 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_7_0 + uv_Dissolve_Texture ));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch48 = IN.ase_texcoord1.x;
				#else
				float staticSwitch48 = _Dissolve;
				#endif
				float temp_output_62_0 = ( tex2D( _Main_Texture, panner3 ).r * saturate( ( tex2D( _Dissolve_Texture, panner30 ).r + staticSwitch48 ) ) );
				float4 break20 = ( ( _Main_Color01 * temp_output_62_0 ) * IN.ase_color );
				float2 uv_Circle = IN.ase_texcoord.xy * _Circle_ST.xy + _Circle_ST.zw;
				float4 appendResult19 = (float4(break20.r , break20.g , break20.b , ( temp_output_62_0 * IN.ase_color.a * saturate( ( IN.ase_texcoord1.y + tex2D( _Circle, uv_Circle ).r ) ) )));
				
				float4 Color = appendResult19;

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
        	#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Noise_Texture;
			sampler2D _Dissolve_Texture;
			sampler2D _Circle;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color01;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float4 _Dissolve_Texture_ST;
			float4 _Circle_ST;
			float _Main_UPanner;
			float _Main_VPanner;
			float _Noise_UPanner;
			float _Noise_VPanner;
			float _Noise_Str;
			float _Diss_UPanner;
			float _Diss_VPanner;
			float _Dissolve;
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
				float2 appendResult26 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult23 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.ase_texcoord.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner10 = ( 1.0 * _Time.y * appendResult23 + uv_Noise_Texture);
				float2 temp_output_7_0 = (( UnpackNormalScale( tex2D( _Noise_Texture, panner10 ), 1.0f ) * _Noise_Str )).xy;
				float2 uv_Main_Texture = IN.ase_texcoord.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner3 = ( 1.0 * _Time.y * appendResult26 + ( temp_output_7_0 + uv_Main_Texture ));
				float2 appendResult34 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissolve_Texture = IN.ase_texcoord.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner30 = ( 1.0 * _Time.y * appendResult34 + ( temp_output_7_0 + uv_Dissolve_Texture ));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch48 = IN.ase_texcoord1.x;
				#else
				float staticSwitch48 = _Dissolve;
				#endif
				float temp_output_62_0 = ( tex2D( _Main_Texture, panner3 ).r * saturate( ( tex2D( _Dissolve_Texture, panner30 ).r + staticSwitch48 ) ) );
				float4 break20 = ( ( _Main_Color01 * temp_output_62_0 ) * IN.ase_color );
				float2 uv_Circle = IN.ase_texcoord.xy * _Circle_ST.xy + _Circle_ST.zw;
				float4 appendResult19 = (float4(break20.r , break20.g , break20.b , ( temp_output_62_0 * IN.ase_color.a * saturate( ( IN.ase_texcoord1.y + tex2D( _Circle, uv_Circle ).r ) ) )));
				
				float4 Color = appendResult19;
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
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-934.4648,-226.5708;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;26;-931.9922,98.6615;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1172.708,172.5299;Inherit;False;Property;_Main_VPanner;Main_VPanner;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1170.371,99.44122;Inherit;False;Property;_Main_UPanner;Main_UPanner;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;19;910.8397,7.651947;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;20;687.4731,-13.48274;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;680.0678,165.2134;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1060.071,297.1198;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1359.327,582.9366;Inherit;False;Property;_Diss_UPanner;Diss_UPanner;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1359.327,664.7576;Inherit;False;Property;_Diss_VPanner;Diss_VPanner;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-1158.866,579.8685;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-453.8365,420.0739;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;48;-586.8911,679.0346;Inherit;False;Property;_Use_Custom;Use_Custom;12;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-801.8029,694.2038;Inherit;False;Property;_Dissolve;Dissolve;11;0;Create;True;0;0;0;False;0;False;-0.11;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-145.0746,-83.08764;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1444.202,-332.0796;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;7;-1287.202,-318.0796;Inherit;False;True;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1570.202,-149.0797;Inherit;False;Property;_Noise_Str;Noise_Str;5;0;Create;True;0;0;0;False;0;False;0.18;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;23;-2104.068,-169.0163;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-2288.493,-164.9406;Inherit;False;Property;_Noise_UPanner;Noise_UPanner;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1770.202,-348.0796;Inherit;True;Property;_Noise_Texture;Noise_Texture;4;0;Create;True;0;0;0;False;0;False;-1;e352ba45d12e9ea449f86114ee6bb980;e352ba45d12e9ea449f86114ee6bb980;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;-2290.192,-78.33223;Inherit;False;Property;_Noise_VPanner;Noise_VPanner;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;65;1104.849,7.530129;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;66;1104.849,7.530129;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;67;1104.849,7.530129;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;218.1625,-206.9832;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-153.7041,-260.0471;Inherit;False;Property;_Main_Color01;Main_Color01;1;1;[HDR];Create;True;0;0;0;False;0;False;0.8301887,0.1527234,0.2274782,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;49;315.0389,179.8994;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;523.9587,-95.71003;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;52;-950.1028,773.9323;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;93;467.2812,839.949;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;64;1104.849,7.530129;Float;False;True;-1;2;ASEMaterialInspector;0;15;FXShader/Post_Fire_2;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;638535270377892772;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;120;-89.89443,855.155;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;61;-214.2701,305.392;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-796.5075,459.5187;Inherit;True;Property;_Dissolve_Texture;Dissolve_Texture;8;0;Create;True;0;0;0;False;0;False;-1;93afafdc5fb3623489ffd517d3f11574;93afafdc5fb3623489ffd517d3f11574;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;118;-663.1847,954.0446;Inherit;True;Property;_Circle;Circle;13;0;Create;True;0;0;0;False;0;False;-1;7ba4a682d31b464459128e57b9cbe33c;7ba4a682d31b464459128e57b9cbe33c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;30;-994.7123,471.0826;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;121;-1271.852,955.2526;Inherit;False;0;118;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1425.975,-58.02006;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-1744.011,543.3638;Inherit;False;0;29;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;10;-2014.602,-333.7796;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2257.203,-363.0796;Inherit;False;0;6;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;3;-779.5,-81;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-512.2272,-88.25127;Inherit;True;Property;_Main_Texture;Main_Texture;0;0;Create;True;0;0;0;False;0;False;-1;f016926f4da6b354eac19272eb7b012f;f016926f4da6b354eac19272eb7b012f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;5;0;7;0
WireConnection;5;1;4;0
WireConnection;26;0;27;0
WireConnection;26;1;28;0
WireConnection;19;0;20;0
WireConnection;19;1;20;1
WireConnection;19;2;20;2
WireConnection;19;3;51;0
WireConnection;20;0;82;0
WireConnection;51;0;62;0
WireConnection;51;1;49;4
WireConnection;51;2;93;0
WireConnection;32;0;7;0
WireConnection;32;1;31;0
WireConnection;34;0;35;0
WireConnection;34;1;36;0
WireConnection;38;0;29;1
WireConnection;38;1;48;0
WireConnection;48;1;39;0
WireConnection;48;0;52;1
WireConnection;62;0;2;1
WireConnection;62;1;61;0
WireConnection;8;0;6;0
WireConnection;8;1;9;0
WireConnection;7;0;8;0
WireConnection;23;0;24;0
WireConnection;23;1;25;0
WireConnection;6;1;10;0
WireConnection;13;0;12;0
WireConnection;13;1;62;0
WireConnection;82;0;13;0
WireConnection;82;1;49;0
WireConnection;93;0;120;0
WireConnection;64;1;19;0
WireConnection;120;0;52;2
WireConnection;120;1;118;1
WireConnection;61;0;38;0
WireConnection;29;1;30;0
WireConnection;118;1;121;0
WireConnection;30;0;32;0
WireConnection;30;2;34;0
WireConnection;10;0;11;0
WireConnection;10;2;23;0
WireConnection;3;0;5;0
WireConnection;3;2;26;0
WireConnection;2;1;3;0
ASEEND*/
//CHKSM=87AFE4DF0186C7D05BB16EDA0E4152E326D3CEE2