Shader "WoodleTree/Mud/MudShader (Vertex Col RGB)"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.,0.,0.,1.)
        [Toggle(DEBUG)] _DEBUG("Debug", Float) = 0

        [Header(Displace A Settings (Red channel))]
        _DisplaceAmpA("Amplitude A", Range(0.,0.5)) = 0.105
        _SmoothnessA("Smoothness A", Range(0.,3.15)) = 1.14
        _SpeedA("Speed A", Range(0.,150))= 87.6
        [Header(Displace B Settings (Green channel))]
        _DisplaceAmpB("Amplitude B", Range(0.,0.5)) = 0.164
        _SmoothnessB("Smoothness B", Range(0.,3.15)) = 2.03
        _SpeedB("Speed B", Range(0.,150))= 74.5
        [Header(Displace C Settings (Blue channel))]
        _DisplaceAmpC("Amplitude C", Range(0.,0.5)) = 0.111
        _SmoothnessC("Smmothness C", Range(0.,3.15)) = 1.64
        _SpeedC("Speed C", Range(0.,150))= 61

        [Header(Rimlight Settings)]
        _RimColor("Rimlight Color", Color) = (0.3, 0.43, 0.51, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.65
        _RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.53
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
            #pragma shader_feature DEBUG

            #include "UnityCG.cginc"

            struct VertexData
            {
                float4 objPos : POSITION;
                float2 uv : TEXCOORD0;
                float3 objNormal : NORMAL;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varys
            {
              UNITY_FOG_COORDS(1)
              float4 pos : SV_POSITION;
              float4 debug: TEXCOORD2;
              float3 color : COLOR;
            };

            uniform fixed4 _BaseColor;
            uniform float _DisplaceAmpA;
            uniform float _DisplaceAmpB;
            uniform float _DisplaceAmpC;

            uniform float _SmoothnessA;
            uniform float _SmoothnessB;
            uniform float _SmoothnessC;

            uniform fixed _SpeedA;
            uniform fixed _SpeedB;
            uniform fixed _SpeedC;


            uniform fixed4 _RimColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;


            #define PI 3.14159265359
            #define PI_3 1.047197551196
            #define PI_5 0.62831853071


            Varys vert (VertexData v)
            {
                Varys o;

                UNITY_INITIALIZE_OUTPUT(Varys, o);
                UNITY_SETUP_INSTANCE_ID(v);

                float3 timeVector =float3(
                	pow(sin(_SpeedA * _Time.x),2),
                	pow(sin(_SpeedB * _Time.x + PI_3),2), 
                	pow(sin(_SpeedC * _Time.x + PI_5),2));


                float timerA = smoothstep(0,_SmoothnessA, timeVector.x);
                float timerB = smoothstep(0,_SmoothnessB, timeVector.y);
                float timerC = smoothstep(0,_SmoothnessC, timeVector.z);

                float displaceA = 2*lerp(0.5, 0.5 + v.color.r*0.5, timerA) - 1;
                float displaceB = 2*lerp(0.5, 0.5 + v.color.g*0.5, timerB) - 1;
                float displaceC = 2*lerp(0.5, 0.5 + v.color.b*0.5, timerC) - 1;

                float displace = max(max(abs(displaceA*_DisplaceAmpA), abs(displaceB*_DisplaceAmpB)), abs(displaceC*_DisplaceAmpC));

                v.objPos.xyz +=  v.objNormal*displace;

                o.pos = UnityObjectToClipPos(v.objPos);

                o.debug = float4(v.color.rgb, 1.);

                float3 worldPos = mul(unity_ObjectToWorld, v.objPos).xyz;
                float3 worldNormal = UnityObjectToWorldNormal(v.objNormal);
                half3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos.xyz));

                float rimF = saturate(dot(worldViewDir, worldNormal)) * _RimSharpnessF;
                o.color = lerp(_BaseColor.rgb, _RimColor.rgb, saturate((1-rimF) * _RimIntensityF));

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {
                fixed3 color = i.color;

            #if DEBUG
                color = float3(i.debug.r,0,0); 
                color = i.debug.rgb;
            #endif
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);
                fixed4 finalRGBA = fixed4(color, _BaseColor.a);

                return finalRGBA;
            }
            ENDCG
        }
    }
}
