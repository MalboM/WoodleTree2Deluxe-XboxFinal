Shader "WoodleTree/Unlit/Water/water_still (Opt)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FloatSpeed("Float Speed (x,y), Scale (w)", Vector) = (35.0, 35.0, 0.0, 1.5)
		_OpacityF("Opacity", Range(0.0, 1.0)) = 0.75
		
		[Header(Rim Effect)]
        [Toggle(RIM)] _RIM("Activate Rim", Float) = 0        
        _RimColor("Rimlight Color", Color) = (0.1, 0.1, 0.9, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0 
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Blend One OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
            #pragma multi_compile_instancing
			
			#define HASHSCALE3 float3(443.897, 441.423, 437.195)
			#pragma shader_feature RIM

			#include "UnityCG.cginc"

			struct VertexData
			{
                UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 objPos : POSITION;
				float2 uv : TEXCOORD0;
				float3 objNormal : NORMAL;
			};

			struct Varys
			{
				float4 pos : SV_POSITION;
				float3 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)			
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float4 _FloatSpeed;
			uniform float _OpacityF;
			
            // rim uniform
            uniform fixed4 _RimColor;
            uniform fixed _RimSharpnessF;
            uniform fixed _RimIntensityF;			

			inline float2 hash22(float2 p)
			{
				float3 p3 = frac(float3(p.xyx) * HASHSCALE3);
				p3 += dot(p3, p3.yzx + 19.19);
				return frac((p3.xx + p3.yz)*p3.zy);
			}

			Varys vert (VertexData v)
			{
				Varys o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);                 
                
				o.pos = UnityObjectToClipPos(v.objPos);
				
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.xy += sin((_Time.x + hash22(v.uv)) * _FloatSpeed.xy) * _FloatSpeed.w * 0.01;
				                
                #ifdef RIM
                o.uv.z = clamp(dot(normalize(ObjSpaceViewDir(v.objPos)), v.objNormal) * _RimSharpnessF, 0.0, 1.0);
                #else
                o.uv.z = 0;
                #endif
                
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 frag (Varys i) : SV_Target
			{
				fixed4 baseCol = tex2D(_MainTex, i.uv);
				baseCol.a = _OpacityF;
				
				#ifdef RIM
                baseCol.rgb = lerp(baseCol.rgb, _RimColor.rgb, (1. - i.uv.z)*_RimIntensityF);
                #endif

				UNITY_APPLY_FOG(i.fogCoord, baseCol);

				return baseCol;
			}
			ENDCG
		}
	}
}
