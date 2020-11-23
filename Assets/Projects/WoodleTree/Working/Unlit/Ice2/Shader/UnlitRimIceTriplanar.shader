// Unlit Ice Shader with
// - Grabpass (common grabpass with all this shader instances)
// - Displacement (based on a normal map)
// - TintColor
// - Rim (based on normal map)

Shader "Polymole/Effects/Ice/UnlitRimIceTriplanar"
{
    Properties
    {
        _TintColor("Ice Tint Color", Color) = (1., 1., 1., 1.0)
        [PowerSlider(2.)] _Contrast("Contrast", Range(0.0, 3)) = 1

        [Header(Rim Effect)]
        [Toggle(RIM)] _RIM("Activate Rim", Float) = 0
        _RimColor("Rim Light Color", Color) = (0.1, 0.1, 0.9, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0

        [Header (Ice Displacement)]
        [NoScaleOffset] _DisplaceTexture ("Displacement Texture (Normal)", 2D) = "bump" {}
        [PowerSlider(3.0)] _DisplacementTiling("Displ. Tex. Tiling", Range(0.000001, 5)) = 1
        [PowerSlider(3.0)] _DisplacementPower ("Displ. Magnitude" , Range(0, 2.0)) = 1.0
        _BackgroundAbsorbtion ("Ice Absorbtion Rate", Range(0,1)) = 0.5

        [Header(Alpha Blending)]
        [KeywordEnum(None, Add, Multiply, Overlay)] _Blend ("Blending mode", Float) = 0
    }
    SubShader
    {
        Tags {"Queue"="Transparent+10" "IgnoreProjector"="True" "RenderType"="Opaque"}
        ZWrite On
        Lighting Off
        Blend Off

        LOD 100

        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // fog
            #pragma multi_compile_fog

            // blending options
            #pragma multi_compile _BLEND_NONE _BLEND_ADD _BLEND_MULTIPLY _BLEND_OVERLAY

            // rim option
            #pragma shader_feature RIM

            #include "UnityCG.cginc"

            #define EPSILON 0.0001

            struct VertexAttrib
            {
                float4 posOS : POSITION;
                float2 texCoord : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varys
            {
                float4 pos : SV_POSITION;
                float3 uvMain : TEXCOORD0;
                float2 uvDisplace : TEXCOORD1;
                float4 grabPos : TEXCOORD2;
                float3 normalCS : TEXCOORD3;
                float3 posWS : TEXCOORD4;
                float3 normalWS : TEXCOORD5;
                float3 triblend : TEXCOORD6;
                UNITY_FOG_COORDS(7)
            };

            // main texture
            uniform sampler2D _MainTex;
            uniform half4 _MainTex_ST;
            uniform fixed4 _TintColor;

            uniform half _Contrast;

            // displacement
            uniform sampler2D _DisplaceTexture;
            uniform half _DisplacementTiling;
            uniform half _DisplacementPower;
            uniform fixed _BackgroundAbsorbtion;

            // grab pass uniform
            uniform sampler2D _BackgroundTexture;
            uniform float4 _BackgroundTexture_TexelSize;

            // rim uniform
            uniform fixed4 _RimColor;
            uniform half _RimSharpnessF;
            uniform half _RimIntensityF;

            Varys vert (VertexAttrib v)
            {
                Varys o;

                o.pos = UnityObjectToClipPos(v.posOS);
                o.normalCS = UnityObjectToClipPos(v.normalOS);

                o.uvMain.xy = TRANSFORM_TEX(v.texCoord, _MainTex);
                o.uvDisplace.xy = v.texCoord * _DisplacementTiling;

            #ifdef RIM
                o.uvMain.z = saturate(dot(normalize(ObjSpaceViewDir(v.posOS)), v.normalOS) * _RimSharpnessF);
            #else
                o.uvMain.z = 0.0;
            #endif

                o.grabPos = ComputeGrabScreenPos(o.pos);

                // Triplanar
                // World Vertex Position
                o.posWS = mul(unity_ObjectToWorld, v.posOS).xyz;
                // world normal
                float3 worldNormal = UnityObjectToWorldNormal(v.normalOS);
                // triplanar normal blending weights
                float3 triblend = saturate(pow(worldNormal, 4));
                triblend /= max(dot(triblend, float3(1.0, 1.0, 1.0)), EPSILON);
                o.normalWS = worldNormal;
                o.triblend = triblend;

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {

                // triplanar world uv positions
                float2 uvX = i.posWS.yz;
                float2 uvY = i.posWS.xz;
                float2 uvZ = i.posWS.xy;
                half3 axisSign = i.normalWS < 0 ? -1 : 1;
                uvX.x *= axisSign.x;
                uvY.x *= axisSign.y;
                uvZ.x *= axisSign.z;

                // tangent space normal maps
                half4 normalColX = tex2D(_DisplaceTexture, uvX * _DisplacementTiling);
                half4 normalColY = tex2D(_DisplaceTexture, uvY * _DisplacementTiling);
                half4 normalColZ = tex2D(_DisplaceTexture, uvZ * _DisplacementTiling);

                // unpacking normal [0,1] => [-1, 1]
                half3 tnormalX = UnpackNormal(normalColX);
                half3 tnormalY = UnpackNormal(normalColY);
                half3 tnormalZ = UnpackNormal(normalColZ);

                // swizzle world normals to match tangent space and apply ala UDN normal blending
                // these should get normalized, but it's very a minor visual difference to skip it
                tnormalX = half3(tnormalX.xy + i.normalWS.zy, i.normalWS.x);
                tnormalY = half3(tnormalY.xy + i.normalWS.xz, i.normalWS.y);
                tnormalZ = half3(tnormalZ.xy + i.normalWS.xy, i.normalWS.z);

                // swizzle tangent normals to match world normal and blend together
                half3 normalWS = normalize(
                    tnormalX.zyx * i.triblend.x +
                    tnormalY.xzy * i.triblend.y +
                    tnormalZ.xyz * i.triblend.z
                );

                fixed2 displDir = normalWS;

                // main diffuse tex color
                fixed4 mainColor = _TintColor;

                //half3 refracted = refract(i.normal, half3(0,0,1), 1.333);
                half3 refracted = normalWS * abs(normalWS);

                // offsetting normal dir
                //refracted.xy * (i.uvrefr.w * _BumpAmt) + i.uvrefr.xy;
                i.grabPos.xy = i.grabPos.xy + refracted.xy * displDir.xy * i.grabPos.w * _DisplacementPower/5.;
                half3 grabColor = tex2Dproj(_BackgroundTexture, i.grabPos).rgb;
                grabColor = lerp(grabColor.rgb, _TintColor.rgb, _BackgroundAbsorbtion);

                fixed3 color;

            #ifdef _BLEND_NONE
                color = mainColor;
            #elif _BLEND_ADD
                color = mainColor.rgb + grabColor*(1 - mainColor.a);
            #elif _BLEND_OVERLAY
                color = grabColor < 0.5 ? 2.*grabColor*mainColor : 1.-2*(1-mainColor)*(1-grabColor);
            #elif _BLEND_MULTIPLY
                color = grabColor*mainColor;
            #endif

            color = pow(color, _Contrast);

            #ifdef RIM
                color = lerp(color, _RimColor, (1. - i.uvMain.z)*_RimIntensityF);
            #endif

                UNITY_APPLY_FOG(i.fogCoord, color);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
