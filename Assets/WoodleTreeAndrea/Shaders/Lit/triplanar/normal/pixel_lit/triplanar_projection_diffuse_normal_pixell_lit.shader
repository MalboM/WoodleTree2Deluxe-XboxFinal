Shader "WoodleTree/Lit/Triplanar/Normal/triplanar_projection_diffuse_normal_pixel_lit"
{
	Properties
	{ 
		[Header(Texture Settings)]
		[NoScaleOffset]
		_MainTex ("Main Texture", 2D) = "white" {}
		[NoScaleOffset]
		_NormalTex ("Normal Tex", 2D) = "gray" {}
		_TileF ("Tiling", Range(0.05, 20)) = 1
		_AttenuationF ("Normal Strength", Range(0.0, 1.0)) = 0.5
		_TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
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
			float4 tangent : TANGENT;
		};

			struct v2f
			{
				float4 position : SV_POSITION;
				float4 uv : TEXCOORD0;			   //.w component used to tranfer rim factor
				float4 normalWorld : TANGENT;      //.w component used to transfer direction.x factor
				float4 tangentWorld : NORMAL;      //.w component used to transfer direction.x factor
				float4 bitangentWorld : TEXCOORD2; //.w component used to transfer direction.x factor
				float4 vertex : TEXCOORD3;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			sampler2D _NormalTex;
			fixed4 _TintColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			float _TileF;
			float _AttenuationF;

			v2f vert(appdata v)
			{
				v2f o;

				o.position = UnityObjectToClipPos(v.vertex);
				o.vertex = v.vertex; // mul(unity_ObjectToWorld, v.vertex);

				o.uv = float4(mul(unity_ObjectToWorld, v.vertex).xyz * _TileF,
					clamp(dot(normalize(ObjSpaceViewDir(v.vertex)), v.normal) *_RimSharpnessF, 0.0, 1.0));

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
				float3x3 local2WorldTranspose = float3x3(
					i.tangentWorld.xyz,
					i.bitangentWorld.xyz,
					i.normalWorld.xyz
					);

				fixed4 baseCol = tex2D(_MainTex, i.uv.yz) * i.normalWorld.w +
								 tex2D(_MainTex, i.uv.xz) * i.tangentWorld.w +
								 tex2D(_MainTex, i.uv.xy) * i.bitangentWorld.w;

				half4 normalCol = tex2D(_NormalTex, i.uv.yz) * i.normalWorld.w +
								  tex2D(_NormalTex, i.uv.xz) * i.tangentWorld.w +
								  tex2D(_NormalTex, i.uv.xy) * i.bitangentWorld.w;

				fixed4 diffuse_color = baseCol * _TintColor * 2.0;

				normalCol = lerp(float4(0.5, 0.5, 1.0, 1.0), normalCol, _AttenuationF);

				half3 worldNormal = normalize(mul(UnpackNormal(normalCol), local2WorldTranspose));

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

				i.uv.w = saturate(dot(normalize(WorldSpaceViewDir(i.vertex)), worldNormal) *_RimSharpnessF);

				fixed4 col = lerp(diffuse_color, lerp(_RimColor, diffuse_color, i.uv.w), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
