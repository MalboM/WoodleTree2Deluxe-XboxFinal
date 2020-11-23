Shader "WoodleTree/Special/Sand_Snow_Terrain"
{
	Properties
	{ 
		[Header(Texture Settings)]
		[NoScaleOffset]
		_MainTex ("Main Texture (RGB) Sparkle (A)", 2D) = "white" {}
		[NoScaleOffset]
		_BaseNormalTex ("Base Normal Tex", 2D) = "gray" {}
		[KeywordEnum(No, Yes)] _DetailNormalFlag("Use Detail Normal", Float) = 0.0
		[NoScaleOffset]
		_DetailNormalTex("Detail Normal Tex", 2D) = "gray" {}
		_BaseTileF ("Base Tiling", Range(0.05, 20)) = 1
		_DetailTileF("Detail Tiling", Range(1, 10)) = 2
		_AttenuationF ("Normal Strength", Range(0.0, 1.0)) = 0.5
		_TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
		[KeywordEnum(Ground, Equator, Sky, Skybox, None)] _Position("Ambient Light Source", Float) = 0.0
		[Header(Rimlight Settings)]
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0
		[Header(Sparkle Settings)]
		_SparkleF("Sparkle intensity", Range(0.0, 5.0)) = 2.0
		_SparkleFallof("Sparkle Fallof", Range(0.0, 1.0)) = 0.05
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
			#pragma multi_compile _DETAILNORMALFLAG_NO, _DETAILNORMALFLAG_YES
			#include "UnityCG.cginc"

			struct appdata
			{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
			float3 normal : NORMAL;
			float4 tangent : TANGENT;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float3 uv : TEXCOORD0;			   
				float4 normalWorld : TANGENT;      //.w component used to transfer direction.x factor
				float4 tangentWorld : NORMAL;      //.w component used to transfer direction.x factor
				float4 bitangentWorld : TEXCOORD2; //.w component used to transfer direction.x factor
				float4 vertex : TEXCOORD3;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;  //alpha channel used for sparkle texture
			sampler2D _BaseNormalTex;
			sampler2D _DetailNormalTex;
			fixed4 _TintColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			float _BaseTileF;
			float _DetailTileF;
			float _AttenuationF;
			float _SparkleF;
			float _SparkleFallof;

			//taken from http://blog.selfshadow.com/publications/blending-in-detail/, reorient detail normal map to follow the base normal map direction
			inline float3 ReorientNormalMap(float3 baseNormal, float3 detailNormal)
			{
				float3 t = baseNormal * float3(2, 2, 2) + float3(-1, -1, 0);
				float3 u = detailNormal * float3(-2, -2, 2) + float3(1, 1, -1);
				return normalize(t * dot(t, u) - u * t.z);
			}

			v2f vert(appdata v)
			{
				v2f o;

				o.position = UnityObjectToClipPos(v.vertex);
				o.vertex = v.vertex;

				o.uv = mul(unity_ObjectToWorld, v.vertex).xyz * _BaseTileF;

				float3 power_normal = UnityObjectToWorldNormal(v.normal) * 1.4;
				float3 direction = abs(power_normal * power_normal * power_normal * power_normal); //to avoid texture bleeding from on edge to the other
				direction /= dot(direction, float3(1.0, 1.0, 1.0));

				o.normalWorld = float4(UnityObjectToWorldNormal(v.normal), direction.x);
				o.tangentWorld = float4(UnityObjectToWorldDir(v.tangent.xyz), direction.y);
				o.bitangentWorld = float4(cross(o.normalWorld.xyz, o.tangentWorld.xyz), direction.z);

				UNITY_TRANSFER_FOG(o, o.position);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				half3 worldNormal = half3(0.5, 0.5, 1.0);

				float3x3 local2WorldTranspose = float3x3(
					i.tangentWorld.xyz,
					i.bitangentWorld.xyz,
					i.normalWorld.xyz
					);

				half4 baseCol = tex2D(_MainTex, i.uv.yz) * i.normalWorld.w +
								tex2D(_MainTex, i.uv.xz) * i.tangentWorld.w +
								tex2D(_MainTex, i.uv.xy) * i.bitangentWorld.w;

				half4 baseNormalCol = tex2D(_BaseNormalTex, i.uv.yz) * i.normalWorld.w +
									  tex2D(_BaseNormalTex, i.uv.xz) * i.tangentWorld.w +
									  tex2D(_BaseNormalTex, i.uv.xy) * i.bitangentWorld.w;
#ifdef _DETAILNORMALFLAG_YES
				half4 detailNormalCol = tex2D(_DetailNormalTex, i.uv.yz * _DetailTileF) * i.normalWorld.w +
										tex2D(_DetailNormalTex, i.uv.xz * _DetailTileF) * i.tangentWorld.w +
										tex2D(_DetailNormalTex, i.uv.xy * _DetailTileF) * i.bitangentWorld.w;

				baseNormalCol = lerp(float4(0.5, 0.5, 1.0, 1.0), baseNormalCol, _AttenuationF * _AttenuationF);
				detailNormalCol = lerp(float4(0.5, 0.5, 1.0, 1.0), detailNormalCol, _AttenuationF);

				half3 normal = ReorientNormalMap(baseNormalCol, detailNormalCol);
				worldNormal = normalize(mul(normal, local2WorldTranspose));

#else
				baseNormalCol = lerp(float4(0.5, 0.5, 1.0, 1.0), baseNormalCol, _AttenuationF * _AttenuationF);
				worldNormal = normalize(mul(UnpackNormal(baseNormalCol), local2WorldTranspose));
#endif
				half sparkle = baseCol.a;
				half3 dist = _WorldSpaceCameraPos - i.uv.xyz;
				sparkle = lerp(sparkle, 0.0, clamp(dot(dist, dist) * (1.0 - _SparkleFallof), 0, 1));

				fixed4 diffuse_color = baseCol * _TintColor * 2.0;

				float light_diffuse = max(dot(normalize(WorldSpaceLightDir(i.vertex)), worldNormal), 0.0);

#ifdef _POSITION_GROUND
				diffuse_color *= light_diffuse + unity_AmbientGround;
#elif _POSITION_EQUATOR
				diffuse_color *= light_diffuse + unity_AmbientEquator;
#elif _POSITION_SKY
				diffuse_color *= light_diffuse + unity_AmbientSky;
#elif _POSITION_SKYBOX
				diffuse_color *= light_diffuse + UNITY_LIGHTMODEL_AMBIENT;
#endif

				float rimScale = saturate(dot(normalize(WorldSpaceViewDir(i.vertex)), worldNormal) *_RimSharpnessF);

				fixed4 col = lerp(diffuse_color, lerp(_RimColor, diffuse_color, rimScale), _RimIntensityF);

				col += sparkle * _SparkleF;

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
