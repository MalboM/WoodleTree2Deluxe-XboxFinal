Shader "WoodleTree/Lit/color_base"
{
	Properties
	{
		_MainColor("Object Color", Color) = (0.5, 0.7, 0.2, 1.0)
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
		[KeywordEnum(Ground, Equator, Sky)] _Position ("Ambient Light Source", Float) = 0.0
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
			#pragma multi_compile _POSITION_NONE _POSITION_GROUND _POSITION_EQUATOR _POSITION_SKY _POSITION_SKYBOX

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
				float4 color : NORMAL;
			};

			fixed4 _MainColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = float4((_MainColor * max(dot(normalize(WorldSpaceLightDir(v.vertex)), v.normal), 0.0)).xyz, 
								 clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF, 0.0, 1.0));
#ifdef _POSITION_GROUND
				o.color.xyz += unity_AmbientGround.xyz;
#elif _POSITION_EQUATOR
				o.color.xyz += unity_AmbientEquator.xyz;
#elif _POSITION_SKY
				o.color.xyz += unity_AmbientSky.xyz;
#elif _POSITION_SKYBOX
				o.color.xyz += UNITY_LIGHTMODEL_AMBIENT;
#endif
				UNITY_TRANSFER_FOG(o,o.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 baseColor = fixed4(i.color.xyz, 1.0);
				fixed4 col = lerp(baseColor, lerp(_RimColor, baseColor, i.color.w), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
		Fallback "Diffuse"
}
