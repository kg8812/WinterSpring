Shader "Custom/FogOfWarShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _AlphaTex ("Alpha Texture", 2D) = "white" {}
        _ClearTex ("Clear Texture", 2D) = "black" {}

        [HideInInspector]_StencilComp("Stencil Comparison", Float) = 3
        [HideInInspector]_Stencil("Stencil ID", Float) = 1
        [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 0
        [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 1
        [HideInInspector]_ColorMask("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            sampler2D _ClearTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float alpha = tex2D(_AlphaTex, i.uv).a;
                float maskGray = tex2D(_ClearTex, i.uv).r;
                fixed4 mainColor = tex2D(_MainTex, i.uv);

                // AlphaTex의 알파 채널을 MainTex에 적용
                mainColor.a *= alpha;

                // grayScale
                fixed gray = saturate((mainColor.r + mainColor.g + mainColor.r) * 0.33);
                fixed3 finalColor = lerp(gray.rrr, mainColor.rgb, maskGray);

                return fixed4(finalColor, mainColor.a);
            }
            ENDCG
        }
    }
}