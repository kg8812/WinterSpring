// Made with Amplify Shader Editor v1.9.2.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXS_Fire_Tornado02"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_Dissolve_Texture("Dissolve_Texture", 2D) = "white" {}
		_Rot("Rot", Float) = 0.93
		_Diss_UPanner("Diss_UPanner", Float) = 1
		_Diss_VPanner("Diss_VPanner", Float) = -0.6
		_Noise_UPanner("Noise_UPanner", Float) = 0.5
		_Noise_VPanner("Noise_VPanner", Float) = -0.3
		_Mask_Texture("Mask_Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Float) = -0.38
		_Mask_Range("Mask_Range", Float) = 4.25
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		[HDR]_Color("Color", Color) = (1,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Back
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

			

			sampler2D _Dissolve_Texture;
			sampler2D _TextureSample0;
			sampler2D _Mask_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _Color;
			float4 _Mask_Texture_ST;
			float _Diss_UPanner;
			float _Diss_VPanner;
			float _Noise_UPanner;
			float _Noise_VPanner;
			float _Rot;
			float _Dissolve;
			float _Mask_Range;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				
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

				float4 break59 = _Color;
				float2 appendResult18 = (float2(_Diss_UPanner , _Diss_VPanner));
				float2 appendResult56 = (float2(_Noise_UPanner , _Noise_VPanner));
				float2 texCoord14 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult13 = (float2(( texCoord14.x + ( texCoord14.y * _Rot ) ) , texCoord14.y));
				float2 panner50 = ( 1.0 * _Time.y * appendResult56 + appendResult13);
				float2 panner12 = ( 1.0 * _Time.y * appendResult18 + ( ( (UnpackNormalScale( tex2D( _TextureSample0, panner50 ), 1.0f )).xy * 0.19 ) + appendResult13 ));
				float4 tex2DNode10 = tex2D( _Dissolve_Texture, panner12 );
				float temp_output_11_0 = ( tex2DNode10.r + _Dissolve );
				float2 uv_Mask_Texture = IN.texCoord0.xy * _Mask_Texture_ST.xy + _Mask_Texture_ST.zw;
				float temp_output_25_0 = saturate( pow( tex2D( _Mask_Texture, uv_Mask_Texture ).r , _Mask_Range ) );
				float temp_output_31_0 = ( temp_output_11_0 * temp_output_25_0 );
				float4 appendResult61 = (float4(break59.r , break59.g , break59.b , step( ( temp_output_31_0 + ( temp_output_31_0 * ( temp_output_11_0 + temp_output_25_0 ) ) ) , -0.01 )));
				
				float4 Color = appendResult61;

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
Node;AmplifyShaderEditor.SamplerNode;10;-1120.248,-30.58154;Inherit;True;Property;_Dissolve_Texture;Dissolve_Texture;0;0;Create;True;0;0;0;False;0;False;-1;f016926f4da6b354eac19272eb7b012f;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;25;-829.0149,404.5643;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;24;-1051.015,395.5643;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;28;-620.0149,34.5643;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-832.0149,13.5643;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-978.3267,-161.2534;Inherit;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;0.28;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;32;-721.3267,-216.2534;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1233.015,651.5643;Inherit;False;Property;_Mask_Range;Mask_Range;8;0;Create;True;0;0;0;False;0;False;4.25;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-1394.015,402.5643;Inherit;True;Property;_Mask_Texture;Mask_Texture;6;0;Create;True;0;0;0;False;0;False;-1;62783772e12d75f41a516efc80a0b118;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-523.5129,532.934;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-416.1266,231.5466;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-414.0149,-28.4357;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-122.166,377.0051;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;96.83398,190.0051;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;118.834,477.0051;Inherit;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;0;False;0;False;-0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;43;355.834,191.0051;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-1343.945,-205.4135;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;45;-2128.722,-381.6405;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;0;False;0;False;-1;e352ba45d12e9ea449f86114ee6bb980;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;47;-1799.238,-346.3875;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1714.238,-228.3875;Inherit;False;Constant;_Float2;Float 2;8;0;Create;True;0;0;0;False;0;False;0.19;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-1565.238,-364.3875;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;12;-1346.015,-0.4356995;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-1475.015,115.5643;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-1658.015,129.5643;Inherit;False;Property;_Diss_UPanner;Diss_UPanner;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1659.015,202.5643;Inherit;False;Property;_Diss_VPanner;Diss_VPanner;3;0;Create;True;0;0;0;False;0;False;-0.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;50;-2345.597,-400.3394;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;993.889,134.1743;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;18;FXS_Fire_Tornado02;cf964e524c8e69742b1d21fbe2ebcc4a;True;Unlit;0;0;Unlit;3;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;True;12;all;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.BreakToComponentsNode;59;579.3466,-7.135101;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ColorNode;60;310.3466,-40.1351;Inherit;False;Property;_Color;Color;10;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;61;787.3466,1.864899;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1025.219,173.8205;Inherit;False;Property;_Dissolve;Dissolve;7;0;Create;True;0;0;0;False;0;False;-0.38;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-3544.187,-114.5302;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-3267.187,-130.5302;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-3266.187,70.46992;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-3478.187,77.46989;Inherit;False;Property;_Rot;Rot;1;0;Create;True;0;0;0;False;0;False;0.93;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;-3115.868,-89.09888;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-2504.956,-251.2222;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-2687.957,-237.2223;Inherit;False;Property;_Noise_UPanner;Noise_UPanner;4;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-2688.957,-164.2223;Inherit;False;Property;_Noise_VPanner;Noise_VPanner;5;0;Create;True;0;0;0;False;0;False;-0.3;0;0;0;0;1;FLOAT;0
WireConnection;10;1;12;0
WireConnection;25;0;24;0
WireConnection;24;0;23;1
WireConnection;24;1;26;0
WireConnection;28;0;11;0
WireConnection;11;0;10;1
WireConnection;11;1;21;0
WireConnection;32;0;10;1
WireConnection;32;1;33;0
WireConnection;36;0;11;0
WireConnection;36;1;25;0
WireConnection;31;0;11;0
WireConnection;31;1;25;0
WireConnection;29;0;28;0
WireConnection;29;1;25;0
WireConnection;41;0;31;0
WireConnection;41;1;36;0
WireConnection;42;0;31;0
WireConnection;42;1;41;0
WireConnection;43;0;42;0
WireConnection;43;1;44;0
WireConnection;46;0;48;0
WireConnection;46;1;13;0
WireConnection;45;1;50;0
WireConnection;47;0;45;0
WireConnection;48;0;47;0
WireConnection;48;1;49;0
WireConnection;12;0;46;0
WireConnection;12;2;18;0
WireConnection;18;0;19;0
WireConnection;18;1;20;0
WireConnection;50;0;13;0
WireConnection;50;2;56;0
WireConnection;4;1;61;0
WireConnection;59;0;60;0
WireConnection;61;0;59;0
WireConnection;61;1;59;1
WireConnection;61;2;59;2
WireConnection;61;3;43;0
WireConnection;15;0;14;1
WireConnection;15;1;16;0
WireConnection;16;0;14;2
WireConnection;16;1;17;0
WireConnection;13;0;15;0
WireConnection;13;1;14;2
WireConnection;56;0;57;0
WireConnection;56;1;58;0
ASEEND*/
//CHKSM=BD3FF68DDA1293E4FFF8C3B6A58CF0E7287ABC72