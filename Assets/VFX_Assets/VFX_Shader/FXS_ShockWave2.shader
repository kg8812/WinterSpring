// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXShader/Shokewave02"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Main_Ins("Main_Ins", Range( 1 , 10)) = 1
		_Main_Pow("Main_Pow", Range( 1 , 10)) = 1
		_Opacity("Opacity", Float) = 1
		_Main_Upanner("Main_Upanner", Float) = -0.25
		_Main_VPanner("Main_VPanner", Float) = 0
		_Main_UTilling("Main_UTilling", Float) = 3
		_Main_VTilling("Main_VTilling", Float) = 0.2
		_Main_UOffset("Main_UOffset", Float) = 0
		_Main_VOffset("Main_VOffset", Float) = 0
		_Nosie_Texture("Nosie_Texture", 2D) = "bump" {}
		_Noise_Str("Noise_Str", Range( 0 , 1)) = 0.2
		_Noise_UOffset("Noise_UOffset", Float) = 0
		_Noise_VOffset("Noise_VOffset", Float) = 0
		_Noise_UTilling("Noise_UTilling", Float) = 3
		_Noise_VTilling("Noise_VTilling", Float) = 0.2
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Dissolve_Texture("Dissolve_Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 0.3882353
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0
		_Diss_UTilling("Diss_UTilling", Float) = 3
		_Diss_VTilling("Diss_VTilling", Float) = 0.2
		_Diss_UPanner("Diss_UPanner", Float) = -0.25
		_Diss_VPanner("Diss_VPanner", Float) = 0
		_Diss_UOffset("Diss_UOffset", Float) = 0
		_Diss_VOffset("Diss_VOffset", Float) = 0
		_Mask_Texture("Mask_Texture", 2D) = "white" {}
		_Mask_Str("Mask_Str", Float) = 16.86
		[ASEEnd]_Mask_Range("Mask_Range", Float) = 3.18
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		HLSLINCLUDE
		#pragma target 2.0
		ENDHLSL

		
		Pass
		{
			Name "Unlit"
			

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define ASE_SRP_VERSION 999999

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITEUNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _USE_CUSTOM_ON


			sampler2D _Main_Texture;
			sampler2D _Nosie_Texture;
			sampler2D _Mask_Texture;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color;
			float4 _Mask_Texture_ST;
			float _Diss_VOffset;
			float _Diss_UOffset;
			float _Diss_UTilling;
			float _Diss_VTilling;
			float _Diss_VPanner;
			float _Diss_UPanner;
			float _Mask_Str;
			float _Mask_Range;
			float _Main_Ins;
			float _Main_Pow;
			float _Dissolve;
			float _Main_VOffset;
			float _Main_UTilling;
			float _Main_VTilling;
			float _Noise_Str;
			float _Noise_VOffset;
			float _Noise_UOffset;
			float _Noise_UTilling;
			float _Noise_VTilling;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Main_VPanner;
			float _Main_Upanner;
			float _Main_UOffset;
			float _Opacity;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
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

				o.ase_texcoord2 = v.ase_texcoord2;
				o.ase_texcoord3 = v.ase_texcoord1;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.vertex.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 appendResult24 = (float2(_Main_Upanner , _Main_VPanner));
				float2 appendResult78 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 CenteredUV15_g2 = ( IN.texCoord0.xy - float2( 0.5,0.5 ) );
				float2 break17_g2 = CenteredUV15_g2;
				float2 appendResult23_g2 = (float2(( length( CenteredUV15_g2 ) * _Noise_VTilling * 2.0 ) , ( atan2( break17_g2.x , break17_g2.y ) * ( 1.0 / TWO_PI ) * _Noise_UTilling )));
				float2 panner13 = ( 1.0 * _Time.y * appendResult78 + appendResult23_g2);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch68 = IN.ase_texcoord2.x;
				#else
				float staticSwitch68 = _Noise_UOffset;
				#endif
				#ifdef _USE_CUSTOM_ON
				float staticSwitch67 = IN.ase_texcoord2.y;
				#else
				float staticSwitch67 = _Noise_VOffset;
				#endif
				float2 appendResult75 = (float2(staticSwitch68 , staticSwitch67));
				float2 CenteredUV15_g1 = ( IN.texCoord0.xy - float2( 0.5,0.5 ) );
				float2 break17_g1 = CenteredUV15_g1;
				float2 appendResult23_g1 = (float2(( length( CenteredUV15_g1 ) * _Main_VTilling * 2.0 ) , ( atan2( break17_g1.x , break17_g1.y ) * ( 1.0 / TWO_PI ) * _Main_UTilling )));
				float2 panner4 = ( 1.0 * _Time.y * appendResult24 + ( ( (UnpackNormalScale( tex2D( _Nosie_Texture, (panner13*1.0 + appendResult75) ), 1.0f )).xy * _Noise_Str ) + appendResult23_g1 ));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch40 = IN.ase_texcoord3.z;
				#else
				float staticSwitch40 = _Main_UOffset;
				#endif
				#ifdef _USE_CUSTOM_ON
				float staticSwitch42 = IN.ase_texcoord3.w;
				#else
				float staticSwitch42 = _Main_VOffset;
				#endif
				float2 appendResult71 = (float2(staticSwitch40 , staticSwitch42));
				float4 tex2DNode2 = tex2D( _Main_Texture, (panner4*1.0 + appendResult71) );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch38 = IN.ase_texcoord3.x;
				#else
				float staticSwitch38 = _Main_Ins;
				#endif
				float4 break9 = ( ( _Main_Color * ( pow( tex2DNode2.r , _Main_Pow ) * staticSwitch38 ) ) * IN.color );
				float2 uv_Mask_Texture = IN.texCoord0.xy * _Mask_Texture_ST.xy + _Mask_Texture_ST.zw;
				float2 appendResult31 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 CenteredUV15_g3 = ( IN.texCoord0.xy - float2( 0.5,0.5 ) );
				float2 break17_g3 = CenteredUV15_g3;
				float2 appendResult23_g3 = (float2(( length( CenteredUV15_g3 ) * _Diss_VTilling * 2.0 ) , ( atan2( break17_g3.x , break17_g3.y ) * ( 1.0 / TWO_PI ) * _Diss_UTilling )));
				float2 panner27 = ( 1.0 * _Time.y * appendResult31 + appendResult23_g3);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch66 = IN.ase_texcoord2.z;
				#else
				float staticSwitch66 = _Diss_UOffset;
				#endif
				#ifdef _USE_CUSTOM_ON
				float staticSwitch70 = IN.ase_texcoord2.w;
				#else
				float staticSwitch70 = _Diss_VOffset;
				#endif
				float2 appendResult34 = (float2(staticSwitch66 , staticSwitch70));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch39 = IN.ase_texcoord3.y;
				#else
				float staticSwitch39 = _Dissolve;
				#endif
				float4 appendResult10 = (float4(break9.r , break9.g , break9.b , ( IN.color.a * saturate( ( ( ( tex2DNode2.r * saturate( ( pow( tex2D( _Mask_Texture, uv_Mask_Texture ).r , _Mask_Range ) * _Mask_Str ) ) ) * saturate( ( tex2D( _Dissolve_Texture, (panner27*1.0 + appendResult34) ).r + staticSwitch39 ) ) ) * _Opacity ) ) )));
				
				float4 Color = appendResult10;

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				Color *= IN.color;

				return Color;
			}

			ENDHLSL
		}
	}
	CustomEditor "ASEMaterialInspector"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18712
