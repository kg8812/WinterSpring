// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify_Shader/Vfx/Fx_Slash_Dissolve_alp"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_Noise_Tex("Noise_Tex", 2D) = "white" {}
		_Normal_Main_Tex("Normal_Main_Tex", 2D) = "white" {}
		_Noiise_Power("Noiise_Power", Float) = 1
		_Noise_Ins("Noise_Ins", Float) = 1
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Normal_Main__Upanner("Normal_Main__Upanner", Float) = -0.1
		_Normal_Main__Upanner2("Normal_Main__Upanner", Float) = -0.1
		_Normal_Main_Upanner("Normal_Main_Upanner", Float) = -0.1
		_Normal_Main_Vpanner("Normal_Main_Vpanner", Float) = 0
		_Normal_Main_Vpanner2("Normal_Main_Vpanner", Float) = 0
		_Normal_Main_Vpanner1("Normal_Main_Vpanner", Float) = 0
		_Dissolve_Tex("Dissolve_Tex", 2D) = "white" {}
		[HDR]_Color_A("Color_A", Color) = (1.932916,1.932916,1.932916,1)
		[HDR]_Color_B("Color_B", Color) = (1,1,1,1)
		_Noise_Str("Noise_Str", Range( 0 , 0.5)) = 0
		_Normal_Noise_Tex("Normal_Noise_Tex", 2D) = "white" {}
		_Normal_Main_Str("Normal_Main_Str", Range( 0 , 0.5)) = 0
		_Vertex_Val("Vertex_Val", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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

			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
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

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_COLOR


			sampler2D _Normal_Main_Tex;
			sampler2D _Noise_Tex;
			sampler2D _Normal_Noise_Tex;
			sampler2D _Main_Tex;
			sampler2D _Dissolve_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Normal_Main_Tex_ST;
			float4 _Color_B;
			float4 _Noise_Tex_ST;
			float4 _Color_A;
			float4 _Normal_Noise_Tex_ST;
			float _Noise_Ins;
			float _Noiise_Power;
			float _Noise_Str;
			float _Normal_Main__Upanner2;
			float _Normal_Main_Str;
			float _Normal_Main_Vpanner;
			float _Normal_Main__Upanner;
			float _Vertex_Val;
			float _Normal_Main_Vpanner1;
			float _Normal_Main_Vpanner2;
			float _Normal_Main_Upanner;
			CBUFFER_END


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
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

				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = v.uv0.xy * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2Dlod( _Normal_Main_Tex, float4( panner388, 0, 0.0) ), 1.0f );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = ( tex2DNode384 * ( v.normal * _Vertex_Val ) );
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

				float2 uv_Noise_Tex = IN.texCoord0.xy * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
				float smoothstepResult355 = smoothstep( 0.25 , 0.4 , uv_Noise_Tex.y);
				float smoothstepResult352 = smoothstep( 0.15 , 0.1 , uv_Noise_Tex.y);
				float temp_output_358_0 = ( smoothstepResult355 + smoothstepResult352 );
				float2 appendResult304 = (float2(_Normal_Main__Upanner , _Normal_Main_Vpanner));
				float2 appendResult397 = (float2(_Normal_Main__Upanner2 , _Normal_Main_Vpanner2));
				float2 uv_Normal_Noise_Tex = IN.texCoord0.xy * _Normal_Noise_Tex_ST.xy + _Normal_Noise_Tex_ST.zw;
				float2 panner399 = ( 1.0 * _Time.y * appendResult397 + uv_Normal_Noise_Tex);
				float2 panner307 = ( 1.0 * _Time.y * appendResult304 + ( ( (UnpackNormalScale( tex2D( _Normal_Noise_Tex, panner399 ), 1.0f )).xyz * _Noise_Str ) + float3( uv_Noise_Tex ,  0.0 ) ).xy);
				float4 temp_output_331_0 = ( ( pow( tex2D( _Noise_Tex, panner307 ).r , _Noiise_Power ) * _Noise_Ins ) * IN.color );
				float4 break340 = ( ( ( 1.0 - temp_output_358_0 ) * _Color_A * temp_output_331_0 ) + ( temp_output_358_0 * _Color_B * temp_output_331_0 ) );
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = IN.texCoord0.xy * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2D( _Normal_Main_Tex, panner388 ), 1.0f );
				float2 uv_Dissolve_Tex = IN.texCoord0.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float4 appendResult341 = (float4(break340.r , break340.g , break340.b , ( IN.color.a * saturate( ( tex2D( _Main_Tex, ( float3( uv_Main_Tex ,  0.0 ) + ( (tex2DNode384).xyz * _Normal_Main_Str ) ).xy ).r * saturate( step( 0.1 , ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.texCoord0.z ) ) ) ) ) )));
				
				float4 Color = appendResult341;

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

			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
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

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_COLOR


			sampler2D _Normal_Main_Tex;
			sampler2D _Noise_Tex;
			sampler2D _Normal_Noise_Tex;
			sampler2D _Main_Tex;
			sampler2D _Dissolve_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Normal_Main_Tex_ST;
			float4 _Color_B;
			float4 _Noise_Tex_ST;
			float4 _Color_A;
			float4 _Normal_Noise_Tex_ST;
			float _Noise_Ins;
			float _Noiise_Power;
			float _Noise_Str;
			float _Normal_Main__Upanner2;
			float _Normal_Main_Str;
			float _Normal_Main_Vpanner;
			float _Normal_Main__Upanner;
			float _Vertex_Val;
			float _Normal_Main_Vpanner1;
			float _Normal_Main_Vpanner2;
			float _Normal_Main_Upanner;
			CBUFFER_END


			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
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

				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = v.uv0.xy * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2Dlod( _Normal_Main_Tex, float4( panner388, 0, 0.0) ), 1.0f );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = ( tex2DNode384 * ( v.normal * _Vertex_Val ) );
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

				float2 uv_Noise_Tex = IN.texCoord0.xy * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
				float smoothstepResult355 = smoothstep( 0.25 , 0.4 , uv_Noise_Tex.y);
				float smoothstepResult352 = smoothstep( 0.15 , 0.1 , uv_Noise_Tex.y);
				float temp_output_358_0 = ( smoothstepResult355 + smoothstepResult352 );
				float2 appendResult304 = (float2(_Normal_Main__Upanner , _Normal_Main_Vpanner));
				float2 appendResult397 = (float2(_Normal_Main__Upanner2 , _Normal_Main_Vpanner2));
				float2 uv_Normal_Noise_Tex = IN.texCoord0.xy * _Normal_Noise_Tex_ST.xy + _Normal_Noise_Tex_ST.zw;
				float2 panner399 = ( 1.0 * _Time.y * appendResult397 + uv_Normal_Noise_Tex);
				float2 panner307 = ( 1.0 * _Time.y * appendResult304 + ( ( (UnpackNormalScale( tex2D( _Normal_Noise_Tex, panner399 ), 1.0f )).xyz * _Noise_Str ) + float3( uv_Noise_Tex ,  0.0 ) ).xy);
				float4 temp_output_331_0 = ( ( pow( tex2D( _Noise_Tex, panner307 ).r , _Noiise_Power ) * _Noise_Ins ) * IN.color );
				float4 break340 = ( ( ( 1.0 - temp_output_358_0 ) * _Color_A * temp_output_331_0 ) + ( temp_output_358_0 * _Color_B * temp_output_331_0 ) );
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = IN.texCoord0.xy * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2D( _Normal_Main_Tex, panner388 ), 1.0f );
				float2 uv_Dissolve_Tex = IN.texCoord0.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float4 appendResult341 = (float4(break340.r , break340.g , break340.b , ( IN.color.a * saturate( ( tex2D( _Main_Tex, ( float3( uv_Main_Tex ,  0.0 ) + ( (tex2DNode384).xyz * _Normal_Main_Str ) ).xy ).r * saturate( step( 0.1 , ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.texCoord0.z ) ) ) ) ) )));
				
				float4 Color = appendResult341;

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

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_COLOR


			sampler2D _Normal_Main_Tex;
			sampler2D _Noise_Tex;
			sampler2D _Normal_Noise_Tex;
			sampler2D _Main_Tex;
			sampler2D _Dissolve_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Normal_Main_Tex_ST;
			float4 _Color_B;
			float4 _Noise_Tex_ST;
			float4 _Color_A;
			float4 _Normal_Noise_Tex_ST;
			float _Noise_Ins;
			float _Noiise_Power;
			float _Noise_Str;
			float _Normal_Main__Upanner2;
			float _Normal_Main_Str;
			float _Normal_Main_Vpanner;
			float _Normal_Main__Upanner;
			float _Vertex_Val;
			float _Normal_Main_Vpanner1;
			float _Normal_Main_Vpanner2;
			float _Normal_Main_Upanner;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
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

				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = v.ase_texcoord * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2Dlod( _Normal_Main_Tex, float4( panner388, 0, 0.0) ), 1.0f );
				
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( tex2DNode384 * ( v.normal * _Vertex_Val ) );
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
				float2 uv_Noise_Tex = IN.ase_texcoord.xy * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
				float smoothstepResult355 = smoothstep( 0.25 , 0.4 , uv_Noise_Tex.y);
				float smoothstepResult352 = smoothstep( 0.15 , 0.1 , uv_Noise_Tex.y);
				float temp_output_358_0 = ( smoothstepResult355 + smoothstepResult352 );
				float2 appendResult304 = (float2(_Normal_Main__Upanner , _Normal_Main_Vpanner));
				float2 appendResult397 = (float2(_Normal_Main__Upanner2 , _Normal_Main_Vpanner2));
				float2 uv_Normal_Noise_Tex = IN.ase_texcoord.xy * _Normal_Noise_Tex_ST.xy + _Normal_Noise_Tex_ST.zw;
				float2 panner399 = ( 1.0 * _Time.y * appendResult397 + uv_Normal_Noise_Tex);
				float2 panner307 = ( 1.0 * _Time.y * appendResult304 + ( ( (UnpackNormalScale( tex2D( _Normal_Noise_Tex, panner399 ), 1.0f )).xyz * _Noise_Str ) + float3( uv_Noise_Tex ,  0.0 ) ).xy);
				float4 temp_output_331_0 = ( ( pow( tex2D( _Noise_Tex, panner307 ).r , _Noiise_Power ) * _Noise_Ins ) * IN.ase_color );
				float4 break340 = ( ( ( 1.0 - temp_output_358_0 ) * _Color_A * temp_output_331_0 ) + ( temp_output_358_0 * _Color_B * temp_output_331_0 ) );
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = IN.ase_texcoord.xy * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2D( _Normal_Main_Tex, panner388 ), 1.0f );
				float2 uv_Dissolve_Tex = IN.ase_texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float4 appendResult341 = (float4(break340.r , break340.g , break340.b , ( IN.ase_color.a * saturate( ( tex2D( _Main_Tex, ( float3( uv_Main_Tex ,  0.0 ) + ( (tex2DNode384).xyz * _Normal_Main_Str ) ).xy ).r * saturate( step( 0.1 , ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.ase_texcoord.z ) ) ) ) ) )));
				
				float4 Color = appendResult341;

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

        	#define ASE_NEEDS_VERT_NORMAL
        	#define ASE_NEEDS_FRAG_COLOR


			sampler2D _Normal_Main_Tex;
			sampler2D _Noise_Tex;
			sampler2D _Normal_Noise_Tex;
			sampler2D _Main_Tex;
			sampler2D _Dissolve_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float4 _Normal_Main_Tex_ST;
			float4 _Color_B;
			float4 _Noise_Tex_ST;
			float4 _Color_A;
			float4 _Normal_Noise_Tex_ST;
			float _Noise_Ins;
			float _Noiise_Power;
			float _Noise_Str;
			float _Normal_Main__Upanner2;
			float _Normal_Main_Str;
			float _Normal_Main_Vpanner;
			float _Normal_Main__Upanner;
			float _Vertex_Val;
			float _Normal_Main_Vpanner1;
			float _Normal_Main_Vpanner2;
			float _Normal_Main_Upanner;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
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

				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = v.ase_texcoord * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2Dlod( _Normal_Main_Tex, float4( panner388, 0, 0.0) ), 1.0f );
				
				o.ase_texcoord = v.ase_texcoord;
				o.ase_color = v.ase_color;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( tex2DNode384 * ( v.normal * _Vertex_Val ) );
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
				float2 uv_Noise_Tex = IN.ase_texcoord.xy * _Noise_Tex_ST.xy + _Noise_Tex_ST.zw;
				float smoothstepResult355 = smoothstep( 0.25 , 0.4 , uv_Noise_Tex.y);
				float smoothstepResult352 = smoothstep( 0.15 , 0.1 , uv_Noise_Tex.y);
				float temp_output_358_0 = ( smoothstepResult355 + smoothstepResult352 );
				float2 appendResult304 = (float2(_Normal_Main__Upanner , _Normal_Main_Vpanner));
				float2 appendResult397 = (float2(_Normal_Main__Upanner2 , _Normal_Main_Vpanner2));
				float2 uv_Normal_Noise_Tex = IN.ase_texcoord.xy * _Normal_Noise_Tex_ST.xy + _Normal_Noise_Tex_ST.zw;
				float2 panner399 = ( 1.0 * _Time.y * appendResult397 + uv_Normal_Noise_Tex);
				float2 panner307 = ( 1.0 * _Time.y * appendResult304 + ( ( (UnpackNormalScale( tex2D( _Normal_Noise_Tex, panner399 ), 1.0f )).xyz * _Noise_Str ) + float3( uv_Noise_Tex ,  0.0 ) ).xy);
				float4 temp_output_331_0 = ( ( pow( tex2D( _Noise_Tex, panner307 ).r , _Noiise_Power ) * _Noise_Ins ) * IN.ase_color );
				float4 break340 = ( ( ( 1.0 - temp_output_358_0 ) * _Color_A * temp_output_331_0 ) + ( temp_output_358_0 * _Color_B * temp_output_331_0 ) );
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult386 = (float2(_Normal_Main_Upanner , _Normal_Main_Vpanner1));
				float2 uv_Normal_Main_Tex = IN.ase_texcoord.xy * _Normal_Main_Tex_ST.xy + _Normal_Main_Tex_ST.zw;
				float2 panner388 = ( 1.0 * _Time.y * appendResult386 + uv_Normal_Main_Tex);
				float3 tex2DNode384 = UnpackNormalScale( tex2D( _Normal_Main_Tex, panner388 ), 1.0f );
				float2 uv_Dissolve_Tex = IN.ase_texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float4 appendResult341 = (float4(break340.r , break340.g , break340.b , ( IN.ase_color.a * saturate( ( tex2D( _Main_Tex, ( float3( uv_Main_Tex ,  0.0 ) + ( (tex2DNode384).xyz * _Normal_Main_Str ) ).xy ).r * saturate( step( 0.1 , ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.ase_texcoord.z ) ) ) ) ) )));
				
				float4 Color = appendResult341;
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
Node;AmplifyShaderEditor.CommentaryNode;413;-192.4819,1205.445;Inherit;False;607.217;383.8408;Comment;3;406;407;383;vertex_offset_set;0.9024602,0.6839622,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;412;-564.887,651.9698;Inherit;False;1059.25;489.1605;Comment;6;318;317;319;370;372;373;dissolve_set;0.5990566,0.9124255,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;411;-576.11,261.682;Inherit;False;1264.256;341.9852;Comment;5;338;337;403;404;339;main_tex_set;0.8094162,1,0.5424528,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;410;-2151.497,275.1566;Inherit;False;1556.071;384.0525;Comment;9;386;388;389;385;387;384;401;402;390;slash_normal_set;1,0.9812674,0.6745283,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;409;-1993.575,-369.823;Inherit;False;2500.541;607.7101;Comment;22;270;271;272;329;332;333;328;392;394;393;395;304;306;307;305;396;397;398;399;400;391;330;normal_noise_set;1,0.7598112,0.4198113,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;408;-691.3427,-1004.785;Inherit;False;1853.031;616.0989;Comment;13;367;347;348;364;352;354;355;356;357;353;309;358;366;color_set;1,0.5424528,0.5424528,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;302;808.8247,409.5975;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;368;1227.737,-425.2036;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;367;907.5755,-906.1683;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;347;624.965,-937.8248;Inherit;False;Property;_Color_A;Color_A;12;1;[HDR];Create;True;0;0;0;False;0;False;1.932916,1.932916,1.932916,1;1.081806,1.135301,0.4041911,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;348;619.1544,-698.7181;Inherit;False;Property;_Color_B;Color_B;13;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;0.1938129,0.3301887,0.05451228,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;364;927.6888,-642.6863;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;352;-241.7427,-704.2559;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;354;-418.3618,-643.7881;Inherit;False;Constant;_Float1;Float 1;8;0;Create;True;0;0;0;False;0;False;0.15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;355;-233.9491,-936.0997;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;356;-409.5682,-954.7852;Inherit;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;0;False;0;False;0.4;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;357;-410.5682,-875.632;Inherit;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;0;False;0;False;0.25;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;353;-417.3618,-721.7881;Inherit;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;309;-641.3427,-562.1782;Inherit;False;0;328;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;358;100.1311,-779.6274;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;366;399.0277,-836.1363;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;270;193.9,145.2001;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;2;5;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;271;193.9,145.2001;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;272;193.9,145.2001;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.PowerNode;329;128.2606,-11.93862;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;332;-60.94775,124.887;Inherit;False;Property;_Noiise_Power;Noiise_Power;2;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;333;160.4022,124.887;Inherit;False;Property;_Noise_Ins;Noise_Ins;3;0;Create;True;0;0;0;False;0;False;1;1.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;328;-205.876,-68.91271;Inherit;True;Property;_Noise_Tex;Noise_Tex;0;0;Create;True;0;0;0;False;0;False;-1;a910e3d4643d63d4f82881cf3c011741;a910e3d4643d63d4f82881cf3c011741;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;392;-712.3371,-104.0237;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;394;-959.3369,-150.8237;Inherit;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;393;-995.7367,-61.12373;Inherit;False;Property;_Noise_Str;Noise_Str;14;0;Create;True;0;0;0;False;0;False;0;0.27;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;395;-1290.837,-239.2236;Inherit;True;Property;_Normal_Noise_Tex;Normal_Noise_Tex;15;0;Create;True;0;0;0;False;0;False;-1;None;eb8166f070234134e84a814d6b0d460c;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;304;-623.2041,33.16925;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;306;-874.9199,15.18945;Inherit;False;Property;_Normal_Main__Upanner;Normal_Main__Upanner;5;0;Create;True;0;0;0;False;0;False;-0.1;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;307;-412.32,-12.9388;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;305;-876.1981,101.5628;Inherit;False;Property;_Normal_Main_Vpanner;Normal_Main_Vpanner;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;396;-1943.575,-84.80598;Inherit;False;Property;_Normal_Main_Vpanner2;Normal_Main_Vpanner;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;397;-1690.581,-153.1996;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;398;-1942.297,-171.1794;Inherit;False;Property;_Normal_Main__Upanner2;Normal_Main__Upanner;6;0;Create;True;0;0;0;False;0;False;-0.1;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;399;-1479.697,-199.3076;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;400;-1787.437,-319.823;Inherit;False;0;395;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;391;-541.5405,-100.9045;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;330;328.9651,-10.94892;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;386;-1832.095,472.6616;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;388;-1621.211,426.5535;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;389;-1910.959,325.1566;Inherit;False;0;384;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;385;-2085.089,541.0552;Inherit;False;Property;_Normal_Main_Vpanner1;Normal_Main_Vpanner;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;387;-2101.498,454.6819;Inherit;False;Property;_Normal_Main_Upanner;Normal_Main_Upanner;7;0;Create;True;0;0;0;False;0;False;-0.1;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;401;-773.4261,456.5091;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;402;-1071.584,546.2092;Inherit;False;Property;_Normal_Main_Str;Normal_Main_Str;16;0;Create;True;0;0;0;False;0;False;0;0.15;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;390;-1030.553,349.0267;Inherit;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;384;-1414.767,373.3697;Inherit;True;Property;_Normal_Main_Tex;Normal_Main_Tex;1;0;Create;True;0;0;0;False;0;False;-1;a910e3d4643d63d4f82881cf3c011741;eb8166f070234134e84a814d6b0d460c;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;403;-228.7207,364.0005;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;404;-526.1101,311.682;Inherit;False;0;337;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;318;-161.2068,765.8066;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;317;-514.887,732.8179;Inherit;True;Property;_Dissolve_Tex;Dissolve_Tex;11;0;Create;True;0;0;0;False;0;False;-1;a773b59055b36684684ab39588736590;a773b59055b36684684ab39588736590;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;319;-454.4379,932.1304;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;370;-92.13412,701.9698;Inherit;False;Constant;_Float4;Float 4;9;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;372;73.26289,752.1446;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;405;639.3455,1193.347;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;407;-142.4819,1461.645;Inherit;False;Property;_Vertex_Val;Vertex_Val;17;0;Create;True;0;0;0;False;0;False;0;0.3;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;383;-117.3632,1255.445;Inherit;True;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;406;181.7351,1335.286;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;331;774.3998,-30.88141;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;239;583.1319,93.75517;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;337;-75.10654,346.5903;Inherit;True;Property;_Main_Tex;Main_Tex;4;0;Create;True;0;0;0;False;0;False;-1;699f33833e9e70d4ba02c81b6615316e;699f33833e9e70d4ba02c81b6615316e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;338;286.7127,347.6621;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;373;167.9811,683.4172;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;339;540.7068,410.5835;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;340;1545.44,-318.6559;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;341;1762.379,-315.1951;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;269;2019.989,522.0324;Float;False;True;-1;2;ASEMaterialInspector;0;15;Amplify_Shader/Vfx/Fx_Slash_Dissolve_alp;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;True;True;2;5;False;;10;False;;2;5;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
WireConnection;302;0;239;4
WireConnection;302;1;339;0
WireConnection;368;0;367;0
WireConnection;368;1;364;0
WireConnection;367;0;366;0
WireConnection;367;1;347;0
WireConnection;367;2;331;0
WireConnection;364;0;358;0
WireConnection;364;1;348;0
WireConnection;364;2;331;0
WireConnection;352;0;309;2
WireConnection;352;1;354;0
WireConnection;352;2;353;0
WireConnection;355;0;309;2
WireConnection;355;1;357;0
WireConnection;355;2;356;0
WireConnection;358;0;355;0
WireConnection;358;1;352;0
WireConnection;366;0;358;0
WireConnection;329;0;328;1
WireConnection;329;1;332;0
WireConnection;328;1;307;0
WireConnection;392;0;394;0
WireConnection;392;1;393;0
WireConnection;394;0;395;0
WireConnection;395;1;399;0
WireConnection;304;0;306;0
WireConnection;304;1;305;0
WireConnection;307;0;391;0
WireConnection;307;2;304;0
WireConnection;397;0;398;0
WireConnection;397;1;396;0
WireConnection;399;0;400;0
WireConnection;399;2;397;0
WireConnection;391;0;392;0
WireConnection;391;1;309;0
WireConnection;330;0;329;0
WireConnection;330;1;333;0
WireConnection;386;0;387;0
WireConnection;386;1;385;0
WireConnection;388;0;389;0
WireConnection;388;2;386;0
WireConnection;401;0;390;0
WireConnection;401;1;402;0
WireConnection;390;0;384;0
WireConnection;384;1;388;0
WireConnection;403;0;404;0
WireConnection;403;1;401;0
WireConnection;318;0;317;1
WireConnection;318;1;319;3
WireConnection;372;0;370;0
WireConnection;372;1;318;0
WireConnection;405;0;384;0
WireConnection;405;1;406;0
WireConnection;406;0;383;0
WireConnection;406;1;407;0
WireConnection;331;0;330;0
WireConnection;331;1;239;0
WireConnection;337;1;403;0
WireConnection;338;0;337;1
WireConnection;338;1;373;0
WireConnection;373;0;372;0
WireConnection;339;0;338;0
WireConnection;340;0;368;0
WireConnection;341;0;340;0
WireConnection;341;1;340;1
WireConnection;341;2;340;2
WireConnection;341;3;302;0
WireConnection;269;1;341;0
WireConnection;269;3;405;0
ASEEND*/
//CHKSM=A9299CE70958B25909D24E91211C5A17C1AB4D28