// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color
Shader "Digicrafts/UV-Free/Unlit/Projector" {
Properties {

	_MainTex ("Base (RGB)", 2D) = "white" {}
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

	_ProjectorTex ("Projector Texture", 2D) = "white" {}
	_ProjectorFalloffTex ("Falloff Texture", 2D) = "white" {}
}

SubShader {

	Tags { "RenderType"="Transparent" "Queue"="Transparent"}
	ZWrite Off
	ColorMask RGB
	Blend SrcAlpha OneMinusSrcAlpha 
	Offset -1, -1	

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
			#include "Assets/Digicrafts/UVFree/Shaders/Core/UVFreeCommon.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform fixed4 _Color;

			uniform sampler2D _ProjectorTex;
			uniform sampler2D _ProjectorFalloffTex;
			uniform float4x4 unity_Projector;
			uniform float4x4 unity_ProjectorClip;

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				DC_UVFREE_INPUT
			};

			struct v2f {
				float4 pos : SV_POSITION;
				DC_UVFREE_OUPUT(1,2)
				UNITY_FOG_COORDS(3)
				float4 projector : TEXCOORD3;
				float4 projectorFalloff : TEXCOORD4;
			};
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				DC_UVFREE_TRANSFER(o,_MainTex)
				UNITY_TRANSFER_FOG(o,o.pos);
				o.projector = mul (unity_Projector, v.vertex);
				o.projectorFalloff = mul (unity_ProjectorClip, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				fixed4 projectorColor = tex2Dproj (_ProjectorTex, UNITY_PROJ_COORD(i.projector));
				fixed4 projectorFalloff = tex2Dproj (_ProjectorFalloffTex, UNITY_PROJ_COORD(i.projectorFalloff));

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

				DC_UVFREE_FINAL(i,c)
				UNITY_APPLY_FOG(i.fogCoord, c);

				c = lerp(fixed4(1,1,1,0), c, projectorColor.a*projectorFalloff.a);

				return c;
			}

		ENDCG
	}
}
CustomEditor "UVFreeMobileShaderGUI"
}