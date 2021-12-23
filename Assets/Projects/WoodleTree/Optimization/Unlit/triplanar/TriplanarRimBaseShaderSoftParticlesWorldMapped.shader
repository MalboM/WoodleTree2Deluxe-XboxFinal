

Shader "WoodleTree/Unlit/Triplanar/TriplanarRimBaseSoftParticlesShaderWorldMapped"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "gray" {}
        [Header(Rimlight Settings)]
        _RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
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
            
            

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
            };

            struct v2f
            {
				float4 objPosition : TEXCOORD0;
                float3 objNormal: NORMAL;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _RimSharpnessF;
            float _RimIntensityF;
            float _AttenuationF;
            float4 _MainColor;
            float4 _RimColor;

            v2f vert (appdata v)
            {
                v2f o;

              UNITY_INITIALIZE_OUTPUT(v2f, o);
              UNITY_SETUP_INSTANCE_ID(v);
  
              // calculate rim factor
              float rimFac = clamp(dot(v.normal,normalize(ObjSpaceViewDir(v.vertex)))*_RimSharpnessF,0.,1.);

              // apply world matrix and re mapping vertex coordinates to fit with uv coordinates

              o.objPosition = float4(mul(unity_ObjectToWorld,v.vertex).xyz,rimFac);

              o.vertex = UnityObjectToClipPos(v.vertex);
              o.objNormal = mul(unity_ObjectToWorld,v.normal).xyz;
              UNITY_TRANSFER_FOG(o,o.vertex);
              return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {

                // sample the texture

                half3 powNormal = abs(i.objNormal);

                half3 blendWeights = powNormal/dot(1., powNormal);

                fixed4 xaxis = tex2D(_MainTex, i.objPosition.yz * _MainTex_ST.xy + _MainTex_ST.zw);
                fixed4 yaxis = tex2D(_MainTex, i.objPosition.xz * _MainTex_ST.xy + _MainTex_ST.zw);
                fixed4 zaxis = tex2D(_MainTex, i.objPosition.xy * _MainTex_ST.xy + _MainTex_ST.zw);

                fixed4 mainTexture = xaxis* blendWeights.x +
                                yaxis* blendWeights.y +
                                zaxis* blendWeights.z;

                // interpolation factor for rim effect

                float rim = (1. - i.objPosition.w)*_RimIntensityF;

                fixed4 outColor =  lerp(mainTexture, _RimColor, rim);

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, outColor);
                return outColor;
            }
            ENDCG
        }

        

        // Pass to render object as a shadow caster
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f {
                V2F_SHADOW_CASTER;
            };

            v2f vert( appdata_base v )
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                return o;
            }

            float4 frag( v2f i ) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        
        
    }
	FallBack "Diffuse"
}
/*

*/
