Shader "UI/CircleHoleDistorted" {
    Properties {
        _Color("Overlay Color", Color) = (0,0,0,0.5)
        _Radius("Base Radius", Range(0,1)) = 0.3
        _BorderTex("Border Texture", 2D) = "white" {}
        _BorderWidth("Border Width", Range(0,0.5)) = 0.001
        _RotationSpeed("Rotation Speed", Float) = 1.0

        _RingColor0("Red Ring Color", Color) = (1,0,0,1)
        _RingColor1("Blue Ring Color", Color) = (0,0,1,1)
        _RingColor2("Yellow Ring Color", Color) = (1,1,0,1)
        _RingColor3("Green Ring Color", Color) = (0,1,0,1)
        _RotationOffsets("Rotation Offsets", Vector) = (0,0.25,0.5,0.75)

        _GlowStrength("Glow Strength", Float) = 1.0
        _DistortionTex("Distortion Texture (Voronoi)", 2D) = "gray" {}
        _DistortionStrength("Distortion Strength", Float) = 0.05

        _PlayerScreenPos("Player Screen Position", Vector) = (0.5, 0.5, 0, 0)
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _Radius;
            sampler2D _BorderTex;
            float _BorderWidth;
            float _RotationSpeed;

            fixed4 _RingColor0, _RingColor1, _RingColor2, _RingColor3;
            float4 _RotationOffsets;
            float _GlowStrength;
            sampler2D _DistortionTex;
            float _DistortionStrength;

            float4 _PlayerScreenPos;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f    { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float voronoi(float2 uv) {
                float2 i = floor(uv);
                float2 f = frac(uv);
                float m = 1e5;
                for(int y=-1; y<=1; ++y) for(int x=-1; x<=1; ++x) {
                    float2 b = float2(x,y);
                    float2 r = b + frac(sin(float2(dot(i+b,float2(127.1,311.7)), dot(i+b,float2(269.5,183.3)))) * 43758.5453) - f;
                    m = min(m, dot(r,r));
                }
                return sqrt(m);
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 screenUV = i.pos.xy / _ScreenParams.xy;
                screenUV.y = 1.0 - screenUV.y;

                float2 centered = screenUV - _PlayerScreenPos.xy;
                centered.x *= _ScreenParams.x / _ScreenParams.y;

                float dist = length(centered);
                float angle = atan2(centered.y, centered.x);
                float angle01 = frac((angle + UNITY_PI) / (2 * UNITY_PI));

                // Dynamic radius via voronoi distortion
                float d = tex2D(_DistortionTex, float2(angle01, 0.5)).r;
                float dynamicRadius = _Radius + (d - 0.5) * 2 * _DistortionStrength;
                float innerBase = dynamicRadius - _BorderWidth;

                fixed3 ringColor = 0;
                bool inRing = false;
                fixed4 ringColors[4] = {_RingColor0, _RingColor1, _RingColor2, _RingColor3};

                for(int idx=0; idx<4; idx++) {
                    float inner = innerBase + idx * _BorderWidth;
                    float outer = inner + _BorderWidth;
                    if(dist > inner && dist < outer) {
                        float t = (dist - inner) / _BorderWidth;
                        float u = frac(angle01 + _Time.y * _RotationSpeed + _RotationOffsets[idx]);
                        float3 sample = tex2D(_BorderTex, float2(u, t)).rgb;
                        ringColor += sample * ringColors[idx].rgb;
                        inRing = true;
                    }
                }

                if(dist < innerBase) return fixed4(0,0,0,0);
                if(inRing) {
                    float glow = dot(ringColor, float3(0.3,0.59,0.11)) * _GlowStrength;
                    return fixed4(ringColor * glow, glow);
                }
                if(dist < dynamicRadius + _BorderWidth * 2) {
                    // Return overlay with its alpha, allowing background visibility
                    return fixed4(_Color.rgb, _Color.a);
                }
                return fixed4(_Color.rgb, _Color.a);
            }
            ENDCG
        }
    }
}









