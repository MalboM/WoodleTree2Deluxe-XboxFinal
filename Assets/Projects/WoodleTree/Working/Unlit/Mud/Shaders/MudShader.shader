Shader "Polymole/Mud/MudShader"
{
    Properties
    {
        _DisplaceTex("Displace Texture",2D) = "black" {}
        _PhaseTex("Phase Texture",2D) = "black" {}
        _DisplaceAmp("Displace Amplitude", Range(0.,0.5)) = 0.05
        _PhaseRange("Phase Range", Range(0.,3.15)) = 0.05
        _Speed("Speed", Range(0.,1000.))= 1.
        _BaseColor("Base Color", Color) = (0.,0.,0.,1.)
        [Header(Rimlight Settings)]
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
        _ULowbound("U low bound", Range(0.0, 0.5))= 0.3
        _UUpbound("U up bound", Range(0.5, 1.))= 0.6
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
            //#include "Assets/Shaders/Libs/ColorMap.cginc"

            struct VertexData
            {
                float4 objPos : POSITION;
                float2 uv : TEXCOORD0;
                float3 objNormal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varys
            {
              float2 uv : TEXCOORD0;
              UNITY_FOG_COORDS(1)
              float4 pos : SV_POSITION;
              float3 worldNormal : NORMAL;
              float3 worldPos : TEXCOORD2;
              float4 debug: TEXCOORD3;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform sampler2D _DisplaceTex;
            uniform float4 _DisplaceTex_ST;
            uniform sampler2D _PhaseTex;
            uniform float4 _PhaseTex_ST;
            uniform fixed4 _BaseColor;


            uniform fixed _Speed;
            uniform float _DisplaceAmp;
            uniform float _PhaseRange;
            uniform float _UUpbound;
            uniform float _ULowbound;

            uniform fixed4 _RimColor;
            uniform float _RimSharpnessF;
            uniform float _RimIntensityF;


            #define PI 3.14159265359


            float4x4 itObjSpaceTMatrix(float3 offset){
            	float4x4 itObjSpaceTMatrix = float4x4(
            			1.0,0.0,0.0,0.0,
            			0.0,1.0,0.0,0.0,
            			0.0,0.0,1.0,0.0,
            			-offset.x,-offset.y,-offset.z,1.0
            		);
            	return itObjSpaceTMatrix;
            }


            Varys vert (VertexData v)
            {
                Varys o;

                UNITY_INITIALIZE_OUTPUT(Varys, o);
                UNITY_SETUP_INSTANCE_ID(v);

                float displaceF = tex2Dlod(_DisplaceTex,float4(v.uv,0.,0.)).x;

                float displaceS = tex2Dlod(_PhaseTex,float4(v.uv,0.,0.)).x;

                float squareDistance = dot(v.objPos.xyz,v.objPos.xyz);

                //float attenuation = max(0,(1.125- squareDistance));
                float timerX = 0.5*sin(_Speed*_Time.x)+ 0.5;
                float displace = 2*lerp(displaceF,displaceS,timerX*timerX)-1;


                if(v.uv.x>_UUpbound)
                {
                    displace = 0.;
                }

                if(v.uv.x<_ULowbound){
                    displace = 0.;
                }

                float oscFactor = displace*_DisplaceAmp;

                v.objPos.xyz +=  v.objNormal*oscFactor;

                o.pos = UnityObjectToClipPos(v.objPos);

                o.debug = float4(v.objNormal,displaceF);
                v.objNormal = mul(v.objNormal, itObjSpaceTMatrix(v.objNormal*oscFactor));
                o.worldNormal = UnityObjectToWorldNormal(v.objNormal);
                o.worldPos = mul(unity_ObjectToWorld, v.objPos).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            fixed4 frag (Varys i) : SV_Target
            {

            	// camera-fragment distance vector

                half3 worldViewVec = UnityWorldSpaceViewDir(i.worldPos.xyz);

                float3 worldViewDir = normalize(worldViewVec);

                fixed4 mainCol = tex2D(_MainTex,i.uv);

                // Rim Factor
                fixed4 diffuseCol = _BaseColor;

                float rimFactorMap = saturate(dot(worldViewDir, i.worldNormal.xyz)) * _RimSharpnessF;

                fixed3 color = lerp(diffuseCol , _RimColor.rgb, saturate((1-rimFactorMap) * _RimIntensityF));

                //return colormapGrayEx(_ULowbound,_UUpbound,i.uv.x);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, color);

                fixed4 finalRGBA = fixed4(color,1.);

                return finalRGBA;
            }
            ENDCG
        }
    }
}
