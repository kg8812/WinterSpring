// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify_Shader/Vfx/FXS_AURA"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_SRC("SRC", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_DST("DST", Float) = 0
		_Widtrh_Range("Widtrh_Range", Float) = 2.77
		_Height_Range("Height_Range", Float) = 2.77
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Normal_Val("Normal_Val", Float) = 0
		_Normal_U_Panner("Normal_U_Panner", Float) = 0
		_Normal_V_Panner("Normal_V_Panner", Float) = 0
		_Dissolve_Tex("Dissolve_Tex", 2D) = "white" {}
		_Dissolve_Val("Dissolve_Val", Float) = 0
		_Dissolve_U_Panner("Dissolve_U_Panner", Float) = 0
		_Dissolve_V_Panner("Dissolve_V_Panner", Float) = 0
		_Main_Ins("Main_Ins", Float) = 1
		_Main_Power("Main_Power", Float) = 1
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,1)
		_Main_U_Panner("Main_U_Panner", Float) = 0
		_Main_V_Panner("Main_V_Panner", Float) = 0
		_Opacity("Opacity", Float) = 1
		_Sub_Tex("Sub_Tex", 2D) = "white" {}
		_Sub_Ins("Sub_Ins", Float) = 0
		_Sub_Power("Sub_Power", Float) = 0
		[HDR]_Sub_Color("Sub_Color", Color) = (1,1,1,1)
		_Sub_Upanner("Sub_Upanner", Float) = 0
		_Sub_Vpanner("Sub_Vpanner", Float) = 0
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Mask_Power("Mask_Power", Float) = 0

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


			sampler2D _Sub_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Main_Color;
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float _CullMode;
			float _Dissolve_Val;
			float _Mask_Power;
			float _Opacity;
			float _Main_Ins;
			float _Main_Power;
			float _Normal_Val;
			float _Normal_V_Panner;
			float _Normal_U_Panner;
			float _Main_V_Panner;
			float _Main_U_Panner;
			float _Widtrh_Range;
			float _Height_Range;
			float _Sub_Ins;
			float _Sub_Power;
			float _Sub_Vpanner;
			float _Sub_Upanner;
			float _DST;
			float _SRC;
			float _Dissolve_U_Panner;
			float _Dissolve_V_Panner;
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

				float2 appendResult50 = (float2(_Sub_Upanner , _Sub_Vpanner));
				float2 uv_Sub_Tex = IN.texCoord0.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float4 Sub_Tex49 = ( ( pow( tex2D( _Sub_Tex, panner52 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color );
				float2 texCoord1 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_20_0 = saturate( pow( ( 1.0 - saturate( abs( ( ( (texCoord1).x - 0.5 ) * 2.0 ) ) ) ) , _Widtrh_Range ) );
				float2 appendResult16 = (float2(_Main_U_Panner , _Main_V_Panner));
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * appendResult16 + uv_Main_Tex);
				float2 appendResult99 = (float2(_Normal_U_Panner , _Normal_V_Panner));
				float2 uv_Dissolve_Tex = IN.texCoord0.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 panner100 = ( 1.0 * _Time.y * appendResult99 + uv_Dissolve_Tex);
				float3 temp_output_95_0 = ( (UnpackNormalScale( tex2D( _Normal_Tex, panner100 ), 1.0f )).xyz * _Normal_Val );
				float temp_output_25_0 = ( saturate( pow( saturate( ( 1.0 - (texCoord1).y ) ) , _Height_Range ) ) * ( temp_output_20_0 * ( temp_output_20_0 + tex2D( _Main_Tex, ( float3( panner15 ,  0.0 ) + temp_output_95_0 ).xy ).r ) ) );
				float4 break66 = ( ( Sub_Tex49 + ( _Main_Color * ( pow( temp_output_25_0 , _Main_Power ) * _Main_Ins ) ) ) * IN.color );
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 appendResult86 = (float2(_Dissolve_U_Panner , _Dissolve_V_Panner));
				float2 panner88 = ( 1.0 * _Time.y * appendResult86 + uv_Dissolve_Tex);
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( IN.color.a * ( saturate( ( temp_output_25_0 * _Opacity ) ) * saturate( ( pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) * ( _Dissolve_Val + tex2D( _Dissolve_Tex, ( temp_output_95_0 + float3( panner88 ,  0.0 ) ).xy ).r ) ) ) ) )));
				
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


			sampler2D _Sub_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Main_Color;
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float _CullMode;
			float _Dissolve_Val;
			float _Mask_Power;
			float _Opacity;
			float _Main_Ins;
			float _Main_Power;
			float _Normal_Val;
			float _Normal_V_Panner;
			float _Normal_U_Panner;
			float _Main_V_Panner;
			float _Main_U_Panner;
			float _Widtrh_Range;
			float _Height_Range;
			float _Sub_Ins;
			float _Sub_Power;
			float _Sub_Vpanner;
			float _Sub_Upanner;
			float _DST;
			float _SRC;
			float _Dissolve_U_Panner;
			float _Dissolve_V_Panner;
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

				float2 appendResult50 = (float2(_Sub_Upanner , _Sub_Vpanner));
				float2 uv_Sub_Tex = IN.texCoord0.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float4 Sub_Tex49 = ( ( pow( tex2D( _Sub_Tex, panner52 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color );
				float2 texCoord1 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_20_0 = saturate( pow( ( 1.0 - saturate( abs( ( ( (texCoord1).x - 0.5 ) * 2.0 ) ) ) ) , _Widtrh_Range ) );
				float2 appendResult16 = (float2(_Main_U_Panner , _Main_V_Panner));
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * appendResult16 + uv_Main_Tex);
				float2 appendResult99 = (float2(_Normal_U_Panner , _Normal_V_Panner));
				float2 uv_Dissolve_Tex = IN.texCoord0.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 panner100 = ( 1.0 * _Time.y * appendResult99 + uv_Dissolve_Tex);
				float3 temp_output_95_0 = ( (UnpackNormalScale( tex2D( _Normal_Tex, panner100 ), 1.0f )).xyz * _Normal_Val );
				float temp_output_25_0 = ( saturate( pow( saturate( ( 1.0 - (texCoord1).y ) ) , _Height_Range ) ) * ( temp_output_20_0 * ( temp_output_20_0 + tex2D( _Main_Tex, ( float3( panner15 ,  0.0 ) + temp_output_95_0 ).xy ).r ) ) );
				float4 break66 = ( ( Sub_Tex49 + ( _Main_Color * ( pow( temp_output_25_0 , _Main_Power ) * _Main_Ins ) ) ) * IN.color );
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 appendResult86 = (float2(_Dissolve_U_Panner , _Dissolve_V_Panner));
				float2 panner88 = ( 1.0 * _Time.y * appendResult86 + uv_Dissolve_Tex);
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( IN.color.a * ( saturate( ( temp_output_25_0 * _Opacity ) ) * saturate( ( pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) * ( _Dissolve_Val + tex2D( _Dissolve_Tex, ( temp_output_95_0 + float3( panner88 ,  0.0 ) ).xy ).r ) ) ) ) )));
				
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


			sampler2D _Sub_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Main_Color;
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float _CullMode;
			float _Dissolve_Val;
			float _Mask_Power;
			float _Opacity;
			float _Main_Ins;
			float _Main_Power;
			float _Normal_Val;
			float _Normal_V_Panner;
			float _Normal_U_Panner;
			float _Main_V_Panner;
			float _Main_U_Panner;
			float _Widtrh_Range;
			float _Height_Range;
			float _Sub_Ins;
			float _Sub_Power;
			float _Sub_Vpanner;
			float _Sub_Upanner;
			float _DST;
			float _SRC;
			float _Dissolve_U_Panner;
			float _Dissolve_V_Panner;
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

				o.ase_texcoord.xy = v.ase_texcoord.xy;
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
				float2 appendResult50 = (float2(_Sub_Upanner , _Sub_Vpanner));
				float2 uv_Sub_Tex = IN.ase_texcoord.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float4 Sub_Tex49 = ( ( pow( tex2D( _Sub_Tex, panner52 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color );
				float2 texCoord1 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_20_0 = saturate( pow( ( 1.0 - saturate( abs( ( ( (texCoord1).x - 0.5 ) * 2.0 ) ) ) ) , _Widtrh_Range ) );
				float2 appendResult16 = (float2(_Main_U_Panner , _Main_V_Panner));
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * appendResult16 + uv_Main_Tex);
				float2 appendResult99 = (float2(_Normal_U_Panner , _Normal_V_Panner));
				float2 uv_Dissolve_Tex = IN.ase_texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 panner100 = ( 1.0 * _Time.y * appendResult99 + uv_Dissolve_Tex);
				float3 temp_output_95_0 = ( (UnpackNormalScale( tex2D( _Normal_Tex, panner100 ), 1.0f )).xyz * _Normal_Val );
				float temp_output_25_0 = ( saturate( pow( saturate( ( 1.0 - (texCoord1).y ) ) , _Height_Range ) ) * ( temp_output_20_0 * ( temp_output_20_0 + tex2D( _Main_Tex, ( float3( panner15 ,  0.0 ) + temp_output_95_0 ).xy ).r ) ) );
				float4 break66 = ( ( Sub_Tex49 + ( _Main_Color * ( pow( temp_output_25_0 , _Main_Power ) * _Main_Ins ) ) ) * IN.ase_color );
				float2 uv_Mask_Tex = IN.ase_texcoord.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 appendResult86 = (float2(_Dissolve_U_Panner , _Dissolve_V_Panner));
				float2 panner88 = ( 1.0 * _Time.y * appendResult86 + uv_Dissolve_Tex);
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( IN.ase_color.a * ( saturate( ( temp_output_25_0 * _Opacity ) ) * saturate( ( pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) * ( _Dissolve_Val + tex2D( _Dissolve_Tex, ( temp_output_95_0 + float3( panner88 ,  0.0 ) ).xy ).r ) ) ) ) )));
				
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


			sampler2D _Sub_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Mask_Tex_ST;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Main_Color;
			float4 _Dissolve_Tex_ST;
			float4 _Main_Tex_ST;
			float _CullMode;
			float _Dissolve_Val;
			float _Mask_Power;
			float _Opacity;
			float _Main_Ins;
			float _Main_Power;
			float _Normal_Val;
			float _Normal_V_Panner;
			float _Normal_U_Panner;
			float _Main_V_Panner;
			float _Main_U_Panner;
			float _Widtrh_Range;
			float _Height_Range;
			float _Sub_Ins;
			float _Sub_Power;
			float _Sub_Vpanner;
			float _Sub_Upanner;
			float _DST;
			float _SRC;
			float _Dissolve_U_Panner;
			float _Dissolve_V_Panner;
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

				o.ase_texcoord.xy = v.ase_texcoord.xy;
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
				float2 appendResult50 = (float2(_Sub_Upanner , _Sub_Vpanner));
				float2 uv_Sub_Tex = IN.ase_texcoord.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner52 = ( 1.0 * _Time.y * appendResult50 + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float4 Sub_Tex49 = ( ( pow( tex2D( _Sub_Tex, panner52 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color );
				float2 texCoord1 = IN.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_20_0 = saturate( pow( ( 1.0 - saturate( abs( ( ( (texCoord1).x - 0.5 ) * 2.0 ) ) ) ) , _Widtrh_Range ) );
				float2 appendResult16 = (float2(_Main_U_Panner , _Main_V_Panner));
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * appendResult16 + uv_Main_Tex);
				float2 appendResult99 = (float2(_Normal_U_Panner , _Normal_V_Panner));
				float2 uv_Dissolve_Tex = IN.ase_texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 panner100 = ( 1.0 * _Time.y * appendResult99 + uv_Dissolve_Tex);
				float3 temp_output_95_0 = ( (UnpackNormalScale( tex2D( _Normal_Tex, panner100 ), 1.0f )).xyz * _Normal_Val );
				float temp_output_25_0 = ( saturate( pow( saturate( ( 1.0 - (texCoord1).y ) ) , _Height_Range ) ) * ( temp_output_20_0 * ( temp_output_20_0 + tex2D( _Main_Tex, ( float3( panner15 ,  0.0 ) + temp_output_95_0 ).xy ).r ) ) );
				float4 break66 = ( ( Sub_Tex49 + ( _Main_Color * ( pow( temp_output_25_0 , _Main_Power ) * _Main_Ins ) ) ) * IN.ase_color );
				float2 uv_Mask_Tex = IN.ase_texcoord.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 appendResult86 = (float2(_Dissolve_U_Panner , _Dissolve_V_Panner));
				float2 panner88 = ( 1.0 * _Time.y * appendResult86 + uv_Dissolve_Tex);
				float4 appendResult67 = (float4(break66.r , break66.g , break66.b , ( IN.ase_color.a * ( saturate( ( temp_output_25_0 * _Opacity ) ) * saturate( ( pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) * ( _Dissolve_Val + tex2D( _Dissolve_Tex, ( temp_output_95_0 + float3( panner88 ,  0.0 ) ).xy ).r ) ) ) ) )));
				
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
Node;AmplifyShaderEditor.CommentaryNode;104;-3754.997,745.7877;Inherit;False;1217.276;394.3054;Comment;8;93;94;96;99;100;103;101;102;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;90;-1336.261,504.9282;Inherit;False;974.4869;426;Comment;4;70;72;74;75;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;81;803.287,-374.1421;Inherit;False;232;315;Comment;3;77;79;80;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;69;-866,-338;Inherit;False;484;483;Comment;5;39;37;40;38;32;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;68;-2465.044,283.5208;Inherit;False;954;387;Comment;6;17;18;16;14;15;13;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;2;-2880,-16;Inherit;False;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-2864,80;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-2672,0;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2624,208;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-2464,16;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;7;-2336,0;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;36;-2032,64;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;46;-1403.127,-934.6152;Inherit;False;1796;499;Comment;13;59;58;57;56;55;54;53;52;51;50;49;48;47;Sub_Tex;1,0.8005763,0.5990566,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;10;-1824,0;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;22;-2032,-224;Inherit;True;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1680,144;Inherit;False;Property;_Widtrh_Range;Widtrh_Range;3;0;Create;True;0;0;0;False;0;False;2.77;2.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1355.127,-614.6152;Inherit;False;Property;_Sub_Upanner;Sub_Upanner;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1355.127,-534.6152;Inherit;False;Property;_Sub_Vpanner;Sub_Vpanner;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;23;-1744,-208;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;11;-1632,16;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;50;-1147.127,-614.6152;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;51;-1211.127,-758.6152;Inherit;False;0;53;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;20;-1456,64;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;24;-1568,-208;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1632,-96;Inherit;False;Property;_Height_Range;Height_Range;4;0;Create;True;0;0;0;False;0;False;2.77;2.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;52;-923.1269,-678.6152;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;26;-1408,-208;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;53;-699.1269,-694.6152;Inherit;True;Property;_Sub_Tex;Sub_Tex;20;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;-427.1269,-566.6152;Inherit;False;Property;_Sub_Power;Sub_Power;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1248,48;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;-1248,-192;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;55;-379.1269,-678.6152;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-235.1269,-566.6152;Inherit;False;Property;_Sub_Ins;Sub_Ins;21;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1024,-96;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-187.1269,-678.6152;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;58;-267.1269,-870.6152;Inherit;False;Property;_Sub_Color;Sub_Color;23;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-896,304;Inherit;False;Property;_Opacity;Opacity;19;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-11.12689,-678.6152;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-768,176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;49;180.8731,-726.6152;Inherit;False;Sub_Tex;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;43;-546.9796,191.4503;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-384,-80;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-336,-160;Inherit;False;49;Sub_Tex;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;31;-80,64;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-128,-80;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;144,-32;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;63;320,-32;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;True;_SRC;10;True;_DST;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;64;320,-32;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;65;320,-32;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;502.3263,-44.63831;Inherit;True;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;66;382.8012,-37.94507;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-3026.055,-218.3548;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;39;-816,32;Inherit;False;Property;_Main_Power;Main_Power;15;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;37;-752,-96;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-624,16;Inherit;False;Property;_Main_Ins;Main_Ins;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-560,-96;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-656,-288;Inherit;False;Property;_Main_Color;Main_Color;16;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;7.464264,7.464264,7.464264,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;144,288;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;62;822.8932,-32.27419;Float;False;True;-1;2;ASEMaterialInspector;0;15;Amplify_Shader/Vfx/FXS_AURA;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;_CullMode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;True;True;2;5;True;_SRC;10;True;_DST;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.RangedFloatNode;77;853.287,-324.1421;Inherit;False;Property;_CullMode;CullMode;0;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;853.287,-247.1421;Inherit;False;Property;_SRC;SRC;1;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;857.287,-172.1421;Inherit;False;Property;_DST;DST;2;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;0;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-2399.044,461.5208;Inherit;False;Property;_Main_U_Panner;Main_U_Panner;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-2415.044,557.5205;Inherit;False;Property;_Main_V_Panner;Main_V_Panner;18;0;Create;True;0;0;0;False;0;False;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;16;-2159.044,461.5208;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2335.044,333.5208;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;15;-2015.046,333.5208;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-1823.046,333.5208;Inherit;True;Property;_Main_Tex;Main_Tex;5;0;Create;True;0;0;0;False;0;False;-1;None;c2f5e06ce5d539b418dc5ebfbfeeee94;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;83;-1942.605,981.5505;Inherit;False;954;387;Comment;5;89;88;87;86;97;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-1286.261,631.4644;Inherit;False;0;70;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;74;-697.7738,642.928;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-915.774,817.928;Inherit;False;Property;_Mask_Power;Mask_Power;27;0;Create;True;0;0;0;False;0;False;0;0.99;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1390.198,291.6038;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;89;-1302.924,1057.03;Inherit;True;Property;_Dissolve_Tex;Dissolve_Tex;10;0;Create;True;0;0;0;False;0;False;-1;None;7ead229491461f64fb8abd86dfcd7d4b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;70;-997.5506,622.4107;Inherit;True;Property;_Mask_Tex;Mask_Tex;26;0;Create;True;0;0;0;False;0;False;-1;d442970fc0ee17040b1a610f36ece114;d442970fc0ee17040b1a610f36ece114;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-2552.3,837.8638;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;-1932.008,704.6525;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;86;-1953.938,1205.708;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;88;-1809.941,1077.709;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-2193.935,1205.708;Inherit;False;Property;_Dissolve_U_Panner;Dissolve_U_Panner;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-2209.935,1301.708;Inherit;False;Property;_Dissolve_V_Panner;Dissolve_V_Panner;13;0;Create;True;0;0;0;False;0;False;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;87;-2129.937,1079.136;Inherit;False;0;89;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;93;-3122.151,795.7877;Inherit;True;Property;_Normal_Tex;Normal_Tex;6;0;Create;True;0;0;0;False;0;False;-1;9d4f08fd5b544864fba35452ea823fb0;9d4f08fd5b544864fba35452ea823fb0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;94;-2810.981,813.0857;Inherit;False;True;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;99;-3449,931.0929;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;100;-3305.003,803.0938;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;103;-3624.999,804.5208;Inherit;False;0;89;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;101;-3690.439,931.0929;Inherit;False;Property;_Normal_U_Panner;Normal_U_Panner;8;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-3704.997,1027.093;Inherit;False;Property;_Normal_V_Panner;Normal_V_Panner;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-2715.72,930.7892;Inherit;False;Property;_Normal_Val;Normal_Val;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-1517.035,1107.717;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;105;-725.1721,1119.228;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-865.5721,1205.028;Inherit;False;Property;_Dissolve_Val;Dissolve_Val;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-487.3059,724.6979;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;108;-297.0439,675.2152;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-101.9612,417.5064;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
WireConnection;2;0;1;0
WireConnection;5;0;2;0
WireConnection;5;1;6;0
WireConnection;8;0;5;0
WireConnection;8;1;9;0
WireConnection;7;0;8;0
WireConnection;36;0;7;0
WireConnection;10;0;36;0
WireConnection;22;0;1;0
WireConnection;23;0;22;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;50;0;47;0
WireConnection;50;1;48;0
WireConnection;20;0;11;0
WireConnection;24;0;23;0
WireConnection;52;0;51;0
WireConnection;52;2;50;0
WireConnection;26;0;24;0
WireConnection;26;1;27;0
WireConnection;53;1;52;0
WireConnection;21;0;20;0
WireConnection;21;1;19;0
WireConnection;28;0;26;0
WireConnection;55;0;53;0
WireConnection;55;1;54;0
WireConnection;25;0;28;0
WireConnection;25;1;21;0
WireConnection;57;0;55;0
WireConnection;57;1;56;0
WireConnection;59;0;57;0
WireConnection;59;1;58;0
WireConnection;29;0;25;0
WireConnection;29;1;30;0
WireConnection;49;0;59;0
WireConnection;43;0;29;0
WireConnection;33;0;32;0
WireConnection;33;1;38;0
WireConnection;60;0;61;0
WireConnection;60;1;33;0
WireConnection;34;0;60;0
WireConnection;34;1;31;0
WireConnection;67;0;66;0
WireConnection;67;1;66;1
WireConnection;67;2;66;2
WireConnection;67;3;35;0
WireConnection;66;0;34;0
WireConnection;37;0;25;0
WireConnection;37;1;39;0
WireConnection;38;0;37;0
WireConnection;38;1;40;0
WireConnection;35;0;31;4
WireConnection;35;1;73;0
WireConnection;62;1;67;0
WireConnection;16;0;17;0
WireConnection;16;1;18;0
WireConnection;15;0;14;0
WireConnection;15;2;16;0
WireConnection;13;1;98;0
WireConnection;74;0;70;1
WireConnection;74;1;75;0
WireConnection;19;0;20;0
WireConnection;19;1;13;1
WireConnection;89;1;97;0
WireConnection;70;1;72;0
WireConnection;95;0;94;0
WireConnection;95;1;96;0
WireConnection;98;0;15;0
WireConnection;98;1;95;0
WireConnection;86;0;84;0
WireConnection;86;1;85;0
WireConnection;88;0;87;0
WireConnection;88;2;86;0
WireConnection;93;1;100;0
WireConnection;94;0;93;0
WireConnection;99;0;101;0
WireConnection;99;1;102;0
WireConnection;100;0;103;0
WireConnection;100;2;99;0
WireConnection;97;0;95;0
WireConnection;97;1;88;0
WireConnection;105;0;106;0
WireConnection;105;1;89;1
WireConnection;92;0;74;0
WireConnection;92;1;105;0
WireConnection;108;0;92;0
WireConnection;73;0;43;0
WireConnection;73;1;108;0
ASEEND*/
//CHKSM=E7CAF0B979A6385C57528121401C914CF57BCFE8