Shader "Unlit/GB"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GBDarkest("GB (Darkest)", Color) = (0.06, 0.22, 0.06, 1.0)
        _GBDark("GB (Dark)", Color) = (0.19, 0.38, 0.19, 1.0)
        _GBLight("GB (Light)", Color) = (0.54, 0.67, 0.06, 1.0)
        _GBLightest("GB (Lightest)", Color) = (0.61, 0.73, 0.06, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _GBDarkest;
            float4 _GBDark;
            float4 _GBLight;
            float4 _GBLightest;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float4 tex = tex2D(_MainTex, i.uv);
                float lum = dot(tex, float3(0.3, 0.59, 0.11));

                int gb = lum * 3;

                float3 col = lerp(_GBDarkest, _GBDark, saturate(gb));
                col = lerp(col, _GBLight, saturate(gb - 1.0));
                col = lerp(col, _GBLightest, saturate(gb - 2.0));

                return float4(col, 1.0);

            }
            ENDCG
        }
    }
}