Shader "WoodleTree/Special/Opaque_Water"
{
	Properties
	{
		[Header(Water Settings)]
		//[NoScaleOffset]
		_MainTex ("Texture", 2D) = "white" {}
		//_TileF("Tiling", Range(0.05, 20)) = 1
		_PanSpeed("Pan Speed (x,y)", Vector) = (0.0, 1.0, 0.0, 0.0)
		//_FloatSpeed("Float Speed (x,y), Scale (w)", Vector) = (35.0, 35.0, 0.0, 1.5)
		[Header(Rimlight Settings)]
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
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
				float4 position : SV_POSITION;
				float4 vertex : TANGENT;
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;

				UNITY_FOG_COORDS(1)			
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _FloatSpeed;
			float _TileF;
			float4 _PanSpeed;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			/*
			inline float2 hash22(float2 p)
			{
				float3 p3 = frac(float3(p.xyx) * HASHSCALE3);
				p3 += dot(p3, p3.yzx + 19.19);
				return frac((p3.xx + p3.yz)*p3.zy);
			}
			*/
			v2f vert (appdata v)
			{
				v2f o;

				o.position = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv += _Time.xx * _PanSpeed.xy;

				//o.uv = v.uv * _TileF; // TRANSFORM_TEX(v.uv, _MainTex);
				//o.uv += sin((_Time.x + hash22(v.vertex.xy)) * _FloatSpeed.xy) * _FloatSpeed.w * 0.01;

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				o.vertex = v.vertex;

				UNITY_TRANSFER_FOG(o,o.position);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 diffuse_color = tex2D(_MainTex, i.uv);
				float rimScale = saturate(dot(normalize(WorldSpaceViewDir(i.vertex)), i.worldNormal) *_RimSharpnessF);
				fixed4 col = lerp(diffuse_color, lerp(_RimColor, diffuse_color, rimScale), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
