Shader "WoodleTree/Unlit/Silhouette/Disabled/base_texture (CamAlphaBlend - VertDist)"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)

        [Header(Rim Settings)]
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0

        [Header(Camera Squared Distance Alpha Decay Settings)]
        _CameraDistanceOffset("Alpha Decay Offset", Float) = 100
        _CameraDistanceDuration("Alpha Decay Interval Speed", Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry+1"}
        Blend SrcAlpha OneMinusSrcAlpha

        Stencil {
            Ref 2
            Comp Always
            Pass Replace
        }

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
                float camSqDist : TEXCOORD2;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed4 _TintColor;

            // rim
            uniform fixed4 _RimColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;

            // cam
            uniform float _CameraDistanceDebug;
            uniform float _CameraDistanceDuration;
            uniform float _CameraDistanceOffset;

            Varys vert (VertexData v)
            {
                Varys o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);

                o.pos = UnityObjectToClipPos(v.objPos);
                o.uv = float3(
                    TRANSFORM_TEX(v.uv, _MainTex),
                    clamp(dot(normalize(ObjSpaceViewDir(v.objPos)), v.objNormal) * _RimSharpnessF, 0.0, 1.0)
                );

                // view dir squared magnitude
                float4 posWS = mul(unity_ObjectToWorld, v.objPos);
                float3 viewDirWS = UnityWorldSpaceViewDir(posWS);
                o.camSqDist = dot(viewDirWS, viewDirWS);

                UNITY_TRANSFER_FOG(o,o.pos);

                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                // base texture color tinted
                fixed4 base_col = tex2D(_MainTex, i.uv.xy) * _TintColor * unity_ColorSpaceDouble;

                // final color
                fixed4 col = lerp(base_col, _RimColor, (1. - i.uv.z) * _RimIntensityF);

                //float alpha = clamp(smoothstep(max(_CameraDistanceOffset - _CameraDistanceDebug, 0), _CameraDistanceDuration, _CameraDistanceDebug), 0, 1);
                float activationDistance = clamp(i.camSqDist - _CameraDistanceOffset, 0, _CameraDistanceDuration);
                float alpha = 1-smoothstep(0, _CameraDistanceDuration, activationDistance);

                UNITY_APPLY_FOG(i.fogCoord, col);

                return float4(col.rgb, alpha);
            }
            ENDCG
        }
    }
		FallBack "Diffuse"
}
