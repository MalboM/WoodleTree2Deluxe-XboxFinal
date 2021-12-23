Shader "WoodleTree/Unlit/Base/color_base"
{
	Properties
	{
		_MainColor("Object Color", Color) = (0.5, 0.7, 0.2, 1.0)
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(0)
				float4 vertex : SV_POSITION;
				float rimF : NORMAL;
			};

			fixed4 _MainColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.rimF = clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF, 0.0, 1.0);

				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = lerp(_MainColor, lerp(_RimColor, _MainColor, i.rimF), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
		Fallback "Diffuse"
}
