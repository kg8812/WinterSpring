Shader "WinterSpring/VFX/FXS_Master_Shader"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Main_Intensity("Main_Intensity", Range( 0.001 , 100)) = 1
		_Main_Power("Main_Power", Range( 0.001 , 100)) = 1
		[Toggle(_MAIN_PANNER_ON)] _Main_Panner("Main_Panner", Float) = 0
		_Main_Upanner("Main_Upanner", Float) = 0
		_Main_Vpanner("Main_Vpanner", Float) = 0
		[KeywordEnum(Vertical,Horizontal)] _Sub_Color_UV("Sub_Color_UV", Float) = 0
		[PowerSlider(3.0)]_Sub_Color_Hardness("_Sub_Color_Hardness", Range(0.01,1)) = 1
		[HDR]_Sub_Color("Sub_Color", Color) = (1,1,1,1)
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,1)
		_Mask_Tex("Mask_Tex", 2D) = "white" {}
		_Mask_Upanner("Mask_Upanner", Float) = 0
		_Mask_Vpanner("Mask_Vpanner", Float) = 0
		[Toggle(_MASK_CUSTOM_ON)] _Mask_Custom("Mask_Custom", Float) = 0
		_Normal_Tex("Normal_Tex", 2D) = "white" {}
		[Toggle(_DISTORTION_CUSTOM_ON)] _Distortion_Custom("Distortion_Custom", Float) = 0
		_Normal_Strength("Normal_Strength", Range( -1 , 1)) = 0
		_Dissolve_SubTex("Dissolve_SubTex", 2D) = "white" {}
		_Dissolve_Tex("Dissolve_Tex", 2D) = "white" {}
		_Dissolve_Upanner("Dissolve_Upanner", Float) = 0
		_Dissolve_Vpanner("Dissolve_Vpanner", Float) = 0
		[Toggle(_USE_STEP_ON)] _Use_Step("Use_Step", Float) = 0
		_Step_Value("Step_Value", Float) = 0.1
		_Vertex_Normal_Strength("Vertex_Normal_Strength", Range( -1 , 1)) = 0
		_Sub_Color_Offset("Sub_Color_Offset", Range(0, 1)) = 0.5
		[Toggle(_USE_SUB_COLOR_ON)] _Use_Sub_Color("Use_Sub_Color", Float) = 0
		[Toggle(_USE_NORMAL_ON)] _Use_Normal("Use_Normal", Float) = 0
		[Toggle(_USE_MASK_ON)] _Use_Mask("Use_Mask", Float) = 0
		[Toggle(_USE_DISSOLVE_ON)] _Use_Dissolve("Use_Dissolve", Float) = 0
		_Normal_Panner_And_Offset("Normal_Panner_And_Offset", Vector) = (0,0,0,0)
		[Toggle(_NORMAL_POLAR_ON)] _Normal_Polar("Normal_Polar", Float) = 0
		[Toggle(_MAIN_POLAR_ON)] _Main_Polar("Main_Polar", Float) = 0
		[Toggle(_DISSOLVE_POLAR_ON)] _Dissolve_Polar("Dissolve_Polar", Float) = 0
		[Toggle(_SUB_DISSOLVE_ON)] _Sub_Dissolve("Sub_Dissolve", Float) = 0
		_Step_Edge_Thickness("Step_Edge_Thickness", Range( 0 , 1)) = 0.1434928
		[HDR]_Edge_Color("Edge_Color", Color) = (1,1,1,0)
		[Enum(UnityEngine.Rendering.CullMode)]_CustomCullMode("CustomCullMode", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendRGBSrc("BlendRGBSrc", Int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)]_BlendRGBDst("BlendRGBDst", Int) = 10

		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull [_CustomCullMode]
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

			Blend [_BlendRGBSrc] [_BlendRGBDst], Zero Zero
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM

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

			#pragma shader_feature_local _NORMAL_POLAR_ON
			#pragma shader_feature_local _DISTORTION_CUSTOM_ON
			#pragma shader_feature_local _USE_DISSOLVE_ON
			#pragma shader_feature_local _USE_SUB_COLOR_ON
			#pragma shader_feature_local _MAIN_PANNER_ON
			#pragma shader_feature_local _MAIN_POLAR_ON
			#pragma shader_feature_local _USE_NORMAL_ON
			#pragma shader_feature_local _SUB_COLOR_UV_VERTICAL _SUB_COLOR_UV_HORIZONTAL
			#pragma shader_feature _USE_STEP_ON
			#pragma shader_feature_local _DISSOLVE_POLAR_ON
			#pragma shader_feature_local _SUB_DISSOLVE_ON
			#pragma shader_feature_local _USE_MASK_ON
			#pragma shader_feature_local _MASK_CUSTOM_ON


			sampler2D _Normal_Tex;
			sampler2D _Main_Tex;
			sampler2D _Dissolve_Tex;
			sampler2D _Dissolve_SubTex;
			sampler2D _Mask_Tex;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color;
			float4 _Edge_Color;
			float4 _Dissolve_SubTex_ST;
			float4 _Dissolve_Tex_ST;
			float4 _Sub_Color;
			float4 _Main_Tex_ST;
			float4 _Mask_Tex_ST;
			float4 _Normal_Panner_And_Offset;
			float4 _Normal_Tex_ST;
			float _Normal_Strength;
			float _Mask_Upanner;
			int _BlendRGBDst;
			float _Step_Edge_Thickness;
			int _BlendRGBSrc;
			float _Dissolve_Vpanner;
			float _Dissolve_Upanner;
			float _Step_Value;
			float _Sub_Color_Offset;
			float _Sub_Color_Hardness;
			float _Mask_Vpanner;
			float _Main_Intensity;
			float _Main_Power;
			float _Main_Vpanner;
			float _Main_Upanner;
			float _Vertex_Normal_Strength;
			int _CustomCullMode;
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
				#ifdef _USE_NORMAL_ON
				// Custom Distortion
				float2 appendResult425 = float2(_Normal_Panner_And_Offset.x, _Normal_Panner_And_Offset.y);
				float2 appendResult427 = float2(_Normal_Panner_And_Offset.z, _Normal_Panner_And_Offset.w);


				float2 temp_output_31_0_g10 = v.uv0.xy * float2(2, 2) - float2( 1,1 );

				float2 appendResult39_g10 = (float2(frac( ( atan2( (temp_output_31_0_g10).x , (temp_output_31_0_g10).y ) / TWO_PI ) ) , length( temp_output_31_0_g10 )));
				float2 panner54_g10 = appendResult425 * _TimeParameters.x + appendResult39_g10;

				#ifdef _DISTORTION_CUSTOM_ON
				float2 DistortionUV = appendResult427 * panner54_g10;
				#else
				float2 DistortionUV = v.uv0.xy;
				#endif


				// Polar Coordinates

				float2 CenteredUV15_g12 = DistortionUV - float2( 0.5,0.5 );
				float2 appendResult23_g12 = float2( length( CenteredUV15_g12 ) * 2.0  ,  atan2(CenteredUV15_g12.x , CenteredUV15_g12.y ) * ( 1.0 / TWO_PI ) );
				#ifdef _NORMAL_POLAR_ON
				float2 staticSwitch437 = appendResult23_g12;
				#else
				float2 staticSwitch437 = DistortionUV;
				#endif


				float2 panner275 = (  staticSwitch437  * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw + _Time.y * appendResult425);
				float3 tex2DNode286 = UnpackNormalScale( tex2Dlod( _Normal_Tex, float4( panner275, 0, 0.0) ), 1.0f );
				

				float3 vertexValue = tex2DNode286 * v.normal * _Vertex_Normal_Strength;
				#else
				float3 vertexValue = 0;
				#endif

				v.positionOS.xyz += vertexValue;

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

				float2 NormalDistortionPanner = _Normal_Panner_And_Offset.xy;

				#ifdef _USE_NORMAL_ON


				//Normal Distorion

				#ifdef _DISTORTION_CUSTOM_ON


				float2 NormalDistortionoffset = _Normal_Panner_And_Offset.zw;

				float2 NormalDistortionCenter = IN.texCoord0.xy * 2 - float2( 1,1 );
				float2 NormalDistortionResult = float2(frac( atan2(NormalDistortionCenter.x , NormalDistortionCenter.y) / TWO_PI), length(NormalDistortionCenter));
				float2 NormalDistortionResultAddPanner = NormalDistortionPanner * _TimeParameters.x + NormalDistortionResult;

				float2 NormalDistorionUV = NormalDistortionoffset * NormalDistortionResultAddPanner;
				#else
				float2 NormalDistorionUV = IN.texCoord0.xy;
				#endif


				//Normal Polar Coordinate

				#ifdef _NORMAL_POLAR_ON

				float2 NormalCenteredUV = NormalDistorionUV - float2( 0.5,0.5 );
				float2 NormalPolarResult = float2( length(NormalCenteredUV)* 2 ,  atan2(NormalCenteredUV.x , NormalCenteredUV.y ) * ( 1 / TWO_PI ) );

				float2 NormalPolarUV = NormalPolarResult;
				#else
				float2 NormalPolarUV = NormalDistorionUV;
				#endif


				// Normal Result

				float2 Normalpanner = _Time.y * NormalDistortionPanner;

				float2 uv_Normal_Tex = NormalPolarUV * _Normal_Tex_ST.xy + _Normal_Tex_ST.zw + Normalpanner;

				float3 NormalFinalUV = UnpackNormalScale( tex2D( _Normal_Tex, uv_Normal_Tex), 1.0f );


				float2 NormalUV = NormalFinalUV.xy;
				#else
				float2 NormalUV = 0;
				#endif


				// Main Polar Coordinate


				float2 CenteredUV = IN.texCoord0.xy - float2( 0.5,0.5 );

				float2 PolarResultUV = float2( length(CenteredUV) * 2.0 ,  atan2(CenteredUV.x , CenteredUV.y ) * ( 1.0 / TWO_PI )  );


				#ifdef _MAIN_POLAR_ON

				float2 MainPolarUV = PolarResultUV;
				#else
				float2 MainPolarUV = IN.texCoord0.xy;
				#endif


				// Main Panner
				#ifdef _MAIN_PANNER_ON

				float2 MainPanner = float2(_Main_Upanner , _Main_Vpanner);
				float2 MainPannerResult =  _Time.y * MainPanner + MainPolarUV;

				float2 MainFinalUV = MainPannerResult;
				#else
				float2 MainFinalUV = MainPolarUV;
				#endif


				// Main Result

				float2 uv_Main_Tex = MainFinalUV * _Main_Tex_ST.xy + _Main_Tex_ST.zw;

				float2 MainUV = uv_Main_Tex + NormalUV * _Normal_Strength;

				float4 MainTex = tex2D( _Main_Tex, MainUV);

				// Sub Color

				#ifdef _USE_SUB_COLOR_ON

				#if defined(_SUB_COLOR_UV_VERTICAL)
				float SubColorUVDirection = IN.texCoord0.x;
				#elif defined(_SUB_COLOR_UV_HORIZONTAL)
				float SubColorUVDirection = IN.texCoord0.y;
				#else
				float SubColorUVDirection = IN.texCoord0.x;
				#endif

				float SubColorlerp = saturate((SubColorUVDirection - _Sub_Color_Offset) / _Sub_Color_Hardness + 0.5);


				float4 SubColorSet = lerp(_Main_Color, _Sub_Color, SubColorlerp);
				#else
				float4 SubColorSet = _Main_Color;
				#endif

				// Subcolor Result

				float4 MainColorResult = SubColorSet * IN.color * pow(MainTex.r, _Main_Power) * _Main_Intensity;



				// Dissolve Tex

				#ifdef _USE_DISSOLVE_ON

				float2 DissolvePanner = (float2(_Dissolve_Upanner , _Dissolve_Vpanner));

				#ifdef _DISSOLVE_POLAR_ON
				float2 DissolvePolarUV = PolarResultUV;
				#else
				float2 DissolvePolarUV = IN.texCoord0.xy;
				#endif

				float2 DissolveResultUV = DissolvePolarUV * _Dissolve_Tex_ST.xy + _Time.y * DissolvePanner + _Dissolve_Tex_ST.zw;


				//Sub Dissolve Tex

				float2 uv_Dissolve_SubTex = IN.texCoord0.xy * _Dissolve_SubTex_ST.xy + _Dissolve_SubTex_ST.zw;

				#ifdef _SUB_DISSOLVE_ON
				float SubTexColor = tex2D( _Dissolve_SubTex, uv_Dissolve_SubTex).r;
				#else
				float SubTexColor = 1.0;
				#endif

				//Dissolve Step and Result

				float AllDissolveTexColor = saturate( IN.texCoord0.z +  tex2D( _Dissolve_Tex, DissolveResultUV).r * SubTexColor );
				float LowStepDissolve = step( _Step_Value , AllDissolveTexColor);
				float HighStepDissolve = step( ( _Step_Value + ( ( 1.0 - _Step_Value ) * _Step_Edge_Thickness ) ) , AllDissolveTexColor);
				#ifdef _USE_STEP_ON
				float4 DissolveStep = ((LowStepDissolve - HighStepDissolve) * _Edge_Color) * MainTex +(HighStepDissolve * MainColorResult);
				float StepAlpha = LowStepDissolve;
				#else
				float4 DissolveStep = MainColorResult;
				float StepAlpha = AllDissolveTexColor;
				#endif


				float4 MainDissolve = DissolveStep;
				float DissolveAlpha = MainTex.r * StepAlpha;
				#else
				float4 MainDissolve = MainColorResult;
				float DissolveAlpha = MainTex.r;
				#endif


				//Mask Result

				float2 MaskPanner = (float2(_Mask_Upanner , _Mask_Vpanner));
				float2 uv_Mask_Tex = IN.texCoord0.xy * _Mask_Tex_ST.xy + _Mask_Tex_ST.zw;
				float2 MaskPannerUV = _Time.y * MaskPanner + uv_Mask_Tex;
				#ifdef _MASK_CUSTOM_ON
				float2 MaskCustomUV = (MaskPannerUV + IN.texCoord0.w );
				#else
				float2 MaskCustomUV = MaskPannerUV;
				#endif

				#ifdef _USE_MASK_ON
				float MaskAlpha = tex2D( _Mask_Tex, MaskCustomUV).r;
				#else
				float MaskAlpha = 1.0;
				#endif

				
				float4 Color = float4(MainDissolve.r, MainDissolve.g, MainDissolve.b, (IN.color.a * saturate((MaskAlpha * DissolveAlpha))));

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
				
	}
	CustomEditor "WinterSpringMasterShaderGUI"
	Fallback "Hidden/InternalErrorShader"
	
}