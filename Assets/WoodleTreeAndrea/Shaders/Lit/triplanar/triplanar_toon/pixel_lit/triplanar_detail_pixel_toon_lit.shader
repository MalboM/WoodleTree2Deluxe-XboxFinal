
Shader "WoodleTree/Lit/Triplanar/Toon/Pixel/triplanar_detail_toon_lit"
{
	Properties
	{
		_MainColor ("Main Color", Color) = (0.5, 0.7, 0.2, 1.0)
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
		_DetailTex ("Detail Tex", 2D) = "gray" {}
		_LightRamp("Shading Ramp Texture", 2D) = "gray" {}
		_TileF ("Tiling", Range(0.05, 20)) = 1
		_AttenuationF ("Detail Strength", Range(0.0, 1.0)) = 0.5
  [KeywordEnum(None, Ground, Equator, Sky, Skybox)] _Position("Ambient Light Source", Float) = 0.0

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
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float4 uv : TEXCOORD0; //.w component used to tranfer rim factor
				float3 direction : TANGENT;
				float3 normal : NORMAL;
				float4 vertex : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _DetailTex;
			sampler2D _LightRamp;
			fixed4 _MainColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			float _TileF;
			float _AttenuationF;

			v2f vert (appdata v)
			{
				v2f o;

				o.position = UnityObjectToClipPos(v.vertex);

				o.uv = float4(mul(unity_ObjectToWorld, v.vertex).xyz * _TileF,
					saturate(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF)); // , 0.0, 1.0));
				
				float3 power_normal = v.normal * 1.4;
				o.direction = abs(power_normal * power_normal * power_normal * power_normal); //to avoid texture bleeding from on edge to the other
				o.direction /= dot(o.direction, 1.0);

				o.normal = v.normal;
				o.vertex = v.vertex;

				UNITY_TRANSFER_FOG(o, o.position);

				return o;
			}

			inline float4 expand_range(float4 in_val)
			{
				return (in_val * 2.0);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed toon_diffuse = tex2D(_LightRamp, float2(max(dot(i.normal, normalize(ObjSpaceLightDir(i.vertex))), 0.0), 0.5)).r;
				
				fixed4 mainCol = _MainColor;
				fixed4 detailCol = expand_range(tex2D(_DetailTex, i.uv.yz)) * i.direction.x +
								   expand_range(tex2D(_DetailTex, i.uv.xz)) * i.direction.y +
								   expand_range(tex2D(_DetailTex, i.uv.xy)) * i.direction.z;

#ifdef _POSITION_GROUND
				mainCol *= toon_diffuse + unity_AmbientGround;
				detailCol *= toon_diffuse + unity_AmbientGround;
#elif _POSITION_EQUATOR
				mainCol *= toon_diffuse + unity_AmbientEquator;
				detailCol *= toon_diffuse + unity_AmbientEquator;
#elif _POSITION_SKY
				mainCol *= toon_diffuse + unity_AmbientSky;
				detailCol *= toon_diffuse + unity_AmbientSky;
#elif _POSITION_SKYBOX
				mainCol *= toon_diffuse + UNITY_LIGHTMODEL_AMBIENT;
				detailCol *= toon_diffuse + UNITY_LIGHTMODEL_AMBIENT;
#endif

				fixed4 col = (lerp(mainCol, detailCol, _AttenuationF) * (lerp(mainCol, lerp(_RimColor, mainCol, i.uv.w), _RimIntensityF)));
				//mainCol * 
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
		Fallback "Diffuse"
}
