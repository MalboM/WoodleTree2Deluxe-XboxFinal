Shader "WoodleTree/Unlit/Base/base_transparency_texture (Pixel Opt)"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
		
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
            #pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct VertexData
			{
                UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 objPos : POSITION;
				float2 uv : TEXCOORD0;
				float3 objNormal : NORMAL;
			};

			struct Varys
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 objNormal : TEXCOORD2;
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _TintColor;
			uniform fixed4 _RimColor;
			uniform float _RimSharpnessF;
			uniform float _RimIntensityF;
			
			Varys vert (VertexData v)
			{
				Varys o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);                

				o.pos = UnityObjectToClipPos(v.objPos);
				o.uv = float4(
				    TRANSFORM_TEX(v.uv, _MainTex), 
					v.objPos.xy
				);
				o.objNormal = float4(v.objNormal, v.objPos.z);

				UNITY_TRANSFER_FOG(o,o.pos);
				
				return o;
			}
			
			fixed4 frag (Varys i) : SV_Target
			{
			    // object space vertex
			    float4 objPos = float4(i.uv.zw, i.objNormal.w, 1.0);
			    
				fixed4 base_col = tex2D(_MainTex, i.uv.xy);
				fixed alpha = base_col.a;
				base_col *= _TintColor * 2.0;
				
				float rimF = clamp(dot(normalize(ObjSpaceViewDir(objPos)), i.objNormal.xyz) * _RimSharpnessF, 0.0, 1.0);

				fixed3 col = lerp(base_col.rgb, _RimColor, (1. - rimF) * _RimIntensityF);

				UNITY_APPLY_FOG(i.fogCoord, col);

				return fixed4(col, alpha);
			}
			ENDCG
		}
	}
}
