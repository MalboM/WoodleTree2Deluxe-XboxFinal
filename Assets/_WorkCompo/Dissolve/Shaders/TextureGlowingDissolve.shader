Shader "Polymole/Effects/Dissolve/TextureGlowingDissolve"
{
    Properties
    {
        [Header(General Settings)]
        _MainTex("Main Texture", 2D) = "white" {}
        _TintColor("Front Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)

        [Header(Dissolve Settings)]
        _DissolveTex ("Dissolve Texture", 2D) = "black" {}
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0.5

        [Header(Rim Settings)]
        [Toggle(RIM)] _RIM("Activate Rim", Float) = 0
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0

        [Header(Glow)]
        [Toggle(GLOW)] _GLOW("Activate Glow", Float) = 0
        [HDR]_GlowColor("Color", Color) = (1, 1, 1, 1)
        [HDR]_BorderGlowColor("Color", Color) = (1, 1, 1, 1)
        _GlowAmount("Activation", Range(0,4)) = 0
        _GlowRange("Range", Range(0, 1)) = 0.1
        _GlowFalloff("Falloff", Range(0.001, 1)) = 0.1

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
            #pragma shader_feature GLOW

            // gpu instancing
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct VertexAttrib
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varys
            {
                float3 uvMain : TEXCOORD0;
                float2 uvDissolve : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 pos : SV_POSITION;
            };

            // dissolve settings
            uniform sampler2D _DissolveTex;
            uniform float4 _DissolveTex_ST;
            uniform half _DissolveAmount;

            // general settings
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed3 _TintColor;

            // rim settings
            uniform fixed3 _RimColor;
            uniform half _RimSharpnessF;
            uniform half _RimIntensityF;

            // glow settings
            uniform fixed4 _GlowColor;
            uniform fixed _GlowRange;
            uniform fixed _GlowFalloff;
            uniform fixed _GlowAmount;
            uniform sampler2D _GlowRamp;
            uniform fixed4 _BorderGlowColor;

            Varys vert (VertexAttrib v)
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

                o.uvDissolve = TRANSFORM_TEX(v.uv, _DissolveTex);

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                // sample the texture [0 -> first to dissolve, 1 -> last]
                float dissolve = tex2D(_DissolveTex, i.uvDissolve).r;
                dissolve *= 0.999;

                // clip if visibleThresh < 0
                float visibleThresh = dissolve - _DissolveAmount;
                clip(visibleThresh);

                // base texture color tinted
                fixed3 color = tex2D(_MainTex, i.uvMain.xy).rgb * _TintColor.rgb * unity_ColorSpaceDouble;

            #ifdef RIM
                color = lerp(color, _RimColor.rgb, (1. - i.uvMain.z) * _RimIntensityF);
            #endif

            #ifdef GLOW
                float isGlowing = smoothstep(_GlowRange + _GlowFalloff, _GlowRange, visibleThresh);
                //float glow = tex2D(_GlowRamp, float2(visibleThresh * (1 / _GlowRange), 0)) * _GlowColor * _GlowAmount;
                //float glow = tex2D(_GlowRamp, float2(visibleThresh, 0));
                //float isGlowing = 1 / _GlowRange * _DissolveAmount;
                //return colormapGrayEx(0,1,isGlowing);
                float3 glow = _GlowColor.rgb * _GlowColor.a * _GlowAmount;
                float3 border = isGlowing * _BorderGlowColor.rgb * _GlowAmount * _BorderGlowColor.a;
                color += (border + glow) * _DissolveAmount;
            #endif

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
