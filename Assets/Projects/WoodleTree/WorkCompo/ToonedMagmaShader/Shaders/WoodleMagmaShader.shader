Shader "WoodleTree/LiquidShader/MagmaShader"
{
    Properties
    {

        _LeftColor("Left Color", Color) = (1.,1.,1.,1.)
        _RightColor("Right Color", Color) = (1.,1.,1.,1.)


        [Header(Texture Settings)]
            [NoScaleOffset] _BubbleTex ("Bubble Tex", 2D) = "white" {}
            _GlobalTexTileF ("Global Tile", Range(0.05, 20)) = 1


        [Header(Normal Settings)]
            [Toggle(NORMAL_ENABLE)] _NormalEnable("Enable Normal map", Range(0.,1.)) = 0.
            [HideIfDisabled(NORMAL_ENABLE)][NoScaleOffset][Normal] _BaseNormalTex ("Base Normal Tex", 2D) = "gray" {}
            [HideIfDisabled(NORMAL_ENABLE)]_AttenuationF ("Overall Normal Strength", Range(0.0, 5.0)) = 0.5

        [Header(Magma Direction)]
            _Direction("Direction", Vector) = (1,1,1,1)

        [Header(Distortion)]
           _UDirection("U distortion direction amount",Range(0,2)) = 0.5
           _VDirection("V distortion direction amount",Range(0,2)) = 0.5
           _NoiseUV("Noise UV",Range(-4.,4.)) = 0.
           _Velocity("Velocity", Range(-50.0,50.0)) = 0.7
           _SinFreqA("Sin freqs A", Range(0.01,10.)) = 3
           _SinFreqB("Sin freqs B", Range(0.01,10.)) = 1.5
           _Quantization("Noise Mix details", Range(1.,10.)) = 3.

        [Header(Rimlight Settings)]
            _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
            _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
            _RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
            _RimContribution("Rim Contrib",Range(0.,1.)) = 0.4
            _RimPow("Rim Shiness", Range(0.25,3)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma shader_feature NORMAL_ENABLE
            #include "UnityCG.cginc"
            #include "./SimplexNoise3D.cginc"

            struct VertexData
            {
                float4 objVertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 objNormal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varys
            {
                float4 pos : SV_POSITION;
                float4 objPos : TEXCOORD0;
                float4 worldPos : TEXCOORD2;
                float3 worldNormal : NORMAL;
                float2 uv : TEXCOORD4;
                float3 triblend : TEXCOORD5;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            uniform sampler2D _BaseNormalTex;
            uniform sampler2D _BubbleTex;

            // tiling

            uniform float _BaseNormalTileF;

            uniform float _AttenuationF;
            uniform float _AttenuationBaseNormalF;
            uniform float _GlobalTexTileF;

            uniform fixed3 _Direction;
            uniform float _UDirection;
            uniform float _VDirection;
            uniform float _Velocity;
            uniform float _NoiseUV;
            uniform float _SinFreqA;
            uniform float _SinFreqB;
            uniform float _Quantization;
            uniform float _NormalEnable;
            uniform float _RimContribution;


            uniform fixed4 _LeftColor;
            uniform fixed4 _RightColor;
            uniform fixed4 _RimColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;
            uniform float _RimPow;


            #define EPSILON 0.0001
            #define PI 3.14159265359


            inline float CalcMixFac(float2 noisedUv, float ut)
            {
                float mix = 0;

                mix =  0.3*sin(_SinFreqA*(noisedUv.x+ 0.1*noisedUv.y) + ut*0.5) + 0.15;
                mix += 0.3*sin(_SinFreqB*noisedUv.y + ut*2.) + 0.15;
                mix += 0.3*tex2D(_BubbleTex,noisedUv).x + 0.15;
                mix = saturate(floor(mix*_Quantization) / _Quantization);
                return mix;
            }


            Varys vert(VertexData v)
            {
                Varys o;

                UNITY_INITIALIZE_OUTPUT(Varys, o);
                UNITY_SETUP_INSTANCE_ID(v);

                o.pos = UnityObjectToClipPos(v.objVertex);
                o.objPos = v.objVertex;
                o.worldPos = mul(unity_ObjectToWorld, v.objVertex);
                float3 worldNormal = UnityObjectToWorldNormal(v.objNormal);
                o.worldNormal = worldNormal;
                o.triblend = pow(worldNormal, 4);
                o.triblend/= dot(1,o.triblend);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o, o.pos);

                return o;
            }


            fixed4 frag(Varys i) : SV_Target
            {


                float ut = (2*PI*_Time.x)/3;

                float3 shiftedWorldPos = i.worldPos + _Velocity*normalize(_Direction)*_Time.x;

                float2 gaussianNoise = gaussian(shiftedWorldPos) + float2( _UDirection , _VDirection);
                float2 uvX = shiftedWorldPos.xz  + (1+_NoiseUV)*gaussianNoise;
                float2 uvY = shiftedWorldPos.yz  + (1+_NoiseUV)*gaussianNoise;
                float2 uvZ = shiftedWorldPos.xy  + (1+_NoiseUV)*gaussianNoise;


                half3 axisSign = i.worldNormal < 0 ? -1 : 1;

                uvX.x *= axisSign.x;
                uvY.x *= axisSign.y;
                uvZ.x *= axisSign.z;

                float2 utShift = float2(ut*0.02, ut*0.04);
                float2 uvScaleFac = 0.3*float2(3.3, 1.);
                float2 noiseUvX = (uvX*uvScaleFac + utShift)*_GlobalTexTileF;
                float2 noiseUvY = (uvY*uvScaleFac + utShift)*_GlobalTexTileF;
                float2 noiseUvZ = (uvZ*uvScaleFac + utShift)*_GlobalTexTileF;

                // tangent space normal maps


            #ifdef NORMAL_ENABLE

                half4 normalColX = tex2D(_BaseNormalTex, noiseUvX);
                half4 normalColY = tex2D(_BaseNormalTex, noiseUvY);
                half4 normalColZ = tex2D(_BaseNormalTex, noiseUvZ);

                // unpacking normal [0,1] => [-1, 1]
                half3 tnormalX = UnpackNormal(normalColX);
                half3 tnormalY = UnpackNormal(normalColY);
                half3 tnormalZ = UnpackNormal(normalColZ);

                // to applly normal strengthing correctly

                tnormalX = half3(_AttenuationF* tnormalX.xy + i.worldNormal.zy, i.worldNormal.x);
                tnormalY = half3(_AttenuationF* tnormalY.xy + i.worldNormal.xz, i.worldNormal.y);
                tnormalZ = half3(_AttenuationF* tnormalZ.xy + i.worldNormal.xy, i.worldNormal.z);

                // swizzle tangent normals to match world normal and blend together
                half3 worldNormal = normalize(
                    tnormalX.zyx * i.triblend.x +
                    tnormalY.xzy * i.triblend.y +
                    tnormalZ.xyz * i.triblend.z
                );

            #else

                half3 worldNormal = i.worldNormal;

            #endif

                half3 worldViewDir = UnityWorldSpaceViewDir(i.worldPos.xyz);
                float rimContribution = _RimContribution*saturate(_RimSharpnessF*pow(dot(worldNormal,worldViewDir),_RimPow));
                half3 factors = half3(CalcMixFac(noiseUvX,ut),CalcMixFac(noiseUvY,ut),CalcMixFac(noiseUvZ,ut));
                float mixFactor = dot(factors,i.triblend);

                fixed4 diffuseColor = lerp(_LeftColor,_RightColor,mixFactor);
                fixed4 rimColor = _RimColor*_RimIntensityF;
                fixed3 color = lerp(diffuseColor, rimColor, rimContribution);

                fixed4 finalRGBA = fixed4(color.rgb,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }

            ENDCG
        }
    }
}
