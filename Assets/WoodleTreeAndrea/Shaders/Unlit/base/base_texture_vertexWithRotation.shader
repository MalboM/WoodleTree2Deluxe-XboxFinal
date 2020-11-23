
Shader "WoodleTree/Unlit/Base/base_texture_vertexWithRotation"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
		
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0
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
				float3 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _TintColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = float3( TRANSFORM_TEX(v.uv, _MainTex),
					clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF, 0.0, 1.0));

				o.vertex.y -= sin(_Time * 50) * 0.2;
				
				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 base_col = tex2D(_MainTex, i.uv) * _TintColor * 2.0;

				fixed4 col = lerp(base_col, lerp(_RimColor, base_col, i.uv.z), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
