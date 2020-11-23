Shader "WoodleTree/Lit/Water/water_moving"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TileF("Tiling", Range(0.05, 20)) = 1
		_PanSpeed("Pan Speed (x,y,z)", Vector) = (0.0, 1.0, 0.0, 0.0)
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
			float4 _PanSpeed;
			float _TileF;
			float _OpacityF;
			float _SpecularF;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				//o.uv = mul(unity_ObjectToWorld, v.vertex).xyz * _TileF;//v.vertex.xyz * _TileF; // mul(unity_ObjectToWorld, v.vertex).xyz * _TileF;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv += _Time.x * _PanSpeed.xy; //xyz

				o.direction = float4(abs(v.normal),
									 max(dot(reflect(normalize(ObjSpaceLightDir(v.vertex)), v.normal), normalize(ObjSpaceViewDir(v.vertex))) * _SpecularF, 0.0));
				o.direction.xyz /= dot(o.direction.xyz, 1.0);

				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				/*fixed4 baseCol = tex2D(_MainTex, i.uv.zy) * i.direction.x +
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
