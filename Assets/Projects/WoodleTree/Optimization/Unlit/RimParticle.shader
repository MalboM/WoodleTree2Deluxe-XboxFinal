Shader "WoodleTree/Particles/RimParticle"
{
    Properties
    {
        _MainColor("Object Color", Color) = (0.5, 0.7, 0.2, 1.0)
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "IgnoreProjector"="True"}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:vertInstancingSetup

            #include "UnityStandardParticleShadow.cginc"

            struct VertexData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                fixed4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varys
            {
                fixed4 color : COLOR;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            uniform fixed4 _MainColor;
            uniform fixed4 _RimColor;
            uniform half _RimIntensityF;
            uniform half _RimSharpnessF;

            Varys vert (VertexData v)
            {
                Varys o;
                UNITY_SETUP_INSTANCE_ID(v);

                o.vertex = UnityObjectToClipPos(v.vertex);

                float rimF = clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) * _RimSharpnessF, 0.0, 1.0);
                o.color = lerp(_MainColor, _RimColor, (1. - rimF)*_RimIntensityF);

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                // sample the texture
                fixed4 col = i.color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
