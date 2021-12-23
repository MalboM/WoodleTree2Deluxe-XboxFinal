Shader "WoodleTree/Tree/ShakeShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        [Header(Sliding Settings)]
        _SlFreq("Slides Frequency", Range(0.1,10)) = 2.5
        _HeightDiffusion("Height Diffusion Amp", Range(0.05,0.95)) = 0.5
        _DampSlFactor("Damping Sliding Factor", Range(0.1,3)) = 2
        _OscSlAmp("Oscillation Sliding Amp", Range(0,1)) = 0.05

        [Header(Rotation Settings)]
        _RotationSpeed("Rotation Speed", Range(0,10))= 1
        _OscRotAmp("Oscillation rotation Amp", Range(0,3.14))= 1
        _DampRotFactor("Damping Rot Factor", Range(0.1,3)) = 2


        [Header(Rim Properties)]
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0

        [Header(Animator)]
        _TimeAnimParam("Time anim", Range(0,1))=0
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
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

			#define PI 3.14159265359
            #define MAX_DAMP 3.1

            struct VertexData
            {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 objPos : POSITION;
                float3 objNormal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varys
            {
                float3 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;

            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;

            uniform float _SlFreq;
            uniform float _DampSlFactor;
            uniform float _OscSlAmp;
            uniform float _HeightDiffusion;

            uniform float _DampRotFactor;
            uniform float _OscRotAmp;
            uniform float _RotationSpeed;

            uniform float _TimeAnimParam;

            uniform fixed4 _RimColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;


            Varys vert (VertexData v)
            {
                Varys o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);

                float height = pow(0.5*v.objPos.y + 0.5 ,2.*(1-_HeightDiffusion));
                float theta = _OscRotAmp*height*lerp(1,0,pow(_TimeAnimParam, MAX_DAMP - _DampRotFactor));
                fixed timePeriod = lerp(-theta,theta,0.5*sin(2*PI*_RotationSpeed* _TimeAnimParam) + 0.5);


                float sint, cost;

                sincos(timePeriod, sint, cost);

                float currAmp = _OscSlAmp*lerp(1,0,pow(_TimeAnimParam, MAX_DAMP - _DampSlFactor))*height;
                float oscFac = currAmp*sin(2*PI*_SlFreq*_TimeAnimParam);

                float4x4 slrot = float4x4(
                	cost, sint , 0, oscFac,
                	-sint,cost, 0, 0,
                	0 , 0, 1, oscFac,
                	0 , 0, 0, 1
                );

                v.objPos = mul(slrot,v.objPos);
                v.objNormal = mul((float3x3)slrot,v.objNormal);


                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

                float3 worldPos = mul(unity_ObjectToWorld, v.objPos).xyz;

                float3 worldNormal = UnityObjectToWorldNormal(v.objNormal);

                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));


                float rimF = saturate(dot(worldViewDir, worldNormal)) * _RimSharpnessF;
                o.uv.z = rimF;

                o.pos = UnityObjectToClipPos(v.objPos);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                // sample the texture
                fixed4 baseColor = tex2D(_MainTex, i.uv.xy);
                float3 color = lerp(baseColor.rgb, _RimColor.rgb, saturate((1-i.uv.z) * _RimIntensityF));
                // apply fog
                fixed4 finalRGBA = fixed4(color,1);

                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
	FallBack "Diffuse"
}
