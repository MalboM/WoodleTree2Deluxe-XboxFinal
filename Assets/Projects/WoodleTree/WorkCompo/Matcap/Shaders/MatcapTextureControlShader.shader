// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Polymole/Effects/Matcap/MatcapTextureControiShader"
{
    Properties
    {
        [Header(General Settings)]
        _MainTex("Main Texture", 2D) = "white" {}
        _TintColor("Front Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)

        [Header(Matcap Settings)]
        _MatcapTex ("Matcap texture", 2D) = "white" {}
        _MatTintColor("Matcap Tint Color", Color) = (1.0,1.0,1.0,1.0)

        _MatcapControlTex("Matcap control texture",2D) = "black"{}

        [Header(Debug)]
        [Toggle(MATCAP)] _MATCAP("Matcap Texture", Float) = 0

        [Header(Rim Settings)]

        [Toggle(RIM)] _RIM("Activate Rim", Float) = 0
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        LOD 100

        Cull [_Cull]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            // shader features
            #pragma shader_feature RIM
            #pragma shader_feature CONTROL
            #pragma shader_feature MATCAP

            // gpu instancing
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct VertexData
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varys
            {
                float3 uvMain : TEXCOORD0;
                float3 uvCap : TEXCOORD1;
                float3 viewNorm: TEXCOORD2;
                float3 worldNorm: TEXCOORD3;
                UNITY_FOG_COORDS(4)
                float4 pos : SV_POSITION;
            };


            // general settings
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed3 _TintColor;

            // matcap settings
            uniform sampler2D _MatcapTex;
            uniform sampler2D _MatcapControlTex;
            uniform float4 _MatcapTex_ST;
            uniform float4 _MatcapControlTex_ST;
            uniform fixed4 _MatTintColor;
            uniform float3 _StretchDirection;
            uniform float _StretchStrength;
            uniform float _StretchPow;


            // rim settings
            uniform fixed3 _RimColor;
            uniform half _RimSharpnessF;
            uniform half _RimIntensityF;


            Varys vert (VertexData v)
            {
                Varys o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uvMain = float3(
                    TRANSFORM_TEX(v.uv, _MainTex),
            #ifdef RIM
                    saturate(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) * _RimSharpnessF)
            #else
                    0.0
            #endif
                );

                float3 worldNorm = normalize(
                        unity_WorldToObject[0].xyz * v.normal.x +
                        unity_WorldToObject[1].xyz * v.normal.y +
                        unity_WorldToObject[2].xyz * v.normal.z
                        );

                // this is world normals
                o.worldNorm = worldNorm;

                // transform normal vectors from world space to view space
                float3 viewNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
                // or use built-in UNITY_MATRIX_V

                // this is viewspace normals
                o.viewNorm = viewNorm;

                // this is in the context of view space
                // get the coordinate on XY plane, ignore z coordinate
                o.uvCap.xy = viewNorm.xy * 0.5 + 0.5; // clamp (-1,1) to (0, 1)

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {


                float4 matCapCol = tex2D(_MatcapTex, i.uvCap.xy);

                // base texture color tinted
                fixed3 baseCol = tex2D(_MainTex, i.uvMain.xy).rgb * _TintColor.rgb * unity_ColorSpaceDouble;
                fixed3 finalCol = baseCol;
                fixed3 controlCol = tex2D(_MatcapControlTex, i.uvMain.xy).rgb*_MatTintColor.rgb;
                finalCol = controlCol.rgb*matCapCol.rgb + (1-controlCol.rgb)*baseCol;


            #ifdef RIM
                finalCol = lerp(finalCol, _RimColor.rgb, (1. - i.uvMain.z) * _RimIntensityF);
            #endif



            #ifdef MATCAP
                finalCol.rgb = matCapCol.rgb;
            #endif

                fixed4 finalRGBA = fixed4(finalCol,1.);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);

                return finalRGBA;
            }
            ENDCG
        }
    }
}

