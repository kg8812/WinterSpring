// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify_Shader/Vfx/FXS_Shock_Wave"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[Enum(UnityEngine.Rendering.CullMode)]_Cullmode("Cullmode", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_SRC("SRC", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)]_DST("DST", Float) = 0
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Ins("Main_Ins", Float) = 1
		_Main_Power("Main_Power", Float) = 1
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,1)
		_Sub_Tex("Sub_Tex", 2D) = "white" {}
		_Sub_Ins("Sub_Ins", Float) = 1
		_Sub_Power("Sub_Power", Float) = 1
		[HDR]_Sub_Color("Sub_Color", Color) = (1,1,1,1)
		_Sub_panner("Sub_panner", Vector) = (0,0,0,0)
		_Distortion_Tex("Distortion_Tex", 2D) = "bump" {}
		_Noise_Value("Noise_Value", Float) = 0.25
		_Noise_Speed("Noise_Speed", Vector) = (0,0,0,0)
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Mask_Power("Mask_Power", Float) = 1
		_Dissolve_Tex("Dissolve_Tex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull [_Cullmode]
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
			sampler2D _Distortion_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Color;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Mask_Tex_ST;
			float4 _Distortion_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Noise_Speed;
			float2 _Sub_panner;
			float _Main_Ins;
			float _Main_Power;
			float _Cullmode;
			float _Sub_Ins;
			float _Sub_Power;
			float _DST;
			float _SRC;
			float _Noise_Value;
			float _Mask_Power;
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

				float2 uv_Sub_Tex = IN.texCoord0.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner58 = ( 1.0 * _Time.y * _Sub_panner + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float2 uv_Distortion_Tex = IN.texCoord0.xy * _Distortion_Tex_ST.xy + _Distortion_Tex_ST.zw;
				float2 panner2 = ( 1.0 * _Time.y * _Noise_Speed + uv_Distortion_Tex);
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult21 = (float2(uv_Main_Tex.x , ( uv_Main_Tex.y + IN.texCoord0.z )));
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Distortion_Tex, panner2 ), 1.0f )).xy * _Noise_Value ) + appendResult21 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				float4 break44 = ( ( ( pow( tex2D( _Sub_Tex, panner58 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color ) * ( ( ( pow( tex2DNode1 , temp_cast_1 ) * _Main_Ins ) * _Main_Color ) * IN.color ) );
				float2 uv_Dissolve_Tex = IN.texCoord0.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float4 appendResult43 = (float4(break44.r , break44.g , break44.b , ( IN.color.a * ( tex2DNode1.r * ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.texCoord0.w ) * pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) ) )));
				
				float4 Color = appendResult43;

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
			sampler2D _Distortion_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Color;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Mask_Tex_ST;
			float4 _Distortion_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Noise_Speed;
			float2 _Sub_panner;
			float _Main_Ins;
			float _Main_Power;
			float _Cullmode;
			float _Sub_Ins;
			float _Sub_Power;
			float _DST;
			float _SRC;
			float _Noise_Value;
			float _Mask_Power;
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

				float2 uv_Sub_Tex = IN.texCoord0.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner58 = ( 1.0 * _Time.y * _Sub_panner + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float2 uv_Distortion_Tex = IN.texCoord0.xy * _Distortion_Tex_ST.xy + _Distortion_Tex_ST.zw;
				float2 panner2 = ( 1.0 * _Time.y * _Noise_Speed + uv_Distortion_Tex);
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult21 = (float2(uv_Main_Tex.x , ( uv_Main_Tex.y + IN.texCoord0.z )));
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Distortion_Tex, panner2 ), 1.0f )).xy * _Noise_Value ) + appendResult21 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				float4 break44 = ( ( ( pow( tex2D( _Sub_Tex, panner58 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color ) * ( ( ( pow( tex2DNode1 , temp_cast_1 ) * _Main_Ins ) * _Main_Color ) * IN.color ) );
				float2 uv_Dissolve_Tex = IN.texCoord0.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float4 appendResult43 = (float4(break44.r , break44.g , break44.b , ( IN.color.a * ( tex2DNode1.r * ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.texCoord0.w ) * pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) ) )));
				
				float4 Color = appendResult43;

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
			sampler2D _Distortion_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Color;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Mask_Tex_ST;
			float4 _Distortion_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Noise_Speed;
			float2 _Sub_panner;
			float _Main_Ins;
			float _Main_Power;
			float _Cullmode;
			float _Sub_Ins;
			float _Sub_Power;
			float _DST;
			float _SRC;
			float _Noise_Value;
			float _Mask_Power;
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

				o.ase_texcoord = v.ase_texcoord;
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
				float2 uv_Sub_Tex = IN.ase_texcoord.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner58 = ( 1.0 * _Time.y * _Sub_panner + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float2 uv_Distortion_Tex = IN.ase_texcoord.xy * _Distortion_Tex_ST.xy + _Distortion_Tex_ST.zw;
				float2 panner2 = ( 1.0 * _Time.y * _Noise_Speed + uv_Distortion_Tex);
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult21 = (float2(uv_Main_Tex.x , ( uv_Main_Tex.y + IN.ase_texcoord.z )));
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Distortion_Tex, panner2 ), 1.0f )).xy * _Noise_Value ) + appendResult21 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				float4 break44 = ( ( ( pow( tex2D( _Sub_Tex, panner58 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color ) * ( ( ( pow( tex2DNode1 , temp_cast_1 ) * _Main_Ins ) * _Main_Color ) * IN.ase_color ) );
				float2 uv_Dissolve_Tex = IN.ase_texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 uv_Mask_Tex = IN.ase_texcoord.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float4 appendResult43 = (float4(break44.r , break44.g , break44.b , ( IN.ase_color.a * ( tex2DNode1.r * ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.ase_texcoord.w ) * pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) ) )));
				
				float4 Color = appendResult43;

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
			sampler2D _Distortion_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Dissolve_Tex_ST;
			float4 _Main_Color;
			float4 _Sub_Tex_ST;
			float4 _Sub_Color;
			float4 _Mask_Tex_ST;
			float4 _Distortion_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Noise_Speed;
			float2 _Sub_panner;
			float _Main_Ins;
			float _Main_Power;
			float _Cullmode;
			float _Sub_Ins;
			float _Sub_Power;
			float _DST;
			float _SRC;
			float _Noise_Value;
			float _Mask_Power;
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

				o.ase_texcoord = v.ase_texcoord;
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
				float2 uv_Sub_Tex = IN.ase_texcoord.xy * _Sub_Tex_ST.xy + _Sub_Tex_ST.zw;
				float2 panner58 = ( 1.0 * _Time.y * _Sub_panner + uv_Sub_Tex);
				float4 temp_cast_0 = (_Sub_Power).xxxx;
				float2 uv_Distortion_Tex = IN.ase_texcoord.xy * _Distortion_Tex_ST.xy + _Distortion_Tex_ST.zw;
				float2 panner2 = ( 1.0 * _Time.y * _Noise_Speed + uv_Distortion_Tex);
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float2 appendResult21 = (float2(uv_Main_Tex.x , ( uv_Main_Tex.y + IN.ase_texcoord.z )));
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Distortion_Tex, panner2 ), 1.0f )).xy * _Noise_Value ) + appendResult21 ) );
				float4 temp_cast_1 = (_Main_Power).xxxx;
				float4 break44 = ( ( ( pow( tex2D( _Sub_Tex, panner58 ) , temp_cast_0 ) * _Sub_Ins ) * _Sub_Color ) * ( ( ( pow( tex2DNode1 , temp_cast_1 ) * _Main_Ins ) * _Main_Color ) * IN.ase_color ) );
				float2 uv_Dissolve_Tex = IN.ase_texcoord.xy * _Dissolve_Tex_ST.xy + _Dissolve_Tex_ST.zw;
				float2 uv_Mask_Tex = IN.ase_texcoord.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float4 appendResult43 = (float4(break44.r , break44.g , break44.b , ( IN.ase_color.a * ( tex2DNode1.r * ( tex2D( _Dissolve_Tex, uv_Dissolve_Tex ).r + IN.ase_texcoord.w ) * pow( tex2D( _Mask_Tex, uv_Mask_Tex ).r , _Mask_Power ) ) )));
				
				float4 Color = appendResult43;
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
Node;AmplifyShaderEditor.CommentaryNode;60;213.05,-364.3578;Inherit;False;1544.795;469.3791;Comment;10;52;53;55;50;49;51;58;57;59;62;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;48;2157.815,141.0239;Inherit;False;526.0679;168.816;Comment;3;45;46;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;23;-144.7985,708.7599;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;103.5868,671.5499;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-34.88108,354.5152;Float;False;Property;_Noise_Value;Noise_Value;13;0;Create;True;0;0;0;False;0;False;0.25;0.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;7;-72.54195,228.376;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;147.1188,303.8152;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;212.9349,541.9115;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;9;359.6834,405.1347;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;32;665.9838,944.4933;Inherit;True;Property;_Dissolve_Tex;Dissolve_Tex;17;0;Create;True;0;0;0;False;0;False;-1;None;41a89d3615ad8f34e9bd4fd043a70610;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;18;1344.866,488.0611;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;1701.692,912.8853;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;1576.856,325.4153;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;40;2169.835,434.5074;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;0;True;_SRC;0;True;_DST;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;41;2169.835,434.5074;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;42;2169.835,434.5074;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;39;2380.665,453.4095;Float;False;True;-1;2;ASEMaterialInspector;0;15;Amplify_Shader/Vfx/FXS_Shock_Wave;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;_Cullmode;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;True;True;2;0;True;_SRC;0;True;_DST;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;43;2163.744,370.267;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;44;1923.285,378.1875;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;45;2207.815,191.0239;Inherit;False;Property;_Cullmode;Cullmode;0;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;2359.03,195.3859;Inherit;False;Property;_SRC;SRC;1;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;2505.883,196.8399;Inherit;False;Property;_DST;DST;2;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.BlendMode;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;781.28,446.4543;Float;False;Property;_Main_Power;Main_Power;5;0;Create;True;0;0;0;False;0;False;1;3.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;540.3999,243.3999;Inherit;True;Property;_Main_Tex;Main_Tex;3;0;Create;True;0;0;0;False;0;False;-1;7e048301f35f19848a5fd8a776731009;7e048301f35f19848a5fd8a776731009;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;11;937.4014,296.2242;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;1176.773,315.9662;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;1347.047,338.1759;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;4;-795.6093,281.1445;Float;False;Property;_Noise_Speed;Noise_Speed;14;0;Create;True;0;0;0;False;0;False;0,0;0.1,-0.12;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-904.6093,89.14448;Inherit;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-618.6093,139.1445;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;5;-419.6092,135.1445;Inherit;True;Property;_Distortion_Tex;Distortion_Tex;12;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-151.1805,525.0022;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;52;1170.199,-163.2519;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;1409.571,-143.5099;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;50;773.1965,-216.0762;Inherit;True;Property;_Sub_Tex;Sub_Tex;7;0;Create;True;0;0;0;False;0;False;-1;a20772110376eb34e9ef28a5e5cad6f8;a20772110376eb34e9ef28a5e5cad6f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;1014.077,-13.02179;Float;False;Property;_Sub_Power;Sub_Power;9;0;Create;True;0;0;0;False;0;False;1;3.19;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;1229.678,-7.9787;Float;False;Property;_Sub_Ins;Sub_Ins;8;0;Create;True;0;0;0;False;0;False;1;2.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;58;534.7822,-204.257;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;1072.587,1039.61;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;998.4487,454.6346;Float;False;Property;_Main_Ins;Main_Ins;4;0;Create;True;0;0;0;False;0;False;1;2.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;61;1062.941,131.642;Inherit;False;Property;_Main_Color;Main_Color;6;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;62;1160.192,-359.3194;Inherit;False;Property;_Sub_Color;Sub_Color;10;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;65;1413.307,1273.041;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;1135.556,1399.739;Inherit;False;Property;_Mask_Power;Mask_Power;16;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;1027.314,1184.405;Inherit;True;Property;_Mask_Tex;Mask_Tex;15;0;Create;True;0;0;0;False;0;False;-1;14eb01569c7174f48bf649760eac7654;14eb01569c7174f48bf649760eac7654;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;1963.708,783.7307;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;1074.626,680.5283;Float;False;Property;_Float0;Float 0;18;0;Create;True;0;0;0;False;0;False;0.76;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;1262.628,766.5675;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;27;1521.033,771.8384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;1004.652,792.4859;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;1697.431,765.255;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;1566.845,-164.2002;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;1650.546,105.0238;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;57;263.05,-233.8903;Inherit;False;0;50;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;59;337.1355,-84.98169;Inherit;False;Property;_Sub_panner;Sub_panner;11;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
WireConnection;22;0;19;2
WireConnection;22;1;23;3
WireConnection;7;0;5;0
WireConnection;6;0;7;0
WireConnection;6;1;8;0
WireConnection;21;0;19;1
WireConnection;21;1;22;0
WireConnection;9;0;6;0
WireConnection;9;1;21;0
WireConnection;38;0;1;1
WireConnection;38;1;33;0
WireConnection;38;2;65;0
WireConnection;17;0;13;0
WireConnection;17;1;18;0
WireConnection;39;1;43;0
WireConnection;43;0;44;0
WireConnection;43;1;44;1
WireConnection;43;2;44;2
WireConnection;43;3;37;0
WireConnection;44;0;68;0
WireConnection;1;1;9;0
WireConnection;11;0;1;0
WireConnection;11;1;14;0
WireConnection;12;0;11;0
WireConnection;12;1;15;0
WireConnection;13;0;12;0
WireConnection;13;1;61;0
WireConnection;2;0;3;0
WireConnection;2;2;4;0
WireConnection;5;1;2;0
WireConnection;52;0;50;0
WireConnection;52;1;49;0
WireConnection;53;0;52;0
WireConnection;53;1;51;0
WireConnection;50;1;58;0
WireConnection;58;0;57;0
WireConnection;58;2;59;0
WireConnection;33;0;32;1
WireConnection;33;1;23;4
WireConnection;65;0;63;1
WireConnection;65;1;66;0
WireConnection;37;0;18;4
WireConnection;37;1;38;0
WireConnection;24;0;26;0
WireConnection;24;1;25;2
WireConnection;27;0;24;0
WireConnection;28;1;27;0
WireConnection;55;0;53;0
WireConnection;55;1;62;0
WireConnection;68;0;55;0
WireConnection;68;1;17;0
ASEEND*/
//CHKSM=558E133D2B4308EE66A71BC3210E6B953508FB0D