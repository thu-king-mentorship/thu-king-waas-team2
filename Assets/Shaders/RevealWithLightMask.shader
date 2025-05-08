Shader "Unlit/RevealWithLightMask"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _LightMaskTex ("Light Mask", 2D) = "black" {}
        _RevealThreshold ("Reveal Threshold", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _LightMaskTex;
            float4 _MainTex_ST;
            float _RevealThreshold;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = o.pos;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV = screenUV * 0.5 + 0.5;
                screenUV.y = 1.0 - screenUV.y;

                float light = tex2D(_LightMaskTex, screenUV).r;

                float alpha = light;
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= alpha;
                return col;
            }
            ENDCG
        }
    }
}
