// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXShader/Fire_Tornado"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_Front_Color("Front_Color", Color) = (1,0,0,0)
		[HDR]_Back_Color("Back_Color", Color) = (0,0,1,0)
		_Main_UPanner("Main_UPanner", Float) = 0.5
		_Main_VPanner("Main_VPanner", Float) = 0
		_Mask_Texture("Mask_Texture", 2D) = "white" {}
		_Mask_Str("Mask_Str", Float) = 4.5
		_Mask_Range("Mask_Range", Float) = 0.18
		_Dissolve_Texture("Dissolve_Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Float) = -0.35
		_Diss_UPanner("Diss_UPanner", Float) = 1.3
		_Diss_VPanner("Diss_VPanner", Float) = 0
		_Noise_Texture("Noise_Texture", 2D) = "bump" {}
		_Noise_Str("Noise_Str", Float) = 0
		_Noise_UPanner("Noise_UPanner", Float) = 1
		_Noise_VPanner("Noise_VPanner", Float) = 0
		_Float1("Float 1", Float) = 0.32
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0
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
			sampler2D _Noise_Texture;
			sampler2D _Mask_Texture;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Front_Color;
			float4 _Back_Color;
			float4 _Dissolve_Texture_ST;
			float4 _Mask_Texture_ST;
			float4 _Noise_Texture_ST;
			float4 _Main_Texture_ST;
			float _Diss_VPanner;
			float _Diss_UPanner;
			float _Mask_Str;
			float _Noise_Str;
			float _Noise_VPanner;
			float _Noise_UPanner;
			float _Main_VPanner;
			float _Main_UPanner;
			float _Mask_Range;
			float _Float1;
			float _Dissolve;
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

				o.ase_texcoord2 = v.ase_texcoord1;
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

			half4 frag( VertexOutput IN , bool ase_vface : SV_IsFrontFace ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float4 switchResult37 = (((ase_vface>0)?(_Front_Color):(_Back_Color)));
				float4 break2 = ( switchResult37 * IN.color );
				float2 appendResult24 = (float2(_Main_UPanner , _Main_VPanner));
				float2 appendResult33 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 uv_Noise_Texture = IN.texCoord0.xy * _Noise_Texture_ST.xy + _Noise_Texture_ST.zw;
				float2 panner32 = ( 1.0 * _Time.y * appendResult33 + uv_Noise_Texture);
				float2 temp_output_30_0 = ( (UnpackNormalScale( tex2D( _Noise_Texture, panner32 ), 1.0f )).xy * _Noise_Str );
				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float2 panner22 = ( 1.0 * _Time.y * appendResult24 + ( temp_output_30_0 + uv_Main_Texture ));
				float2 uv_Mask_Texture = IN.texCoord0.xy * _Mask_Texture_ST.xy + _Mask_Texture_ST.zw;
				float2 appendResult42 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 uv_Dissolve_Texture = IN.texCoord0.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float2 panner41 = ( 1.0 * _Time.y * appendResult42 + ( temp_output_30_0 + uv_Dissolve_Texture ));
				#ifdef _USE_CUSTOM_ON
				float staticSwitch64 = IN.ase_texcoord2.x;
				#else
				float staticSwitch64 = _Dissolve;
				#endif
				float4 appendResult4 = (float4(break2.r , break2.g , break2.b , ( IN.color.a * ( step( _Mask_Range , ( tex2D( _Main_Texture, panner22 ).r * ( pow( tex2D( _Mask_Texture, uv_Mask_Texture ).r , 2.0 ) * _Mask_Str ) ) ) * saturate( ( step( tex2D( _Dissolve_Texture, panner41 ).r , _Float1 ) + staticSwitch64 ) ) ) )));
				
				float4 Color = appendResult4;

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
Node;AmplifyShaderEditor.SwitchByFaceNode;37;-841.1747,-177.5449;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;39;-1104.334,-120.4199;Inherit;False;Property;_Back_Color;Back_Color;2;1;[HDR];Create;True;0;0;0;False;0;False;0,0,1,0;1,0.8816209,0.1921569,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;38;-1118.533,-320.2203;Inherit;False;Property;_Front_Color;Front_Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;17;-897.4492,259.0094;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1174.124,251.5317;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-1477.518,23.15158;Inherit;True;Property;_Main_Texture;Main_Texture;0;0;Create;True;0;0;0;False;0;False;-1;f016926f4da6b354eac19272eb7b012f;f016926f4da6b354eac19272eb7b012f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1146.997,544.6095;Inherit;False;Property;_Mask_Range;Mask_Range;7;0;Create;True;0;0;0;False;0;False;0.18;0.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;29;-2315.887,-299.3641;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-2022.087,-281.1643;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;22;-1685.709,48.54489;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-1822.945,193.4973;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-2038.945,199.4973;Inherit;False;Property;_Main_UPanner;Main_UPanner;3;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2039.945,282.4973;Inherit;False;Property;_Main_VPanner;Main_VPanner;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-2187.445,39.99734;Inherit;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;32;-2935.661,-201.1206;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-3072.897,-56.16808;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-3288.897,-50.16807;Inherit;False;Property;_Noise_UPanner;Noise_UPanner;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-3289.897,32.83192;Inherit;False;Property;_Noise_VPanner;Noise_VPanner;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-3305.397,-222.6681;Inherit;False;0;28;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2625.287,-304.5642;Inherit;True;Property;_Noise_Texture;Noise_Texture;12;0;Create;True;0;0;0;False;0;False;-1;9d4f08fd5b544864fba35452ea823fb0;9d4f08fd5b544864fba35452ea823fb0;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-2224.887,-139.4644;Inherit;False;Property;_Noise_Str;Noise_Str;13;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;41;-1474.752,854.8264;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;42;-1611.988,999.7789;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-1827.988,1005.779;Inherit;False;Property;_Diss_UPanner;Diss_UPanner;10;0;Create;True;0;0;0;False;0;False;1.3;1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1828.988,1088.779;Inherit;False;Property;_Diss_VPanner;Diss_VPanner;11;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;49;-975.3634,832.1114;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-1696.208,807.4307;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-2043.283,862.0005;Inherit;False;0;40;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1860.339,-94.78519;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-621.3255,822.457;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-391.2253,722.2277;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-231.2253,447.2277;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1148.69,1061.455;Inherit;False;Property;_Float1;Float 1;16;0;Create;True;0;0;0;False;0;False;0.32;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-7.11792,216.8558;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;59;-598.4204,207.7188;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-467.8905,-110.7755;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;446.4139,-20.88487;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;18;FXShader/Fire_Tornado;cf964e524c8e69742b1d21fbe2ebcc4a;True;Unlit;0;0;Unlit;3;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;True;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;True;12;all;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.DynamicAppendNode;4;157.4315,-15.52675;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;40;-1284.196,852.6171;Inherit;True;Property;_Dissolve_Texture;Dissolve_Texture;8;0;Create;True;0;0;0;False;0;False;-1;f016926f4da6b354eac19272eb7b012f;f016926f4da6b354eac19272eb7b012f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;2;-144.0052,-34.24195;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TexCoordVertexDataNode;62;-1197.608,1235.902;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-932.4672,1092.442;Inherit;False;Property;_Dissolve;Dissolve;9;0;Create;True;0;0;0;False;0;False;-0.35;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;64;-742.7802,1107.039;Inherit;False;Property;_Use_Custom;Use_Custom;17;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-2173.713,382.4833;Inherit;True;Property;_Mask_Texture;Mask_Texture;5;0;Create;True;0;0;0;False;0;False;-1;14eb01569c7174f48bf649760eac7654;14eb01569c7174f48bf649760eac7654;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;13;-1812.465,395.8853;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1520.835,394.6389;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1757.628,650.126;Inherit;False;Property;_Mask_Str;Mask_Str;6;0;Create;True;0;0;0;False;0;False;4.5;4.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2000.652,616.4764;Inherit;False;Constant;_1;1;2;0;Create;True;0;0;0;False;0;False;2;2.47;0;0;0;1;FLOAT;0
WireConnection;37;0;38;0
WireConnection;37;1;39;0
WireConnection;17;0;18;0
WireConnection;17;1;9;0
WireConnection;9;0;5;1
WireConnection;9;1;15;0
WireConnection;5;1;22;0
WireConnection;29;0;28;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;22;0;27;0
WireConnection;22;2;24;0
WireConnection;24;0;25;0
WireConnection;24;1;26;0
WireConnection;32;0;36;0
WireConnection;32;2;33;0
WireConnection;33;0;34;0
WireConnection;33;1;35;0
WireConnection;28;1;32;0
WireConnection;41;0;58;0
WireConnection;41;2;42;0
WireConnection;42;0;43;0
WireConnection;42;1;44;0
WireConnection;49;0;40;1
WireConnection;49;1;50;0
WireConnection;58;0;30;0
WireConnection;58;1;45;0
WireConnection;27;0;30;0
WireConnection;27;1;23;0
WireConnection;54;0;49;0
WireConnection;54;1;64;0
WireConnection;53;0;54;0
WireConnection;56;0;17;0
WireConnection;56;1;53;0
WireConnection;60;0;59;4
WireConnection;60;1;56;0
WireConnection;61;0;37;0
WireConnection;61;1;59;0
WireConnection;1;1;4;0
WireConnection;4;0;2;0
WireConnection;4;1;2;1
WireConnection;4;2;2;2
WireConnection;4;3;60;0
WireConnection;40;1;41;0
WireConnection;2;0;61;0
WireConnection;64;1;55;0
WireConnection;64;0;62;1
WireConnection;13;0;8;1
WireConnection;13;1;14;0
WireConnection;15;0;13;0
WireConnection;15;1;16;0
ASEEND*/
//CHKSM=A02826016CD48DFEB81815EC38F7831D8F67FA8B