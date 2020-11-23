Shader "WoodleTree/Special/Snow_Terrain_OptimizedV1"
{
    Properties
    {
        [Header(Color Settings)]
        _TintColor ("Tint Color", Color) = (1.0, 1.0, 1.0, 1.0)

        [Header(Texture Settings)]
        [Toggle(USE_MAIN_TEX)] _UseMainTex("Activate Main Tex", Float) = 0
        [NoScaleOffset] _MainTex ("Main Texture (RGB) Sparkle (A)", 2D) = "white" {}
        _BaseTileF ("Main Texture Tiling", Range(0.05, 20)) = 1
        [HideIfDisabled(USE_MAIN_TEX)]_Mixer("Mix factor", Range(0.0,1.0)) = 0.

        [Header(Normal Settings)]
        [NoScaleOffset][Normal] _BaseNormalTex ("Base Normal Tex", 2D) = "gray" {}
        _BaseNormalTileF ("Base Normal Tiling", Range(0.05, 20)) = 1
        _AttenuationF ("Overall Normal Strength", Range(0.0, 5.0)) = 0.5

        [Header(Detail Normal Settings)]
        [Toggle(DETAIL_NORMAL)] _DetailNormal("Activate Detail Normal", Float) = 0
        [HideIfDisabled(DETAIL_NORMAL)] [Header(Detail Settings)]
        [NoScaleOffset][Normal] _DetailNormalTex("Detail Normal Tex", 2D) = "gray" {}
        [HideIfDisabled(DETAIL_NORMAL)] _DetailTileF("Detail Normal Tiling", Range(0.05, 10)) = 1
        [HideIfDisabled(DETAIL_NORMAL)] _AttenuationBaseNormalF("Base Normal Strenght", Range(0.,5.)) = 0.5
        [HideIfDisabled(DETAIL_NORMAL)] _AttenuationDetailNormalF("Detail Normal strength", Range(0.,5.)) = 0.5

        [Header(Rimlight Settings)]
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0

        [Header(Sparkle Settings)]
        [KeywordEnum(No_Sparkle,Old_Texture, Procedural, New_Texture)] _Sparkle_Mode("Sparkle mode", Float) = 0.0
        _SparkleF("Sparkle intensity", Range(0.0, 10.0)) = 0.5
        _SparkleFallof("Sparkle Fall Off", Range(0.0, 1.0)) = 0.05
        _SparkleColor("Sparkle Color", Color) = (1.,1.,1.,1.)

        [Header(Procedural Sparkle Settings)]
        _SparkleSharpness("Sparke Sharpness", Range(0.1, 10.)) = 2.5
        _AnimSpeed ("Animation Speed", Range(0.0, 5.0)) = 1.0
        _NoiseScale ("Noise Scale", Range(0.0, 500.0)) = 5.0

        [Header(Texture Noise Sparkle Settings)]
        [NoscaleOffset] _SpeckleTex("Speckle Texture", 2D) = "white" {}
        [NoScaleOffset] _ClipTex("Clip texture", 2D) = "white" {}
        _SpeckleSize("Sparkle Size Amp", Range(-1,5)) = 0.
        _AnimSpeedTexture ("Texture Noise Animation Speed", Range(0.0, 1.0)) = 0.1
        _SpeckleSharpness("Speckle Sharpness", Range(0.5, 5.)) = 2.5

        [Header(Ambient Light Settings)]
        [KeywordEnum(Ground, Equator, Sky, Skybox, None)] _Position("Ambient Light Source", Float) = 0.0

        [Header(Fake Light Settings)]
        _FakeLightPosition("Fake Light Position", Vector) = (0.0, 1.0, 0.0, 1.0)
        _FakeLightContribution("Fake Light Contribution", Range(0,1)) = 0.6
        _ColorBrightness("Color Brightness", Range(0,1)) = 0.7
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
            #pragma multi_compile _POSITION_NONE _POSITION_GROUND _POSITION_EQUATOR _POSITION_SKY _POSITION_SKYBOX
            #pragma shader_feature DETAIL_NORMAL
            #pragma shader_feature USE_MAIN_TEX
            #pragma multi_compile _SPARKLE_MODE_NO_SPARKLE _SPARKLE_MODE_OLD_TEXTURE _SPARKLE_MODE_PROCEDURAL _SPARKLE_MODE_NEW_TEXTURE
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #include "SimplexNoise3D.cginc"


            #define EPSILON 0.0001
            #define PI 3.14159265359

            struct VertexData
            {
                float4 objPos : POSITION;
                float2 uv : TEXCOORD0;
                float3 objNormal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varys
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD0;
                float3 triblend : TEXCOORD2;
                float3 worldCrossL : TEXCOORD3;
                float4 projPos: TEXCOORD4;
                UNITY_FOG_COORDS(1)
            };

            uniform sampler2D _MainTex;  //alpha channel used for sparkle texture
            uniform sampler2D _BaseNormalTex;
            uniform sampler2D _DetailNormalTex;

            // new texture sparkle
            uniform sampler2D _SpeckleTex;
            uniform float4 _SpeckleTex_ST;
            uniform sampler2D _ClipTex;
            uniform float4 _ClipTex_ST;

            uniform fixed4 _TintColor;
            uniform float _Mixer;
            uniform fixed4 _RimColor;
            uniform fixed4 _SparkleColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;

            // tiling
            uniform float _BaseTileF;
            uniform float _DetailTileF;
            uniform float _BaseNormalTileF;

            uniform float _AttenuationF;
            uniform float _AttenuationBaseNormalF;
            uniform float _AttenuationDetailNormalF;


            // sparkles
            uniform float _SparkleF;
            uniform float _SparkleFallof;
            uniform float _SparkleSharpness;

            // texture noise sparkles uniforms
            uniform float _SpeckleSharpness;
            uniform float _SpeckleSize;
            uniform float _AnimSpeedTexture;

            // generative noise sparkles uniforms
            uniform float _AnimSpeed;
            uniform float _NoiseScale;

            // fake light
            uniform float4 _FakeLightPosition;
            uniform float _FakeLightContribution;
            uniform float _ColorBrightness;

            //taken from http://blog.selfshadow.com/publications/blending-in-detail/,
            //reorient detail normal map to follow the base normal map direction

            inline float3 ReorientNormalMap(float3 baseNormal, float3 detailNormal)
            {
                float3 t = baseNormal * float3(2, 2, 2) + float3(-1, -1, 0);
                float3 u = detailNormal * float3(-2, -2, 2) + float3(1, 1, -1);
                return normalize(t * dot(t, u) - u * t.z);
            }

            inline float3 UDNBlend(float3 baseNormal, float3 detailNormal){
                return normalize(float3(baseNormal.xy + detailNormal.xy, baseNormal.z*detailNormal.z));
            }


            Varys vert(VertexData v)
            {
                Varys o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);

                // World Vertex Position
                o.worldPos = mul(unity_ObjectToWorld, v.objPos).xyz;

                // object pos and clip space pos
                o.pos = mul(unity_MatrixVP, float4(o.worldPos, 1.));

                // calculate camera world space right dir
                float3 crossLight = normalize(mul(float4(1,0,0,0), UNITY_MATRIX_IT_MV));
                o.worldCrossL = crossLight;

                // world normal
                float3 worldNormal = UnityObjectToWorldNormal(v.objNormal);

                // triplanar normal blending weights
                float3 triblend = saturate(pow(worldNormal, 4));
                triblend /= max(dot(triblend, float3(1.0, 1.0, 1.0)), EPSILON);

                // tangent space basis
                o.worldNormal = worldNormal;
                o.triblend = triblend;

                o.projPos = ComputeScreenPos (o.pos);
                o.projPos.z = -UnityObjectToViewPos(v.objPos).z;

                UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }

            fixed4 frag(Varys i) : SV_Target
            {

                // triplanar world uv positions
                float2 uvX = i.worldPos.yz;
                float2 uvY = i.worldPos.xz;
                float2 uvZ = i.worldPos.xy;

            // 	offset UVs to prevent obvious mirroring
           	/*	#if defined(TRIPLANAR_UV_OFFSET)
                	uvY += 0.33;
                	uvZ += 0.67;
        		#endif
            */

                half3 axisSign = i.worldNormal < 0 ? -1 : 1;
                uvX.x *= axisSign.x;
                uvY.x *= axisSign.y;
                uvZ.x *= axisSign.z;

                // albedo textures
                fixed4 colX = tex2D(_MainTex, uvX * _BaseTileF);
                fixed4 colY = tex2D(_MainTex, uvY * _BaseTileF);
                fixed4 colZ = tex2D(_MainTex, uvZ * _BaseTileF);
                fixed4 baseCol = colX * i.triblend.x + colY * i.triblend.y + colZ * i.triblend.z;

                // tangent space normal maps
                half4 normalColX = tex2D(_BaseNormalTex, uvX * _BaseNormalTileF);
                half4 normalColY = tex2D(_BaseNormalTex, uvY * _BaseNormalTileF);
                half4 normalColZ = tex2D(_BaseNormalTex, uvZ * _BaseNormalTileF);

                // unpacking normal [0,1] => [-1, 1]
                half3 tnormalX = UnpackNormal(normalColX);
                half3 tnormalY = UnpackNormal(normalColY);
                half3 tnormalZ = UnpackNormal(normalColZ);


            #ifdef DETAIL_NORMAL

                tnormalX = half3(_AttenuationBaseNormalF*tnormalX.xy, tnormalX.z);
                tnormalY = half3(_AttenuationBaseNormalF*tnormalY.xy, tnormalY.z);
                tnormalZ = half3(_AttenuationBaseNormalF*tnormalZ.xy, tnormalZ.z);

                half4 detailColX = tex2D(_DetailNormalTex, uvX * _DetailTileF);
                half4 detailColY = tex2D(_DetailNormalTex, uvY * _DetailTileF);
                half4 detailColZ = tex2D(_DetailNormalTex, uvZ * _DetailTileF);

                half3 dnormalX = UnpackNormal(detailColX);
                half3 dnormalY = UnpackNormal(detailColY);
                half3 dnormalZ = UnpackNormal(detailColZ);

                dnormalX = half3(_AttenuationDetailNormalF*dnormalX.xy, dnormalX.z);
                dnormalY = half3(_AttenuationDetailNormalF*dnormalY.xy, dnormalY.z);
                dnormalZ = half3(_AttenuationDetailNormalF*dnormalZ.xy, dnormalZ.z);

                tnormalX = UDNBlend(tnormalX,dnormalX);
                tnormalY = UDNBlend(tnormalY,dnormalY);
                tnormalZ = UDNBlend(tnormalZ,dnormalZ);

                // swizzle world normals to match tangent space and apply ala UDN normal blending
                // these should get normalized, but it's very a minor visual difference to skip it

                tnormalX = half3(_AttenuationF* tnormalX.xy + i.worldNormal.zy, i.worldNormal.x);
                tnormalY = half3(_AttenuationF* tnormalY.xy + i.worldNormal.xz, i.worldNormal.y);
                tnormalZ = half3(_AttenuationF* tnormalZ.xy + i.worldNormal.xy, i.worldNormal.z);

            #else

                // swizzle world normals to match tangent space and apply ala UDN normal blending
                // these should get normalized, but it's very a minor visual difference to skip it
                tnormalX = half3(_AttenuationF* tnormalX.xy + i.worldNormal.zy, i.worldNormal.x);
                tnormalY = half3(_AttenuationF* tnormalY.xy + i.worldNormal.xz, i.worldNormal.y);
                tnormalZ = half3(_AttenuationF* tnormalZ.xy + i.worldNormal.xy, i.worldNormal.z);

            #endif

                // swizzle tangent normals to match world normal and blend together
                half3 worldNormal = normalize(
                    tnormalX.zyx * i.triblend.x +
                    tnormalY.xzy * i.triblend.y +
                    tnormalZ.xyz * i.triblend.z
                );


                // base color from BaseCol Texture
            #ifdef USE_MAIN_TEX
                fixed3 diffuseCol = lerp(baseCol ,_TintColor.rgb, _Mixer);
            #else
                fixed3 diffuseCol = _TintColor.rgb; // interesting 6.0
            #endif

                // camera-fragment distance vector
                half3 worldViewVec = UnityWorldSpaceViewDir(i.worldPos.xyz);
                float3 worldViewDir = normalize(worldViewVec);

                // Rim Factor
                float rimFactorMap = saturate(dot(worldViewDir, worldNormal.xyz)) * _RimSharpnessF;
                fixed3 color = lerp(diffuseCol , _RimColor.rgb, saturate((1-rimFactorMap) * _RimIntensityF));
                float diffuseInt = max(0,dot(worldNormal.xyz, i.worldCrossL.xyz));
                color = color*_ColorBrightness + diffuseInt*_FakeLightContribution*color;


            #ifdef _POSITION_GROUND
                color += unity_AmbientGround;

            #elif _POSITION_EQUATOR
                color += unity_AmbientEquator;

            #elif _POSITION_SKY
                color += unity_AmbientSky;

            #elif _POSITION_SKYBOX
                color += UNITY_LIGHTMODEL_AMBIENT;

            #endif

            #ifdef _SPARKLE_MODE_NO_SPARKLE


            #elif _SPARKLE_MODE_OLD_TEXTURE

                half sparkle = baseCol.a;
                sparkle = lerp(sparkle, 0.0, saturate(dot(worldViewVec, worldViewVec) * (1.0 - _SparkleFallof)));
                color += sparkle*_SparkleF*_SparkleColor;

            #elif _SPARKLE_MODE_PROCEDURAL

                float firstNoise = snoise((i.worldPos * _NoiseScale) + worldViewDir * _SparkleF - (_Time.x * _AnimSpeed)) * 0.5 + 0.5;
                float secondNoise = snoise((i.worldPos * _NoiseScale) + _Time.x * _AnimSpeed) * 0.5 + 0.5;
                float fp = _SparkleF*pow(saturate(firstNoise) * saturate(secondNoise), _SparkleSharpness);
                fp = smoothstep(0.5,0.5+_SparkleFallof,fp);
                color += lerp(_SparkleColor.rgb*fp, 0.0, saturate(dot(worldViewVec, worldViewVec) * (1.0 - _SparkleFallof)));

            #elif _SPARKLE_MODE_NEW_TEXTURE

                // real target texture [w/h] coordinates to ([0,1], [0.1]) normalized device (screen?) coordinates and then to the range ([-1,1], [-1,1])
                float2 ndcUVs = (i.projPos.xy / i.projPos.z) * 2 - 1;

                // fix aspect ratio: screencoords to ([-ar,ar], [0,1])
                float2 screenCoord = float2(ndcUVs.x*(_ScreenParams.r/_ScreenParams.g), ndcUVs.y);

                // uv for screen space textures
                float2 speckleUVs = TRANSFORM_TEX(screenCoord, _SpeckleTex);
                // screen space tex col

                float expSize = exp(_SpeckleSize*log(2));

                float4 speckleCol = tex2D(_SpeckleTex,speckleUVs/expSize  + _Time.x*_AnimSpeedTexture);

                // triplanar for world mapped sparkle texture
                fixed spX = tex2D(_ClipTex, uvX/expSize).g;
                fixed spY = tex2D(_ClipTex, uvY/expSize).g;
                fixed spZ = tex2D(_ClipTex, uvZ/expSize).g;
                fixed clipCol = spX * i.triblend.x + spY * i.triblend.y + spZ * i.triblend.z;

                float emissive = lerp(0.0, speckleCol*_SparkleF, pow(1-saturate(clipCol),_SpeckleSharpness));

                //color = lerp(color, _SparkleColor.rgb, emissive/pow(dot(worldViewVec,worldViewVec), _SparkleFallof + EPSILON));
                color += lerp(_SparkleColor.rgb*emissive, 0.0, saturate(dot(worldViewVec, worldViewVec) * (1.0 - _SparkleFallof)));
            #else
                color = fixed3(0,0,1);
            #endif

                fixed4 finalRGBA = fixed4(color,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
}
