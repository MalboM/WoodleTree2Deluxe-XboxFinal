Shader "WoodleTree/Base/BaseTextureVertexRotation"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)

        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 uvw : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            fixed4 _RimColor;
            fixed4 _TintColor;
            float _RimSharpnessF;
            float _RimIntensityF;
            
            float _OscAmplitudeF;
            float _OscPeriodF;
            float _RotationPeriodF;

            void RotateAroundYAxis (inout float4 vertex, in float period)
            {
                fixed timePeriod = _Time.y * period;
                float sint, cost;
                sincos(timePeriod, sint, cost);

                float4x4 m2 = float4x4(
                    cost, 0, sint, 0,
                    0, 1, 0, 0,
                    -sint, 0, cost, 0,
                    0, 0, 0, 1);

                vertex = mul(m2, vertex);
            }

            v2f vert (appdata v)
            {
                v2f o;

                // rotate vertex
                RotateAroundYAxis(v.vertex, _RotationPeriodF);
                
                // rotate normal
                float4 normal = float4(v.normal, 0.);
                RotateAroundYAxis(normal, _RotationPeriodF);
                normal = mul(UNITY_MATRIX_V, normal);
                
                // transform view direction
                float3 vdir = mul((float3x3)UNITY_MATRIX_V, ObjSpaceViewDir(v.vertex));
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uvw = float3(
                    TRANSFORM_TEX(v.uv, _MainTex),
					 clamp(dot(normalize(vdir), normal) *_RimSharpnessF, 0.0, 1.0)
			    );

                o.vertex.y -= sin(_Time.y * _OscPeriodF) * _OscAmplitudeF;

                UNITY_TRANSFER_FOG(o,o.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // calculate base colo via texture
                fixed4 base_col = tex2D(_MainTex, i.uvw) * _TintColor * 2.0;
                // lerping between rim color and base_color via 
                fixed4 col = lerp(_RimColor, base_col, i.uvw.z);

                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
