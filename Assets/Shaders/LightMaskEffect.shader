Shader "Custom/LightMaskEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  
        _LightPos ("Light Position", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _LightPos;
            float _Radius;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                float dist = distance(uv, _LightPos.xy);

                fixed4 col = tex2D(_MainTex, uv);

                if (dist > _Radius)
                {
                    col.rgb = float3(0, 0, 0); // negro
                }

                return col;
            }
            ENDCG
        }
    }
}
