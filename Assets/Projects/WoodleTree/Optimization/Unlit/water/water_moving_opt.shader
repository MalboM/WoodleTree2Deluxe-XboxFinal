Shader "WoodleTree/Unlit/Water/water_moving (Opt)"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PanSpeed("Pan Speed (x,y)", Vector) = (0.0, 1.0, 0.0, 0.0)
		_OpacityF("Opacity", Range(0.0, 1.0)) = 0.75
		
		[Header(Rim Effect)]
        [Toggle(RIM)] _RIM("Activate Rim", Float) = 0        
        _RimColor("Rimlight Color", Color) = (0.1, 0.1, 0.9, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0 		
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Blend One OneMinusSrcAlpha // treat src as alpha 1 (adding more brightness to the transparent object)
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
            #pragma multi_compile_instancing
			
			#pragma shader_feature RIM

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
				float3 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)			
			};

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed2 _PanSpeed;
			uniform fixed _OpacityF;

            // rim uniform
            uniform fixed4 _RimColor;
            uniform fixed _RimSharpnessF;
            uniform fixed _RimIntensityF;
            
			Varys vert (VertexData v)
			{
				Varys o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(Varys, o);                

				o.pos = UnityObjectToClipPos(v.objPos);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.xy += _Time.x * _PanSpeed.xy;
				
                #ifdef RIM
                o.uv.z = clamp(dot(normalize(ObjSpaceViewDir(v.objPos)), v.objNormal) * _RimSharpnessF, 0.0, 1.0);
                #else
                o.uv.z = 0.0;
                #endif				

				UNITY_TRANSFER_FOG(o,o.pos);

				return o;
			}
			
			fixed4 frag (Varys i) : SV_Target
			{
				fixed4 baseCol = tex2D(_MainTex, i.uv);
				baseCol.a = _OpacityF;
				
				#ifdef RIM
                baseCol.rgb = lerp(baseCol.rgb, _RimColor.rgb, (1. - i.uv.z)*_RimIntensityF);
                #endif				

				UNITY_APPLY_FOG(i.fogCoord, baseCol);

				return baseCol;
			}
			ENDCG
		}
	}
}
