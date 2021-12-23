Shader "WoodleTree/Unlit/Triplanar/Detail/triplanar_projection_diffuse_detail"
{
	Properties
	{
		[Header(Texture Settings)]
		[NoScaleOffset]
		_MainTex ("Main Texture", 2D) = "white" {}
		[NoScaleOffset]
		_DetailTex("Detail Tex", 2D) = "gray" {}
		_TileF("Tiling", Range(0.05, 20)) = 1
		_AttenuationF("Detail Strength", Range(0.0, 1.0)) = 0.5
		_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
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
			float3 direction : NORMAL;
			UNITY_FOG_COORDS(1)
		};

		sampler2D _MainTex;
		sampler2D _DetailTex;
		fixed4 _TintColor;
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
			o.direction = abs(power_normal * power_normal * power_normal * power_normal); //to avoid texture bleeding from on edge to the other
			o.direction /= dot(o.direction, 1.0);

			UNITY_TRANSFER_FOG(o, o.vertex);

			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 baseCol = tex2D(_MainTex, i.uv.yz) * i.direction.x +
							 tex2D(_MainTex, i.uv.xz) * i.direction.y +
							 tex2D(_MainTex, i.uv.xy) * i.direction.z;

			baseCol *= _TintColor * 2.0;

			fixed4 detailCol = tex2D(_DetailTex, i.uv.yz) * i.direction.x +
							   tex2D(_DetailTex, i.uv.xz) * i.direction.y +
							   tex2D(_DetailTex, i.uv.xy) * i.direction.z;

			fixed4 col = (lerp(float4(1.0, 1.0, 1.0, 1.0), detailCol, _AttenuationF) * (lerp(baseCol, lerp(_RimColor, baseCol, i.uv.w), _RimIntensityF)));
			
			UNITY_APPLY_FOG(i.fogCoord, col);
			
			return col;
		}
			ENDCG
		}
	}
			Fallback "Diffuse"
}
