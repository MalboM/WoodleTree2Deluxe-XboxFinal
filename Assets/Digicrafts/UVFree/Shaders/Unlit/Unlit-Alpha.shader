// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Digicrafts/UV-Free/Unlit/Transparent" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1)

	// UV-Free Properties
	[Toggle(_UVFREE_WORLDSPACE)] _UVSpace ("_UVSpace", Float) = 0
	[KeywordEnum(Free,UV0,UV1)] _UVFREE_UV ("_UVFREE_UV", Float) = 0
	[Toggle(_UVFREE_VERTEX_COLOR)] _VertexColor ("_VertexColor", Float) = 0
	_VertexColorPower ("_VertexColorPower", Range(0,5)) = 1

	_Top ("_Top", Float) = 0
	_PowerTop ("_PowerTop", Range(0,1)) = 0.5
	_ColorTop ("_ColorTop", Color) = (1,1,1,1)
	_MainTexTop ("_MainTexTop", 2D) = "white" {}

	_Bottom ("_Bottom", Float) = 0
	_PowerBottom ("_PowerBottom", Range(0,1)) = 0.5
	_ColorBottom ("_ColorBottom", Color) = (1,1,1,1)
	_MainTexBottom ("_MainTexBottom", 2D) = "white" {}

}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			#pragma shader_feature _UVFREE_UP_NONE _UVFREE_UP_TOP _UVFREE_UP_FRONT _UVFREE_UP_LEFT
			#pragma shader_feature _UVFREE_DOWN_NONE _UVFREE_DOWN_BOTTOM _UVFREE_DOWN_BACK _UVFREE_DOWN_RIGHT
			#pragma shader_feature _UVFREE_WORLDSPACE
			#pragma shader_feature _UVFREE_VERTEX_COLOR
//			#pragma shader_feature _UVFREE_UV_FREE _UVFREE_UV_UV0 _UVFREE_UV_UV1
			#include "../Core/UVFreeCommon.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;	

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				DC_UVFREE_INPUT
			};

			struct v2f {
				float4 pos : SV_POSITION;
				DC_UVFREE_OUPUT(1,2)
				UNITY_FOG_COORDS(3)
			};
						
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				DC_UVFREE_TRANSFER(o,_MainTex)
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				

				DC_UVFREE_SETUP(i.objNormal)

				fixed4 c = fixed4(1,1,1,1);
				DC_UVFREE_UV_APPLY(i,_MainTex);
				DC_UVFREE_APPLY_MAIN_TEX(c)
				c*=_Color;

				// Add top/bottom
				#if _UVFREE_UP_LEFT || _UVFREE_DOWN_RIGHT
				if(uv_normal.x>0){
					#if _UVFREE_UP_LEFT
					fixed4 top_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexTop);
					DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
					DC_UVFREE_VALUE_X(c,(top_c*_ColorTop),_PowerTop);
					#endif
				} else {
					#if _UVFREE_DOWN_RIGHT
					fixed4 bottom_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexBottom);
					DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
					DC_UVFREE_VALUE_X(c,(bottom_c*_ColorBottom),_PowerBottom);
					#endif
				}
				#endif

				#if _UVFREE_UP_FRONT || _UVFREE_DOWN_BACK
				if(uv_normal.z>0){
					#if _UVFREE_UP_FRONT
					fixed4 top_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexTop);
					DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
					DC_UVFREE_VALUE_Z(c,(top_c*_ColorTop),_PowerTop);
					#endif
				} else {
					#if _UVFREE_DOWN_BACK
					fixed4 bottom_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexBottom);
					DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
					DC_UVFREE_VALUE_Z(c,(bottom_c*_ColorBottom),_PowerBottom);
					#endif
				}
				#endif

				#if _UVFREE_UP_TOP || _UVFREE_DOWN_BOTTOM
				if(uv_normal.y>0){
					#if _UVFREE_UP_TOP
					fixed4 top_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexTop);
					DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
					DC_UVFREE_VALUE_Y(c,(top_c*_ColorTop),_PowerTop);
					#endif
				} else {
					#if _UVFREE_DOWN_BOTTOM
					fixed4 bottom_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexBottom);
					DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
					DC_UVFREE_VALUE_Y(c,(bottom_c*_ColorBottom),_PowerBottom);
					#endif
				}
				#endif

				DC_UVFREE_FINAL(i,c.rgb)
				UNITY_APPLY_FOG(i.fogCoord, c);
				return c;
			}

		ENDCG
	}
}
CustomEditor "UVFreeMobileShaderGUI"
}
