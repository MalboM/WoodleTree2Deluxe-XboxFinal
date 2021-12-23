Shader "Polymole/FX/Vortex/Vortex Main (unlit)"
{
    Properties
    {
        //Clipping
        [Space(20)]
        _Cutoff("Mask Clip Value", Range(0, 1)) = 0.5

        _MainTex("Main Tex", 2D) = "white" {}
        _Mask("Mask", 2D) = "white" {}
        _Noise("Noise", 2D) = "white" {}
        _SpeedMainTexUVNoiseZW("Speed MainTex U/V + Noise Z/W", Vector) = (0,0,0,0)

        _Alpha("Alpha Color", Range(0, 1)) = 0.5
        _FrontFacesColor("Front Faces Color", Color) = (0,0.2313726,1,1)
        _BackFacesColor("Back Faces Color", Color) = (0.1098039,0.4235294,1,1)
        _Emission("Emission", Float) = 2

        [MaterialToggle] _UseFresnel("Use Fresnel?", Float) = 1
        _FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        _Fresnel("Fresnel", Float) = 1
        _FresnelEmission("Fresnel Emission", Float) = 1

        [MaterialToggle] _UseCustomData("Use Custom Data?", Float) = 0
        [HideInInspector] _texcoord( "", 2D ) = "white" {}
        [HideInInspector] _tex4coord( "", 2D ) = "white" {}
        [HideInInspector] __dirty( "", Int ) = 1
    }

    SubShader
    {
        //Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent" "IsEmissive" = "true" "PreviewType"="Plane" }
        Tags{ "PreviewType"="Plane" }

        // I use a neat little trick with the “Offset -1, -1” tag. This is to avoid Z-fighting when a face of the object with the shader is in the exact same place as another face.
        Offset -1, -1

        Cull Off
        Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_particles

            #include "UnityCG.cginc"

            uniform float _Cutoff = 0.5;

            // textures
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform sampler2D _Noise;
            uniform float4 _Noise_ST;

            // Fresnel
            uniform float _UseFresnel;
            uniform fixed4 _FresnelColor;
            uniform float _Fresnel;
            uniform float _FresnelEmission;

            // color settings
            uniform fixed4 _FrontFacesColor;
            uniform fixed4 _BackFacesColor;
            uniform float _Emission;
            uniform float _Alpha;

            // displacement
            uniform float4 _SpeedMainTexUVNoiseZW;

            // circle mask
            uniform float _Smoothness;
            uniform float _OuterRadius;
            uniform float _InnerRadius;

            struct VertexData {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varys
            {
                float4 pos : SV_POSITION;
                float4 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float2 displUV : TEXCOORD2;
                float4 posWS : TEXCOORD3;
                float3 normalWS : TEXCOORD4;
                float3 viewDirWS : TEXCOORD5;
                float2 rawUV : TEXCOORD6;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varys vert(VertexData v)
            {
                Varys o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _Noise);
                o.rawUV = v.uv.zw;
                o.displUV = float2(0., 0.);

                // world space position
                o.posWS = mul(unity_ObjectToWorld, v.vertex);
                //o.normalWS = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.viewDirWS = WorldSpaceViewDir(v.vertex);
                o.color = v.color;

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag(Varys i): SV_Target
            {
                float3 normalWS = normalize(i.normalWS);
                float3 viewDirWS = normalize(i.viewDirWS);

                // fresnel intensity
                float NdotV = dot(normalWS, viewDirWS);
                float fresnelIntesity = pow(1.0 - NdotV, _Fresnel);

                float4 staticSwitch101 = lerp(_FrontFacesColor, (( _FrontFacesColor * ( 1.0 - fresnelIntesity ) ) + ( _FresnelEmission * _FresnelColor * fresnelIntesity ) ), _UseFresnel);
                float4 backColor = lerp(staticSwitch101,_BackFacesColor, (1.0 + (sign(NdotV) +1.0) * -1./2.));

                float2 uv_Mask = i.uv.xy;
                float2 uv_Noise = i.uv.zw;
                fixed2 noiseVector = tex2D(_Noise, uv_Noise + _Time.x * _SpeedMainTexUVNoiseZW.zw);

                fixed4 finalCol = backColor * _Emission * i.color * tex2D(_MainTex, (i.uv.xy + (noiseVector * _Time.x))) * i.color.a;
                clip(noiseVector.r - (1. - i.rawUV.x));
                UNITY_APPLY_FOG(i.fogCoord, finalCol);
                return finalCol * i.color.a;
            }
            ENDCG
        }
    }
}
