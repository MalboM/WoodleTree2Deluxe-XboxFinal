Shader "Polymole/Effects/Dissolve/Base Color Dissolve (vertex)"
{
    Properties
    {
        [Header(General Settings)]
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
        _GlowRange("Range", Range(0, .3)) = 0.1
        _GlowFalloff("Falloff", Range(0.001, .3)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        LOD 100

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
                float4 OSPos : POSITION;
                float3 OSNormal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varys
            {
                float2 uvMain : TEXCOORD0;
                fixed3 color : COLOR;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
            };

            // dissolve settings
            uniform sampler2D _DissolveTex;
            uniform float4 _DissolveTex_ST;
            uniform half _DissolveAmount;

            // general settings
            uniform fixed3 _TintColor;

            // rim settings
            uniform fixed3 _RimColor;
            uniform half _RimSharpnessF;
            uniform half _RimIntensityF;

            // glow settings
            uniform fixed3 _GlowColor;
            uniform fixed _GlowRange;
            uniform fixed _GlowFalloff;

            Varys vert (VertexAttrib v)
            {
                Varys o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);

                o.pos = UnityObjectToClipPos(v.OSPos);
                o.uvMain = TRANSFORM_TEX(v.uv, _DissolveTex);

                o.color = _TintColor.rgb * unity_ColorSpaceDouble;
            #ifdef RIM
                o.color = lerp(o.color, _RimColor.rgb, (1. - saturate(dot(normalize(ObjSpaceViewDir(v.OSPos)), v.OSNormal) * _RimSharpnessF)) * _RimIntensityF);
            #endif

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                // sample the texture [0 -> first to dissolve, 1 -> last]
                float dissolve = tex2D(_DissolveTex, i.uvMain.xy).r;
                dissolve *= 0.999;

                // clip if visibleThresh < 0
                float visibleThresh = dissolve - _DissolveAmount;
                clip(visibleThresh);

                fixed3 color = i.color;

            #ifdef GLOW
                float isGlowing = smoothstep(_GlowRange + _GlowFalloff, _GlowRange, visibleThresh);
                //return colormapGrayEx(0,1,isGlowing);
                float3 glow = isGlowing * _GlowColor;
                color += glow;
            #endif

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
