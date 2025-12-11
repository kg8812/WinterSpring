// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Amplify_Shader/Vfx/Fire"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Normal_Tex("Normal_Tex", 2D) = "bump" {}
		_Noise_Str("Noise_Str", Float) = 0.16
		_Noise_Upanner("Noise_Upanner", Float) = 0.2
		_Noise_Vpanner("Noise_Vpanner", Float) = 0
		_Emi_Dissolve_Tex("Emi_Dissolve_Tex", 2D) = "white" {}
		[HDR]_Color_B("Color_B", Color) = (2.401371,0.915374,0.05943987,1)
		[HDR]_Color_A("Color_A", Color) = (1,0,0,1)
		_Emi_Dissolve_Speed("Emi_Dissolve_Speed", Vector) = (0.1,0,0,0)
		_Color_offset("Color_offset", Float) = 10.94

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

			Blend SrcAlpha OneMinusSrcAlpha
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


			sampler2D _Emi_Dissolve_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Color_A;
			float4 _Color_B;
			float4 _Emi_Dissolve_Tex_ST;
			float4 _Normal_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Emi_Dissolve_Speed;
			float _Noise_Upanner;
			float _Noise_Vpanner;
			float _Noise_Str;
			float _Color_offset;
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

				float2 uv_Emi_Dissolve_Tex = IN.texCoord0.xy * _Emi_Dissolve_Tex_ST.xy + _Emi_Dissolve_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * _Emi_Dissolve_Speed + uv_Emi_Dissolve_Tex);
				float4 tex2DNode13 = tex2D( _Emi_Dissolve_Tex, panner15 );
				float2 appendResult10 = (float2(_Noise_Upanner , _Noise_Vpanner));
				float2 uv_Normal_Tex = IN.texCoord0.xy * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
				float2 panner9 = ( 1.0 * _Time.y * appendResult10 + uv_Normal_Tex);
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Normal_Tex, panner9 ), 1.0f )).xy * _Noise_Str ) + uv_Main_Tex ) );
				float4 lerpResult19 = lerp( _Color_A , _Color_B , saturate( pow( saturate( ( ( tex2DNode13.r + IN.ase_texcoord3.x ) * tex2DNode1.r ) ) , _Color_offset ) ));
				float4 break41 = ( lerpResult19 * IN.color );
				float4 appendResult40 = (float4(break41.r , break41.g , break41.b , ( IN.color.a * saturate( ( tex2DNode1.r * ( tex2DNode13.r + IN.ase_texcoord3.y ) ) ) )));
				
				float4 Color = appendResult40;

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

			Blend SrcAlpha OneMinusSrcAlpha
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


			sampler2D _Emi_Dissolve_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Color_A;
			float4 _Color_B;
			float4 _Emi_Dissolve_Tex_ST;
			float4 _Normal_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Emi_Dissolve_Speed;
			float _Noise_Upanner;
			float _Noise_Vpanner;
			float _Noise_Str;
			float _Color_offset;
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

				float2 uv_Emi_Dissolve_Tex = IN.texCoord0.xy * _Emi_Dissolve_Tex_ST.xy + _Emi_Dissolve_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * _Emi_Dissolve_Speed + uv_Emi_Dissolve_Tex);
				float4 tex2DNode13 = tex2D( _Emi_Dissolve_Tex, panner15 );
				float2 appendResult10 = (float2(_Noise_Upanner , _Noise_Vpanner));
				float2 uv_Normal_Tex = IN.texCoord0.xy * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
				float2 panner9 = ( 1.0 * _Time.y * appendResult10 + uv_Normal_Tex);
				float2 uv_Main_Tex = IN.texCoord0.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Normal_Tex, panner9 ), 1.0f )).xy * _Noise_Str ) + uv_Main_Tex ) );
				float4 lerpResult19 = lerp( _Color_A , _Color_B , saturate( pow( saturate( ( ( tex2DNode13.r + IN.ase_texcoord3.x ) * tex2DNode1.r ) ) , _Color_offset ) ));
				float4 break41 = ( lerpResult19 * IN.color );
				float4 appendResult40 = (float4(break41.r , break41.g , break41.b , ( IN.color.a * saturate( ( tex2DNode1.r * ( tex2DNode13.r + IN.ase_texcoord3.y ) ) ) )));
				
				float4 Color = appendResult40;

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


			sampler2D _Emi_Dissolve_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Color_A;
			float4 _Color_B;
			float4 _Emi_Dissolve_Tex_ST;
			float4 _Normal_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Emi_Dissolve_Speed;
			float _Noise_Upanner;
			float _Noise_Vpanner;
			float _Noise_Str;
			float _Color_offset;
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
				float2 uv_Emi_Dissolve_Tex = IN.ase_texcoord.xy * _Emi_Dissolve_Tex_ST.xy + _Emi_Dissolve_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * _Emi_Dissolve_Speed + uv_Emi_Dissolve_Tex);
				float4 tex2DNode13 = tex2D( _Emi_Dissolve_Tex, panner15 );
				float2 appendResult10 = (float2(_Noise_Upanner , _Noise_Vpanner));
				float2 uv_Normal_Tex = IN.ase_texcoord.xy * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
				float2 panner9 = ( 1.0 * _Time.y * appendResult10 + uv_Normal_Tex);
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Normal_Tex, panner9 ), 1.0f )).xy * _Noise_Str ) + uv_Main_Tex ) );
				float4 lerpResult19 = lerp( _Color_A , _Color_B , saturate( pow( saturate( ( ( tex2DNode13.r + IN.ase_texcoord1.x ) * tex2DNode1.r ) ) , _Color_offset ) ));
				float4 break41 = ( lerpResult19 * IN.ase_color );
				float4 appendResult40 = (float4(break41.r , break41.g , break41.b , ( IN.ase_color.a * saturate( ( tex2DNode1.r * ( tex2DNode13.r + IN.ase_texcoord1.y ) ) ) )));
				
				float4 Color = appendResult40;

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


			sampler2D _Emi_Dissolve_Tex;
			sampler2D _Main_Tex;
			sampler2D _Normal_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Color_A;
			float4 _Color_B;
			float4 _Emi_Dissolve_Tex_ST;
			float4 _Normal_Tex_ST;
			float4 _Main_Tex_ST;
			float2 _Emi_Dissolve_Speed;
			float _Noise_Upanner;
			float _Noise_Vpanner;
			float _Noise_Str;
			float _Color_offset;
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
				float2 uv_Emi_Dissolve_Tex = IN.ase_texcoord.xy * _Emi_Dissolve_Tex_ST.xy + _Emi_Dissolve_Tex_ST.zw;
				float2 panner15 = ( 1.0 * _Time.y * _Emi_Dissolve_Speed + uv_Emi_Dissolve_Tex);
				float4 tex2DNode13 = tex2D( _Emi_Dissolve_Tex, panner15 );
				float2 appendResult10 = (float2(_Noise_Upanner , _Noise_Vpanner));
				float2 uv_Normal_Tex = IN.ase_texcoord.xy * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw;
				float2 panner9 = ( 1.0 * _Time.y * appendResult10 + uv_Normal_Tex);
				float2 uv_Main_Tex = IN.ase_texcoord.xy * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
				float4 tex2DNode1 = tex2D( _Main_Tex, ( ( (UnpackNormalScale( tex2D( _Normal_Tex, panner9 ), 1.0f )).xy * _Noise_Str ) + uv_Main_Tex ) );
				float4 lerpResult19 = lerp( _Color_A , _Color_B , saturate( pow( saturate( ( ( tex2DNode13.r + IN.ase_texcoord1.x ) * tex2DNode1.r ) ) , _Color_offset ) ));
				float4 break41 = ( lerpResult19 * IN.ase_color );
				float4 appendResult40 = (float4(break41.r , break41.g , break41.b , ( IN.ase_color.a * saturate( ( tex2DNode1.r * ( tex2DNode13.r + IN.ase_texcoord1.y ) ) ) )));
				
				float4 Color = appendResult40;
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
Node;AmplifyShaderEditor.RangedFloatNode;12;-3025.925,-231.2589;Float;False;Property;_Noise_Vpanner;Noise_Vpanner;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-3026.925,-318.2589;Float;False;Property;_Noise_Upanner;Noise_Upanner;3;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-2901.925,-446.2589;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;10;-2794.925,-305.2589;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;9;-2631.925,-449.2589;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-2420.925,-488.2589;Inherit;True;Property;_Normal_Tex;Normal_Tex;1;0;Create;True;0;0;0;False;0;False;-1;None;645b0a2fda25d114599a2fba6417fe81;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;7;-2075.926,-340.2589;Float;False;Property;_Noise_Str;Noise_Str;2;0;Create;True;0;0;0;False;0;False;0.16;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;5;-2109.926,-489.2589;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1897.927,-420.2589;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-2005.927,-158.2589;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;2;-1737.927,-285.2589;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;27;-2393.555,-748.0403;Float;False;Property;_Emi_Dissolve_Speed;Emi_Dissolve_Speed;8;0;Create;True;0;0;0;False;0;False;0.1,0;0.05,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2438.044,-888.9073;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;15;-2179.344,-856.4073;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;13;-1943.077,-771.3981;Inherit;True;Property;_Emi_Dissolve_Tex;Emi_Dissolve_Tex;5;0;Create;True;0;0;0;False;0;False;-1;a910e3d4643d63d4f82881cf3c011741;a910e3d4643d63d4f82881cf3c011741;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1503.26,-239.2922;Inherit;True;Property;_Main_Tex;Main_Tex;0;0;Create;True;0;0;0;False;0;False;-1;1ca5eb01c8205784e9893ff212617299;1ca5eb01c8205784e9893ff212617299;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1405.177,-738.4406;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1150.01,-622.1074;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;20;-889.3101,-630.0071;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-828.777,-500.236;Float;False;Property;_Color_offset;Color_offset;9;0;Create;True;0;0;0;False;0;False;10.94;10.94;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;29;-630.1366,-585.9877;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;31;-430.411,-590.3295;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-619.7474,-849.0406;Float;False;Property;_Color_B;Color_B;6;1;[HDR];Create;True;0;0;0;False;0;False;2.401371,0.915374,0.05943987,1;4.867144,1.960028,0.5499874,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;21;-632.7473,-1068.741;Float;False;Property;_Color_A;Color_A;7;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,1;0.5283019,0.08123831,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;-324.6476,-836.0406;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;25;-73.15924,-493.4382;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;135.0278,-308.873;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;134.4128,-608.3209;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;37;1652.374,-456.5041;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;True;2;5;False;;10;False;;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;38;1652.374,-456.5041;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;39;1652.374,-456.5041;Float;False;False;-1;2;ASEMaterialInspector;0;1;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;40;673.8053,-671.5482;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;41;433.3477,-663.6277;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;36;860.5103,-654.1372;Float;False;True;-1;2;ASEMaterialInspector;0;15;Amplify_Shader/Vfx/Fire;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;12;all;0;True;True;2;5;False;;10;False;;0;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;3;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1062.099,-203.1489;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;-589.3082,-262.3676;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-860.6218,-265.3928;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;28;-1649.509,-548.3843;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;10;0;11;0
WireConnection;10;1;12;0
WireConnection;9;0;8;0
WireConnection;9;2;10;0
WireConnection;4;1;9;0
WireConnection;5;0;4;0
WireConnection;6;0;5;0
WireConnection;6;1;7;0
WireConnection;2;0;6;0
WireConnection;2;1;3;0
WireConnection;15;0;14;0
WireConnection;15;2;27;0
WireConnection;13;1;15;0
WireConnection;1;1;2;0
WireConnection;16;0;13;1
WireConnection;16;1;28;1
WireConnection;17;0;16;0
WireConnection;17;1;1;1
WireConnection;20;0;17;0
WireConnection;29;0;20;0
WireConnection;29;1;30;0
WireConnection;31;0;29;0
WireConnection;19;0;21;0
WireConnection;19;1;22;0
WireConnection;19;2;31;0
WireConnection;24;0;25;4
WireConnection;24;1;35;0
WireConnection;23;0;19;0
WireConnection;23;1;25;0
WireConnection;40;0;41;0
WireConnection;40;1;41;1
WireConnection;40;2;41;2
WireConnection;40;3;24;0
WireConnection;41;0;23;0
WireConnection;36;1;40;0
WireConnection;33;0;13;1
WireConnection;33;1;28;2
WireConnection;35;0;42;0
WireConnection;42;0;1;1
WireConnection;42;1;33;0
ASEEND*/
//CHKSM=E088E7C9AAE56B7B97CD736F5236BCBE6D406C2E