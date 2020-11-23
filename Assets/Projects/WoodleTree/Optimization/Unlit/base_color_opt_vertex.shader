Shader "WoodleTree/Unlit/Base/color_base_opt (Vertex)"
{
	Properties
	{
		_MainColor("Object Color", Color) = (0.5, 0.7, 0.2, 1.0)
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
            #pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct VertexData
			{
                UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 objPos : POSITION;
				float3 objNormal : NORMAL;
			};

			struct Varys
			{
    			UNITY_FOG_COORDS(0)
				float4 pos : SV_POSITION;
				fixed4 color : TEXCOORD1;
			};

			uniform fixed4 _MainColor;
			uniform fixed4 _RimColor;
			uniform float _RimSharpnessF;
			uniform float _RimIntensityF;
			
			Varys vert (VertexData v)
			{
				Varys o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);                

				o.pos = UnityObjectToClipPos(v.objPos);
				
				float rimF = clamp(dot(normalize(ObjSpaceViewDir(v.objPos)), v.objNormal) * _RimSharpnessF, 0.0, 1.0);
				o.color = lerp(_MainColor, _RimColor, (1. - rimF)*_RimIntensityF);
                
                UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			
			fixed4 frag (Varys i) : SV_Target
			{
			    fixed4 col = i.color;
				UNITY_APPLY_FOG(i.fogCoord, col);

				return col;
			}
			
			ENDCG
		}
	}
}
