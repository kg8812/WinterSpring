// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXShader/Boss_Jururu_Shield"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Sub_UPanner("Sub_UPanner", Float) = 0
		_Sub_VPanner("Sub_VPanner", Float) = -0.6
		_Noise_Texture("Noise_Texture", 2D) = "white" {}
		_Noise_Str("Noise_Str", Float) = 0.55
		_Noise_UPanner("Noise_UPanner", Float) = 0
		_Noise_VPanner("Noise_VPanner", Float) = 0
		[HDR]_Edge_color("Edge_color", Color) = (1,1,1,0)
		_Edge_Thickness("Edge_Thickness", Float) = 0.55
		_Range("Range", Range( -1.5 , 1.5)) = -0.03305449
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0
		_Main_Opacity("Main_Opacity", Float) = 0.21
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Sub_Texture_Opacity("Sub_Texture_Opacity", Float) = 1
		_Noise_Tex02("Noise_Tex02", 2D) = "bump" {}
		_Noise_Str02("Noise_Str02", Float) = 0.2
		_Noise02_UPanner("Noise02_UPanner", Float) = 0
		_Noise02_VPanner("Noise02_VPanner", Float) = 0
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
			

			Blend SrcAlpha One, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define ASE_SRP_VERSION 120110

			#pragma prefer_hlslcc gles
			

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
			sampler2D _TextureSample0;
			sampler2D _Noise_Tex02;
			sampler2D _Noise_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Main_Color;
			float4 _Main_Texture_ST;
			float4 _Edge_color;
			float4 _Noise_Tex02_ST;
			float4 _TextureSample0_ST;
			float _Noise_Str;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Sub_Texture_Opacity;
			float _Noise_Str02;
			float _Noise02_VPanner;
			float _Noise02_UPanner;
			float _Sub_VPanner;
			float _Sub_UPanner;
			float _Main_Opacity;
			float _Range;
			float _Edge_Thickness;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
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

				o.ase_texcoord2.xy = v.ase_texcoord1.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
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

				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float4 tex2DNode4 = tex2D( _Main_Texture, uv_Main_Texture );
				float temp_output_81_0 = ( saturate( ( pow( tex2DNode4.r , 2.41 ) * 0.65 ) ) * _Main_Opacity );
				float2 appendResult26 = (float2(_Sub_UPanner , _Sub_VPanner));
				float2 appendResult95 = (float2(_Noise02_UPanner , _Noise02_VPanner));
				float2 uv_Noise_Tex02 = IN.texCoord0.xy * _Noise_Tex02_ST.xy + _Noise_Tex02_ST.zw;
				float2 panner93 = ( 1.0 * _Time.y * appendResult95 + uv_Noise_Tex02);
				float2 uv_TextureSample0 = IN.texCoord0.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float2 panner24 = ( 1.0 * _Time.y * appendResult26 + ( ( (UnpackNormalScale( tex2D( _Noise_Tex02, panner93 ), 1.0f )).xy * _Noise_Str02 ) + uv_TextureSample0 ));
				float2 temp_cast_0 = (0.5).xx;
				float2 texCoord58 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult52 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 panner53 = ( 1.0 * _Time.y * appendResult52 + uv_Main_Texture);
				#ifdef _USE_CUSTOM_ON
				float staticSwitch62 = IN.ase_texcoord2.xy.x;
				#else
				float staticSwitch62 = _Range;
				#endif
				float2 temp_output_64_0 = ( (( float3( texCoord58 ,  0.0 ) + ( (tex2D( _Noise_Texture, panner53 )).rga * _Noise_Str ) )).yz + staticSwitch62 );
				float2 temp_output_68_0 = step( temp_cast_0 , temp_output_64_0 );
				float2 temp_cast_2 = (_Edge_Thickness).xx;
				float4 break29 = ( ( ( _Main_Color * ( temp_output_81_0 + ( temp_output_81_0 * ( tex2D( _TextureSample0, panner24 ).r * _Sub_Texture_Opacity ) ) ) ) * IN.color ) + saturate( ( _Edge_color * float4( ( temp_output_68_0 - step( temp_cast_2 , temp_output_64_0 ) ), 0.0 , 0.0 ) ) ) );
				float2 temp_cast_4 = (0.5).xx;
				float4 appendResult30 = (float4(break29.r , break29.g , break29.b , ( IN.color.a * ( tex2DNode4.r * temp_output_68_0 ) ).x));
				
				float4 Color = appendResult30;

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
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19202
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-142.8915,73.54462;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;29;117.1085,-82.45538;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;533,-3;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;16;FXShader/Boss_Jururu_Shield;cf964e524c8e69742b1d21fbe2ebcc4a;True;Unlit;0;0;Unlit;3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;True;True;8;5;False;;1;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;True;12;all;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;309.1085,-45.45538;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode;33;-185.8915,245.9193;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;31;-362.8915,-122.4554;Inherit;False;Property;_Main_Color;Main_Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;50;-2643.844,1596.57;Inherit;False;Property;_Noise_VPanner;Noise_VPanner;7;0;Create;True;0;0;0;False;0;False;0;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2640.67,1502.969;Inherit;False;Property;_Noise_UPanner;Noise_UPanner;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;-2431.37,1510.769;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;53;-2219.182,1289.682;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;54;-1969.952,1252.227;Inherit;True;Property;_Noise_Texture;Noise_Texture;4;0;Create;True;0;0;0;False;0;False;-1;f016926f4da6b354eac19272eb7b012f;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-1626.952,1418.227;Inherit;False;Property;_Noise_Str;Noise_Str;5;0;Create;True;0;0;0;False;0;False;0.55;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;56;-1646.952,1273.227;Inherit;False;True;True;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1392.951,1270.227;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-576.9515,1152.227;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-447.9516,1429.227;Inherit;False;Property;_Edge_Thickness;Edge_Thickness;9;0;Create;True;0;0;0;False;0;False;0.55;0.505;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-379.9516,1061.227;Inherit;False;Constant;_Float1;Float 1;8;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;67;-219.9517,1354.227;Inherit;True;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;69;5.048389,1001.227;Inherit;False;Property;_Edge_color;Edge_color;8;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;4,4,4,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;70;47.0484,1206.227;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;72;-2508.526,1226.227;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-1915.952,1093.227;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;62;-795.8022,1500.908;Inherit;False;Property;_Use_Custom;Use_Custom;11;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;61;-1089.839,1570.944;Inherit;False;1;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;59;-1098.951,1457.227;Inherit;False;Property;_Range;Range;10;0;Create;True;0;0;0;False;0;False;-0.03305449;0.34;-1.5;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;43.10852,117.5446;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;196.659,353.6815;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-8.822561,422.5683;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-1170.951,1117.227;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;63;-867.4291,1090.354;Inherit;True;False;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-655.2395,545.9937;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;82;-987.2395,566.9937;Inherit;True;Property;_TextureSample0;Texture Sample 0;13;0;Create;True;0;0;0;False;0;False;-1;a773b59055b36684684ab39588736590;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;26;-1372.58,694.0138;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1606.989,707.9865;Inherit;False;Property;_Sub_UPanner;Sub_UPanner;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1606.479,794.7682;Inherit;False;Property;_Sub_VPanner;Sub_VPanner;3;0;Create;True;0;0;0;False;0;False;-0.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-402.2395,620.9937;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-1625.408,235.0225;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;89;-2190.408,225.0225;Inherit;True;Property;_Noise_Tex02;Noise_Tex02;15;0;Create;True;0;0;0;False;0;False;-1;9d4f08fd5b544864fba35452ea823fb0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;91;-1827.408,452.0225;Inherit;False;Property;_Noise_Str02;Noise_Str02;16;0;Create;True;0;0;0;False;0;False;0.2;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-932.9046,397.6918;Inherit;False;Property;_Main_Opacity;Main_Opacity;12;0;Create;True;0;0;0;False;0;False;0.21;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-698.325,182.9936;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-838.2395,820.9937;Inherit;False;Property;_Sub_Texture_Opacity;Sub_Texture_Opacity;14;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-401.2395,309.1936;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;93;-2395.201,242.7516;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;95;-2599.201,374.7516;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;-2787.201,209.7516;Inherit;False;0;89;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;96;-2785.201,387.7516;Inherit;False;Property;_Noise02_UPanner;Noise02_UPanner;17;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-2793.201,477.7516;Inherit;False;Property;_Noise02_VPanner;Noise02_VPanner;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;90;-1870.408,167.0225;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-1614.807,518.9867;Inherit;False;0;82;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;24;-1195.634,546.8594;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-1391.408,307.0225;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-1381.934,111.6542;Inherit;True;Property;_Main_Texture;Main_Texture;0;0;Create;True;0;0;0;False;0;False;-1;c78d18dd7470c3442b476283d1acd79e;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;99;-1030.958,111.5538;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-1181.958,304.5538;Inherit;False;Constant;_Float0;Float 0;19;0;Create;True;0;0;0;False;0;False;2.41;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-996.9575,298.5538;Inherit;False;Constant;_Float2;Float 2;19;0;Create;True;0;0;0;False;0;False;0.65;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-851.9575,155.5538;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;103;-765.9575,124.5538;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;68;-236.5515,1069.427;Inherit;True;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;106;592.695,282.2657;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;107;569.5806,803.8923;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;283.8014,976.4444;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
WireConnection;32;0;31;0
WireConnection;32;1;87;0
WireConnection;29;0;106;0
WireConnection;1;1;30;0
WireConnection;30;0;29;0
WireConnection;30;1;29;1
WireConnection;30;2;29;2
WireConnection;30;3;35;0
WireConnection;52;0;51;0
WireConnection;52;1;50;0
WireConnection;53;0;72;0
WireConnection;53;2;52;0
WireConnection;54;1;53;0
WireConnection;56;0;54;0
WireConnection;57;0;56;0
WireConnection;57;1;55;0
WireConnection;64;0;63;0
WireConnection;64;1;62;0
WireConnection;67;0;65;0
WireConnection;67;1;64;0
WireConnection;70;0;68;0
WireConnection;70;1;67;0
WireConnection;62;1;59;0
WireConnection;62;0;61;1
WireConnection;34;0;32;0
WireConnection;34;1;33;0
WireConnection;35;0;33;4
WireConnection;35;1;73;0
WireConnection;73;0;4;1
WireConnection;73;1;68;0
WireConnection;60;0;58;0
WireConnection;60;1;57;0
WireConnection;63;0;60;0
WireConnection;84;0;82;1
WireConnection;84;1;85;0
WireConnection;82;1;24;0
WireConnection;26;0;27;0
WireConnection;26;1;28;0
WireConnection;86;0;81;0
WireConnection;86;1;84;0
WireConnection;92;0;90;0
WireConnection;92;1;91;0
WireConnection;89;1;93;0
WireConnection;81;0;103;0
WireConnection;81;1;79;0
WireConnection;87;0;81;0
WireConnection;87;1;86;0
WireConnection;93;0;94;0
WireConnection;93;2;95;0
WireConnection;95;0;96;0
WireConnection;95;1;97;0
WireConnection;90;0;89;0
WireConnection;24;0;88;0
WireConnection;24;2;26;0
WireConnection;88;0;92;0
WireConnection;88;1;25;0
WireConnection;99;0;4;1
WireConnection;99;1;100;0
WireConnection;101;0;99;0
WireConnection;101;1;102;0
WireConnection;103;0;101;0
WireConnection;68;0;66;0
WireConnection;68;1;64;0
WireConnection;106;0;34;0
WireConnection;106;1;107;0
WireConnection;107;0;71;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
ASEEND*/
//CHKSM=E05B98D68F22A3E0864D91B84CEA28665C447408