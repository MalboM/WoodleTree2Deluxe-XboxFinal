
Shader "WoodleTree/Lit/Triplanar/Detail/triplanar_projection_detail"
{
	Properties
	{
		[Header(Texture Settings)]
		[NoScaleOffset]
		_MainColor ("Main Color", Color) = (0.5, 0.7, 0.2, 1.0)
		[NoScaleOffset]
		_DetailTex ("Detail Tex", 2D) = "gray" {}
		_TileF ("Tiling", Range(0.05, 20)) = 1
		_AttenuationF ("Detail Strength", Range(0.0, 1.0)) = 0.5
		[KeywordEnum(Ground, Equator, Sky, Skybox, None)] _Position("Ambient Light Source", Float) = 0.0
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
				float4 vertex : SV_POSITION;
				float4 uv : TEXCOORD0; //.w component used to tranfer rim factor
				float4 direction : NORMAL;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _DetailTex;
			fixed4 _MainColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			float _TileF;
			float _AttenuationF;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = float4(mul(unity_ObjectToWorld, v.vertex).xyz * _TileF,
							  clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF, 0.0, 1.0));
				float3 power_normal = v.normal * 1.4;
				o.direction = float4(abs(power_normal * power_normal * power_normal * power_normal), //to avoid texture bleeding from on edge to the other
									 max(dot(normalize(WorldSpaceLightDir(v.vertex)), v.normal), 0.0));
				o.direction.xyz /= dot(o.direction.xyz, 1.0);

				UNITY_TRANSFER_FOG(o, o.vertex);

				return o;
			}
			
			inline float4 expand_range(float4 in_val)
			{
				return (in_val * 2.0 - 1.0);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mainCol = _MainColor;
				fixed4 detailCol = tex2D(_DetailTex, i.uv.yz) * i.direction.x +
								   tex2D(_DetailTex, i.uv.xz) * i.direction.y +
								   tex2D(_DetailTex, i.uv.xy) * i.direction.z;
				
#ifdef _POSITION_GROUND
				mainCol *= i.direction.w + unity_AmbientGround;
				detailCol *= i.direction.w + unity_AmbientGround;
#elif _POSITION_EQUATOR
				mainCol *= i.direction.w + unity_AmbientEquator;
				detailCol *= i.direction.w + unity_AmbientEquator;
#elif _POSITION_SKY
				mainCol *= i.direction.w + unity_AmbientSky;
				detailCol *= i.direction.w + unity_AmbientSky;
#elif _POSITION_SKYBOX
				mainCol *= i.direction.w + UNITY_LIGHTMODEL_AMBIENT;
				detailCol *= i.direction.w + UNITY_LIGHTMODEL_AMBIENT;
#endif

				fixed4 col = (lerp(mainCol, detailCol, _AttenuationF) * (lerp(mainCol, lerp(_RimColor, mainCol, i.uv.w), _RimIntensityF)));

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
		Fallback "Diffuse"
}
