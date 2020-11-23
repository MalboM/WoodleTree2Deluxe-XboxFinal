Shader "WoodleTree/Lit/Water/water_still"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TileF("Tiling", Range(0.05, 20)) = 1
		_FloatSpeed("Float Speed (x,y), Scale (w)", Vector) = (35.0, 35.0, 0.0, 1.5)
		_OpacityF("Opacity", Range(0.0, 1.0)) = 0.75
		_SpecularF("Specular Power", Range(0.0, 3.0)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Blend One OneMinusSrcAlpha //SrcAlpha One
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
			
			#define HASHSCALE3 float3(443.897, 441.423, 437.195)

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				//float3 uv : TEXCOORD0;
				float2 uv : TEXCOORD0;
				float4 direction : NORMAL;
				UNITY_FOG_COORDS(1)			
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _FloatSpeed;
			float _TileF;
			float _OpacityF;
			float _SpecularF;
			
			/*
			inline float3 hash33(float3 p3)
			{
				p3 = frac(p3 * HASHSCALE3);
				p3 += dot(p3, p3.yxz + 19.19);
				return frac((p3.xxy + p3.yxx)*p3.zyx);
			}
			*/
			inline float2 hash22(float2 p)
			{
				float3 p3 = frac(float3(p.xyx) * HASHSCALE3);
				p3 += dot(p3, p3.yzx + 19.19);
				return frac((p3.xx + p3.yz)*p3.zy);

			}

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				//o.uv = mul(unity_ObjectToWorld, v.vertex).xyz * _TileF;//v.vertex.xyz * _TileF; // mul(unity_ObjectToWorld, v.vertex).xyz * _TileF;
				//o.uv += sin((_Time.x + hash33(v.vertex.xyz)) * _FloatSpeed.xyz) * _FloatSpeed.w * 0.01;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv += sin((_Time.x + hash22(v.vertex.xy)) * _FloatSpeed.xy) * _FloatSpeed.w * 0.01;

				o.direction = float4(abs(v.normal),
									 max(dot(reflect(normalize(ObjSpaceLightDir(v.vertex)), v.normal), normalize(ObjSpaceViewDir(v.vertex))) * _SpecularF, 0.0));
				o.direction.xyz /= dot(o.direction.xyz, 1.0);

				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				

				/*fixed4 baseCol = tex2D(_MainTex, i.uv.yz) * i.direction.x +
								 tex2D(_MainTex, i.uv.xz) * i.direction.y +
								 tex2D(_MainTex, i.uv.xy) * i.direction.z;*/
				fixed4 baseCol = tex2D(_MainTex, i.uv);
				baseCol.rgb += i.direction.w;
				baseCol.a = _OpacityF;

				UNITY_APPLY_FOG(i.fogCoord, baseCol);

				return baseCol;
			}
			ENDCG
		}
	}
}
