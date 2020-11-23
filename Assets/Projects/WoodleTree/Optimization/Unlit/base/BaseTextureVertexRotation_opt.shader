Shader "WoodleTree/Base/BaseTextureVertexRotation (Vertex Opt)"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
        
        [Header(Rim Properties)]
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0
        
        [Header(Movement Properties)]
        _OscAmplitudeF("Vertical Amp", Float) = 0.4
        _OscPeriodF("Vertical Speed", Float) = 2.5
        _RotationPeriodF("Rotation Speed", Float) = 2.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct VertexData
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 objPos : POSITION;
                float2 uv : TEXCOORD0;
                float3 objNormal : NORMAL;
            };

            struct Varys
            {
                float4 pos : SV_POSITION;
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;

            uniform fixed4 _RimColor;
            uniform fixed4 _TintColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;

            uniform float _OscAmplitudeF;
            uniform float _OscPeriodF;
            uniform float _RotationPeriodF;

            inline float2x2 CreateYRotationMatrix (float period)
            {
                fixed timePeriod = _Time.y * period;
                float sint, cost;
                
                sincos(timePeriod, sint, cost);
                return float2x2(
                    cost,sint,
                    -sint,cost
                );
            }

            Varys vert (VertexData v)
            {
                Varys o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o); 

                float2x2 m2 = CreateYRotationMatrix(_RotationPeriodF);
                // rotate vertex
                v.objPos = float4(mul(m2, v.objPos.xz), v.objPos.yw).xzyw; 
                // rotate normal
                v.objNormal = float3(mul(m2, v.objNormal.xz), v.objNormal.y).xzy; 
                
                // transform view direction
                float3 vdir = ObjSpaceViewDir(v.objPos);
                
                o.pos = UnityObjectToClipPos(v.objPos);
                o.uv = float3(
                    TRANSFORM_TEX(v.uv, _MainTex),
                    clamp(dot(normalize(vdir), v.objNormal) * _RimSharpnessF, 0.0, 1.0)
                );
                
                o.pos.y -= sin(_Time.y * _OscPeriodF) * _OscAmplitudeF;
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                // calculate base colo via texture
                fixed4 base_col = tex2D(_MainTex, i.uv.xy) * _TintColor * 2.0;
                // lerping between rim color and base_color via 1 - rimFactor
                fixed4 col = lerp(base_col, _RimColor, (1. - i.uv.z) * _RimIntensityF);
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}