-1850;136;1775;828;4744.274;1298.671;3.397372;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;69;-3198.539,586.3903;Inherit;False;2;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;77;-2967.563,-36.07513;Inherit;False;Property;_Noise_VOffset;Noise_VOffset;14;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-2949.563,-135.0751;Inherit;False;Property;_Noise_UOffset;Noise_UOffset;13;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-3146.563,-187.0751;Inherit;False;Property;_Noise_UPanner;Noise_UPanner;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-3142.563,-107.0751;Inherit;False;Property;_Noise_VPanner;Noise_VPanner;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-3326.456,-408.4141;Inherit;False;Property;_Noise_VTilling;Noise_VTilling;16;0;Create;True;0;0;0;False;0;False;0.2;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3325.017,-315.6593;Inherit;False;Property;_Noise_UTilling;Noise_UTilling;15;0;Create;True;0;0;0;False;0;False;3;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;67;-2769.518,-52.04472;Inherit;False;Property;_Use_Custom;Use_Custom;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;68;-2767.629,-168.6599;Inherit;False;Property;_Use_Custom;Use_Custom;21;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;12;-3064.798,-478.5016;Inherit;True;Polar Coordinates;-1;;2;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;78;-2940.563,-237.0751;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;75;-2522.563,-169.0751;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-2707.798,-458.5016;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;74;-2499.153,-386.6529;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;11;-2285.798,-419.3016;Inherit;True;Property;_Nosie_Texture;Nosie_Texture;11;0;Create;True;0;0;0;False;0;False;-1;9d4f08fd5b544864fba35452ea823fb0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-1935.91,-209.9743;Inherit;False;Property;_Noise_Str;Noise_Str;12;0;Create;True;0;0;0;False;0;False;0.2;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1899.5,-83;Inherit;False;Property;_Main_VTilling;Main_VTilling;8;0;Create;True;0;0;0;False;0;False;0.2;0.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1876.792,551.5327;Inherit;False;Property;_Diss_UPanner;Diss_UPanner;23;0;Create;True;0;0;0;False;0;False;-0.25;-0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;15;-1985.798,-418.3016;Inherit;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1872.891,636.0327;Inherit;False;Property;_Diss_VPanner;Diss_VPanner;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1907.5,26;Inherit;False;Property;_Main_UTilling;Main_UTilling;7;0;Create;True;0;0;0;False;0;False;3;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1981.696,854.2417;Inherit;False;Property;_Diss_VOffset;Diss_VOffset;26;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2173.276,452.0618;Inherit;False;Property;_Diss_VTilling;Diss_VTilling;22;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1983.486,760.6055;Inherit;False;Property;_Diss_UOffset;Diss_UOffset;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2164.177,540.4618;Inherit;False;Property;_Diss_UTilling;Diss_UTilling;21;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1741.006,106.5011;Inherit;False;Property;_Main_Upanner;Main_Upanner;5;0;Create;True;0;0;0;False;0;False;-0.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-1526.738,214.0728;Inherit;False;Property;_Main_UOffset;Main_UOffset;9;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1502.693,307.2746;Inherit;False;Property;_Main_VOffset;Main_VOffset;10;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;31;-1683.091,541.1326;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;6;-1698.5,-143;Inherit;True;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1675.91,-407.9743;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;66;-1783.2,731.3057;Inherit;False;Property;_Use_Custom;Use_Custom;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;28;-1927.577,380.5618;Inherit;False;Polar Coordinates;-1;;3;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1742.875,188.964;Inherit;False;Property;_Main_VPanner;Main_VPanner;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;70;-1789.663,857.2046;Inherit;False;Property;_Use_Custom;Use_Custom;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;41;-3198.282,386.0317;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;34;-1525.79,681.5326;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;27;-1583.077,400.0618;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-1495.062,98.16577;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-1438.798,-336.3016;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;46;-1247.964,798.519;Inherit;True;Property;_Mask_Texture;Mask_Texture;27;0;Create;True;0;0;0;False;0;False;-1;e387f662afce60c4cbf4689043469d34;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;42;-1288.979,248.1924;Inherit;False;Property;_Use_Custom;Use_Custom;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;40;-1288.163,134.5258;Inherit;False;Property;_Use_Custom;Use_Custom;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1137.923,1022.508;Inherit;False;Property;_Mask_Range;Mask_Range;29;0;Create;True;0;0;0;False;0;False;3.18;1.72;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-899.0308,1028.309;Inherit;False;Property;_Mask_Str;Mask_Str;28;0;Create;True;0;0;0;False;0;False;16.86;16.86;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;26;-1381.577,376.6617;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;61;-914.3223,789.808;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;4;-1341.5,-114;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1361.581,587.0792;Inherit;False;Property;_Dissolve;Dissolve;20;0;Create;True;0;0;0;False;0;False;0.3882353;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;-1048.669,134.9219;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;25;-1190.277,357.5618;Inherit;True;Property;_Dissolve_Texture;Dissolve_Texture;19;0;Create;True;0;0;0;False;0;False;-1;93afafdc5fb3623489ffd517d3f11574;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleAndOffsetNode;21;-995.062,-100.8342;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-642.4308,779.5096;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;39;-1070.168,559.696;Inherit;False;Property;_Use_Custom;Use_Custom;13;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-641.3162,168.639;Inherit;False;Property;_Main_Ins;Main_Ins;2;0;Create;True;0;0;0;False;0;False;1;0;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;63;-404.1224,772.2078;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-797.5,-95;Inherit;True;Property;_Main_Texture;Main_Texture;0;0;Create;True;0;0;0;False;0;False;-1;f016926f4da6b354eac19272eb7b012f;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-774.3162,93.63904;Inherit;False;Property;_Main_Pow;Main_Pow;3;0;Create;True;0;0;0;False;0;False;1;0;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-861.7368,347.4033;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;44;-644.7366,358.4033;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-496.5043,258.3069;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;56;-442.3162,-82.36096;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;38;-359.6429,19.61998;Inherit;False;Property;_Use_Custom;Use_Custom;30;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-314.4031,260.0071;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-139.3162,-128.361;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-380.7028,443.3071;Inherit;False;Property;_Opacity;Opacity;4;0;Create;True;0;0;0;False;0;False;1;-1.49;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;58;-145.3162,-317.361;Inherit;False;Property;_Main_Color;Main_Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-107.0028,352.5071;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;53;57.53979,163.2908;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;138.6838,-188.361;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;51;175.5398,349.2908;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;292.6838,-132.361;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;325.5398,280.2908;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;9;470.3343,-57.50537;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;10;616.3315,-38.93324;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;780.7001,-23.5;Float;False;True;-1;2;ASEMaterialInspector;0;18;FXShader/Shokewave02;cf964e524c8e69742b1d21fbe2ebcc4a;True;Unlit;0;0;Unlit;3;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;0;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;1;True;False;;False;0
WireConnection;67;1;77;0
WireConnection;67;0;69;2
WireConnection;68;1;76;0
WireConnection;68;0;69;1
WireConnection;12;3;17;0
WireConnection;12;4;18;0
WireConnection;78;0;79;0
WireConnection;78;1;80;0
WireConnection;75;0;68;0
WireConnection;75;1;67;0
WireConnection;13;0;12;0
WireConnection;13;2;78;0
WireConnection;74;0;13;0
WireConnection;74;2;75;0
WireConnection;11;1;74;0
WireConnection;15;0;11;0
WireConnection;31;0;32;0
WireConnection;31;1;33;0
WireConnection;6;3;7;0
WireConnection;6;4;8;0
WireConnection;19;0;15;0
WireConnection;19;1;20;0
WireConnection;66;1;35;0
WireConnection;66;0;69;3
WireConnection;28;3;29;0
WireConnection;28;4;30;0
WireConnection;70;1;36;0
WireConnection;70;0;69;4
WireConnection;34;0;66;0
WireConnection;34;1;70;0
WireConnection;27;0;28;0
WireConnection;27;2;31;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;16;0;19;0
WireConnection;16;1;6;0
WireConnection;42;1;73;0
WireConnection;42;0;41;4
WireConnection;40;1;72;0
WireConnection;40;0;41;3
WireConnection;26;0;27;0
WireConnection;26;2;34;0
WireConnection;61;0;46;1
WireConnection;61;1;62;0
WireConnection;4;0;16;0
WireConnection;4;2;24;0
WireConnection;71;0;40;0
WireConnection;71;1;42;0
WireConnection;25;1;26;0
WireConnection;21;0;4;0
WireConnection;21;2;71;0
WireConnection;64;0;61;0
WireConnection;64;1;65;0
WireConnection;39;1;45;0
WireConnection;39;0;41;2
WireConnection;63;0;64;0
WireConnection;2;1;21;0
WireConnection;43;0;25;1
WireConnection;43;1;39;0
WireConnection;44;0;43;0
WireConnection;47;0;2;1
WireConnection;47;1;63;0
WireConnection;56;0;2;1
WireConnection;56;1;55;0
WireConnection;38;1;57;0
WireConnection;38;0;41;1
WireConnection;48;0;47;0
WireConnection;48;1;44;0
WireConnection;54;0;56;0
WireConnection;54;1;38;0
WireConnection;49;0;48;0
WireConnection;49;1;50;0
WireConnection;59;0;58;0
WireConnection;59;1;54;0
WireConnection;51;0;49;0
WireConnection;60;0;59;0
WireConnection;60;1;53;0
WireConnection;52;0;53;4
WireConnection;52;1;51;0
WireConnection;9;0;60;0
WireConnection;10;0;9;0
WireConnection;10;1;9;1
WireConnection;10;2;9;2
WireConnection;10;3;52;0
WireConnection;1;1;10;0
ASEEND*/
//CHKSM=1E33F814B09F065A646F3EB852E193FD2E302F9C