Shader "WoodleTree/Lit/Triplanar/Toon/triplanar_diffuse_detail_vertex_toon_lit"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
		_LightRamp("Shading Ramp Texture", 2D) = "gray" {}
		_DetailTex("Detail Tex", 2D) = "gray" {}
		_TileF("Tiling", Range(0.05, 20)) = 1
		_AttenuationF("Detail Strength", Range(0.0, 1.0)) = 0.5
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
			float4 vertex : SV_POSITION;
			float4 uv : TEXCOORD0; //.w component used to tranfer rim factor
			float4 direction : NORMAL;
			UNITY_FOG_COORDS(1)
		};

		sampler2D _MainTex;
		sampler2D _DetailTex;
		sampler2D _LightRamp;
		fixed4 _MainColor;
		fixed4 _RimColor;
		float _RimSharpnessF;
		float _RimIntensityF;
		float _TileF;
		float _AttenuationF;

		v2f vert(appdata v)
		{
			v2f o;

			o.vertex = UnityObjectToClipPos(v.vertex);

			o.uv = float4(mul(unity_ObjectToWorld, v.vertex).xyz * _TileF,
						  clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF, 0.0, 1.0));

			float3 power_normal = v.normal * 1.4;
			o.direction = float4(abs(power_normal * power_normal * power_normal * power_normal), //to avoid texture bleeding from on edge to the other
								 tex2Dlod(_LightRamp, float4(max(dot(v.normal, normalize(ObjSpaceLightDir(v.vertex))), 0.0), 0.5, 0.0, 0.0)).r);
			o.direction.xyz /= dot(o.direction.xyz, 1.0);

			UNITY_TRANSFER_FOG(o, o.vertex);

			return o;
		}

		inline float4 expand_range(float4 in_val)
		{
			return (in_val * 2.0);
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 baseCol = tex2D(_MainTex, i.uv.yz) * i.direction.x + tex2D(_MainTex, i.uv.xz) * i.direction.y + tex2D(_MainTex, i.uv.xy) * i.direction.z;
			fixed4 detailCol = expand_range(tex2D(_DetailTex, i.uv.yz)) * i.direction.x +
							   expand_range(tex2D(_DetailTex, i.uv.xz)) * i.direction.y +
							   expand_range(tex2D(_DetailTex, i.uv.xy)) * i.direction.z;

#ifdef _POSITION_GROUND
			baseCol *= i.direction.w + unity_AmbientGround;
			detailCol *= i.direction.w + unity_AmbientGround;
#elif _POSITION_EQUATOR
			baseCol *= i.direction.w + unity_AmbientEquator;
			detailCol *= i.direction.w + unity_AmbientEquator;
#elif _POSITION_SKY
			baseCol *= i.direction.w + unity_AmbientSky;
			detailCol *= i.direction.w + unity_AmbientSky;
#elif _POSITION_SKYBOX
			baseCol *= i.direction.w + UNITY_LIGHTMODEL_AMBIENT;
			detailCol *= i.direction.w + UNITY_LIGHTMODEL_AMBIENT;
#endif

			fixed4 col = (lerp(float4(1.0, 1.0, 1.0, 1.0), detailCol, _AttenuationF) * (lerp(baseCol, lerp(_RimColor, baseCol, i.uv.w), _RimIntensityF)));
			
			UNITY_APPLY_FOG(i.fogCoord, col);
			
			return col;
		}
			ENDCG
		}
	}
}
