Shader "WoodleTree/Lit/Toon/toon_pixel_simple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LightRamp("Shading Ramp Texture", 2D) = "gray" {}
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
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 vertex : TEXCOORD2;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			sampler2D _LightRamp;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal = v.normal;
				o.vertex = v.vertex;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed toon_diffuse = tex2D(_LightRamp, float2(max(dot(i.normal, normalize(ObjSpaceLightDir(i.vertex))), 0.0), 0.5)).r;
				fixed4 baseColor = tex2D(_MainTex, i.uv) * toon_diffuse;
		
#ifdef _POSITION_GROUND
				baseColor += unity_AmbientGround;
#elif _POSITION_EQUATOR
				baseColor += unity_AmbientEquator;
#elif _POSITION_SKY
				baseColor += unity_AmbientSky;
#elif _POSITION_SKYBOX
				baseColor += UNITY_LIGHTMODEL_AMBIENT;
#endif

				UNITY_APPLY_FOG(i.fogCoord, baseColor);
				return baseColor;
			}
			ENDCG
		}
	}
}
