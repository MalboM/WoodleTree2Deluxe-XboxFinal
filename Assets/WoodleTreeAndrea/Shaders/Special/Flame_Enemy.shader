Shader "WoodleTree/Special/Flame_Enemy"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SpeedF("Fire Speed", Range(1.0, 5.0)) = 2.0
		_LightF("Light Intensity", Range(0.05, 0.5)) = 0.2
		_LightD("Light Direction", Range(-1.0, 1.0)) = 1.0
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
				float3 color : COLOR;

				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;		

			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			float _SpeedF;
			float _LightF;
			float _LightD;

			v2f vert (appdata v)
			{
				v2f o;

				v.vertex.x += sin(_Time.z * _SpeedF + /*(v.vertex.x * 10) +*/ (v.vertex.y * 25.0)) * 0.015 * (v.vertex.y * 2);
				v.vertex.z += sin(_Time.z * _SpeedF + /*(v.vertex.x * 20) +*/ (v.vertex.y * 20.0)) * 0.015 * (v.vertex.y * 2);
				//v.vertex.x += sin(_Time.w * (v.vertex.y * 5) + (v.vertex.z * 800.0)) * v.vertex.y * 0.007; //high-frequence noise
				//v.vertex.z += sin(_Time.w * (v.vertex.y * 7) + (v.vertex.z * 600.0)) * v.vertex.y * 0.007; //high-frequence noise

				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.uv = float4(TRANSFORM_TEX(v.uv, _MainTex), v.vertex.xy);
				o.normal = float4(v.normal, v.vertex.z);

				o.color = v.vertex.y * (sin((_Time.w * _LightD) + (v.vertex.x * v.vertex.z * 60)) * 0.5 + 0.5) * _LightF;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 base_col = tex2D(_MainTex, i.uv);
				base_col.xyz += i.color.xyz;
				
				float rimF = clamp(dot(normalize(ObjSpaceViewDir(float4(i.uv.zw, i.normal.w, 1.0))), i.normal.xyz) * _RimSharpnessF, 0.0, 1.0);
				fixed4 col = lerp(base_col, saturate(lerp(_RimColor, base_col, rimF)), _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			ENDCG
		}
	}
}
