// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "FXShader/RG_Master"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[ASEBegin]_Main_Texture("Main_Texture", 2D) = "white" {}
		[HDR]_R_Color("R_Color", Color) = (1,0,0,0)
		[HDR]_G_Color("G_Color", Color) = (0,1,0,0)
		_Main_Ins("Main_Ins", Float) = 1
		_Dissolve_Texture("Dissolve_Texture", 2D) = "white" {}
		_Dissolve("Dissolve", Range( -1 , 1)) = 1
		[Toggle(_USE_CUSTOM_ON)] _Use_Custom("Use_Custom", Float) = 0
		[ASEEnd][Toggle(_USE_STEP_DISSOLVE_ON)] _Use_Step_Dissolve("Use_Step_Dissolve", Float) = 0
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
			#pragma shader_feature_local _USE_STEP_DISSOLVE_ON


			sampler2D _Main_Texture;
			sampler2D _Dissolve_Texture;
			CBUFFER_START( UnityPerMaterial )
			float4 _R_Color;
			float4 _Main_Texture_ST;
			float4 _G_Color;
			float4 _Dissolve_Texture_ST;
			float _Main_Ins;
			float _Dissolve;
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

				#ifdef _USE_CUSTOM_ON
				float staticSwitch23 = IN.texCoord0.z;
				#else
				float staticSwitch23 = _Main_Ins;
				#endif
				float2 uv_Main_Texture = IN.texCoord0.xy * _Main_Texture_ST.xy + _Main_Texture_ST.zw;
				float4 tex2DNode2 = tex2D( _Main_Texture, uv_Main_Texture );
				float4 tex2DNode3 = tex2D( _Main_Texture, uv_Main_Texture );
				float4 break11 = ( ( staticSwitch23 * ( ( _R_Color * tex2DNode2.r ) + ( tex2DNode3.g * _G_Color ) ) ) * IN.color );
				float2 uv_Dissolve_Texture = IN.texCoord0.xy * _Dissolve_Texture_ST.xy + _Dissolve_Texture_ST.zw;
				float4 tex2DNode32 = tex2D( _Dissolve_Texture, uv_Dissolve_Texture );
				#ifdef _USE_CUSTOM_ON
				float staticSwitch33 = IN.texCoord0.w;
				#else
				float staticSwitch33 = _Dissolve;
				#endif
				float temp_output_35_0 = saturate( ( tex2DNode32.r + staticSwitch33 ) );
				float smoothstepResult36 = smoothstep( tex2DNode32.r , staticSwitch33 , temp_output_35_0);
				#ifdef _USE_STEP_DISSOLVE_ON
				float staticSwitch38 = ( 1.0 - smoothstepResult36 );
				#else
				float staticSwitch38 = temp_output_35_0;
				#endif
				float4 appendResult14 = (float4(break11.r , break11.g , break11.b , ( IN.color.a * ( ( tex2DNode2.r + tex2DNode3.g ) * staticSwitch38 ) )));
				
				float4 Color = appendResult14;

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
17;99;1775;828;3143.442;615.041;1.808311;True;False
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-1706.766,224.4092;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;30;-1055.551,931.6608;Inherit;False;Property;_Dissolve;Dissolve;5;0;Create;True;0;0;0;False;0;False;1;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;33;-756.1133,967.6744;Inherit;False;Property;_Use_Custom;Use_Custom;7;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;32;-815.5726,738.483;Inherit;True;Property;_Dissolve_Texture;Dissolve_Texture;4;0;Create;True;0;0;0;False;0;False;-1;b0c552b9a0531564dbf486ea02aab0e8;f016926f4da6b354eac19272eb7b012f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1180.5,77;Inherit;True;Property;_Main_Texture;Main_Texture;0;0;Create;True;0;0;0;False;0;False;39b497d0c47bacf4c8faee6ab225cfcb;7817bcc8850df4e44a6bc3714c94fb11;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-524.2171,750.1668;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;35;-323.8255,741.4665;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-763.5,-224;Inherit;False;Property;_R_Color;R_Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;1,0.909465,0.75,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-844.5,12;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-774.5,273;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-747.5,510.2893;Inherit;False;Property;_G_Color;G_Color;2;1;[HDR];Create;True;0;0;0;False;0;False;0,1,0,0;1.676832,0.91333,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;36;-330.5562,967.8154;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-239.4518,-6.167725;Inherit;False;Property;_Main_Ins;Main_Ins;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-391.5,364;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-395.5,32;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;5;-222.5,180;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;37;-166.8687,870.4583;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;23;-68.17607,-99.23822;Inherit;False;Property;_Use_Custom;Use_Custom;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-224.949,420.5161;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;60.54822,49.83228;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;15;6.688079,187.3395;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;38;-107.6413,708.9106;Inherit;False;Property;_Use_Step_Dissolve;Use_Step_Dissolve;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;214.0353,440.335;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;226.39,24.83197;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;11;343.861,157.9475;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;307.39,308.832;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;14;462.861,151.9475;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;610,45;Float;False;True;-1;2;ASEMaterialInspector;0;18;FXShader/RG_Master;cf964e524c8e69742b1d21fbe2ebcc4a;True;Unlit;0;0;Unlit;3;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;0;True;2;5;False;-1;10;False;-1;3;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;1;True;False;;False;0
WireConnection;33;1;30;0
WireConnection;33;0;24;4
WireConnection;34;0;32;1
WireConnection;34;1;33;0
WireConnection;35;0;34;0
WireConnection;2;0;4;0
WireConnection;3;0;4;0
WireConnection;36;0;35;0
WireConnection;36;1;32;1
WireConnection;36;2;33;0
WireConnection;9;0;3;2
WireConnection;9;1;8;0
WireConnection;7;0;6;0
WireConnection;7;1;2;1
WireConnection;5;0;7;0
WireConnection;5;1;9;0
WireConnection;37;0;36;0
WireConnection;23;1;21;0
WireConnection;23;0;24;3
WireConnection;18;0;2;1
WireConnection;18;1;3;2
WireConnection;22;0;23;0
WireConnection;22;1;5;0
WireConnection;38;1;35;0
WireConnection;38;0;37;0
WireConnection;39;0;18;0
WireConnection;39;1;38;0
WireConnection;16;0;22;0
WireConnection;16;1;15;0
WireConnection;11;0;16;0
WireConnection;17;0;15;4
WireConnection;17;1;39;0
WireConnection;14;0;11;0
WireConnection;14;1;11;1
WireConnection;14;2;11;2
WireConnection;14;3;17;0
WireConnection;1;1;14;0
ASEEND*/
//CHKSM=53C6BBBFF252F367D9468DB14EC23150EA09336F