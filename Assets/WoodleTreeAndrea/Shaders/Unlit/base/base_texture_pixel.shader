Shader "WoodleTree/Unlit/Base/base_texture_pixel"
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
				float4 uv : TEXCOORD0;
				float4 normal : NORMAL;
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
				o.uv = float4(TRANSFORM_TEX(v.uv, _MainTex), 
					v.vertex.xy);
				o.normal = float4(v.normal, v.vertex.z);

				UNITY_TRANSFER_FOG(o,o.vertex);
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 base_col = tex2D(_MainTex, i.uv.xy) * _TintColor * 2.0;
				float rimF = clamp(dot(normalize(ObjSpaceViewDir(float4(i.uv.zw, i.normal.w, 1.0))), i.normal.xyz) * _RimSharpnessF, 0.0, 1.0);

				fixed4 col = lerp(base_col, saturate(lerp(_RimColor, base_col, rimF)), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
