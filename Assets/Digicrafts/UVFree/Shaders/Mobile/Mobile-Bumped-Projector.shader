Shader "Digicrafts/UV-Free/Mobile/Projector/Bumped Diffuse" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_BumpScale("Scale", Float) = 1.0
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

	_BumpScaleTop("Scale", Float) = 1.0
	_BumpMapTop ("_BumpMapTop", 2D) = "bump" {}
	_BumpScaleBottom("Scale", Float) = 1.0
	_BumpMapBottom ("_BumpMapBottom", 2D) = "bump" {}

	_ProjectorTex ("Projector Texture", 2D) = "white" {}
	_ProjectorFalloffTex ("Falloff Texture", 2D) = "white" {}	

}
SubShader {
Tags { "RenderType"="Transparent" "Queue"="Transparent"}
ZWrite Off
ColorMask RGB
Blend SrcAlpha OneMinusSrcAlpha 
Offset -1, -1

CGPROGRAM
#pragma surface surf Lambert noforwardadd vertex:vert alpha
#pragma target 3.0

#pragma shader_feature _UVFREE_UP_NONE _UVFREE_UP_TOP _UVFREE_UP_FRONT _UVFREE_UP_LEFT
#pragma shader_feature _UVFREE_DOWN_NONE _UVFREE_DOWN_BOTTOM _UVFREE_DOWN_BACK _UVFREE_DOWN_RIGHT
#pragma shader_feature _UVFREE_WORLDSPACE
#pragma shader_feature _UVFREE_VERTEX_COLOR
//#pragma shader_feature _UVFREE_UV_FREE _UVFREE_UV_UV0 _UVFREE_UV_UV1
#include "../Core/UVFreeCommon.cginc"

struct Input {
	float4 vertex;
	float3 objNormal;
	DC_UVFREE_WORLD_POS
	DC_UVFREE_VCOLOR
	INTERNAL_DATA
};

void vert (inout appdata_full v, out Input o) {
	UNITY_INITIALIZE_OUTPUT(Input,o);
  	DC_UVFREE_TRANSFER(o,_MainTex)
}

uniform sampler2D _MainTex;
uniform half4 _MainTex_ST;
uniform fixed4 _Color;

uniform sampler2D _ProjectorTex;
uniform sampler2D _ProjectorFalloffTex;
uniform float4x4 unity_Projector;
uniform float4x4 unity_ProjectorClip;

void surf (Input i, inout SurfaceOutput o) {
	
	DC_UVFREE_SETUP(i.objNormal)	

	fixed4 projectorColor = tex2Dproj (_ProjectorTex, UNITY_PROJ_COORD(mul (unity_Projector, i.vertex)));
	fixed4 projectorFalloff = tex2Dproj (_ProjectorFalloffTex, UNITY_PROJ_COORD(mul (unity_ProjectorClip, i.vertex)));

	fixed4 c = fixed4(1,1,1,1);
	DC_UVFREE_UV_APPLY(i,_MainTex)
	DC_UVFREE_APPLY_MAIN_TEX(c)
	c*=_Color;

	fixed3 n = fixed3(1,1,1);
	DC_UVFREE_SCALE_NORMAL_APPLY(n,_BumpMap,_MainTex,_BumpScale);

	// Add top/bottom
	#if _UVFREE_UP_LEFT || _UVFREE_DOWN_RIGHT
	if(uv_normal.x>0){
		#if _UVFREE_UP_LEFTLEFT
		fixed4 top_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexTop);
		DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
		DC_UVFREE_LERP_RATIO_X(c,(top_c*_ColorTop),_PowerTop,ratio);

		fixed3 top_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
		DC_UVFREE_LERP(n,top_n,ratio);

		#endif
	} else {
		#if _UVFREE_DOWN_RIGHT
		fixed4 bottom_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexBottom);
		DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
		DC_UVFREE_LERP_RATIO_X(c,(bottom_c*_ColorBottom),_PowerBottom,ratio);

		fixed3 bottom_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(bottom_n,_BumpMapBottom,_MainTexBottom,_BumpScaleBottom);
		DC_UVFREE_LERP(n,bottom_n,ratio);

		#endif
	}
	#endif

	#if _UVFREE_UP_FRONT || _UVFREE_DOWN_BACK
	if(uv_normal.z>0){
		#if _UVFREE_UP_FRONT
		fixed4 top_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexTop);
		DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
		DC_UVFREE_LERP_RATIO_Z(c,(top_c*_ColorTop),_PowerTop,ratio);

		fixed3 top_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
		DC_UVFREE_LERP(n,top_n,ratio);

		#endif
	} else {
		#if _UVFREE_DOWN_BACK
		fixed4 bottom_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexBottom);
		DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
		DC_UVFREE_LERP_RATIO_Z(c,(bottom_c*_ColorBottom),_PowerBottom,ratio);

		fixed3 bottom_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(bottom_n,_BumpMapBottom,_MainTexBottom,_BumpScaleBottom);
		DC_UVFREE_LERP(n,bottom_n,ratio);

		#endif
	}
	#endif

	#if _UVFREE_UP_TOP || _UVFREE_DOWN_BOTTOM
	if(uv_normal.y>0){
		#if _UVFREE_UP_TOP
		fixed4 top_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexTop);
		DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
		DC_UVFREE_LERP_RATIO_Y(c,(top_c*_ColorTop),_PowerTop,ratio);

		fixed3 top_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
		DC_UVFREE_LERP(n,top_n,ratio);

		#endif
	} else {
		#if _UVFREE_DOWN_BOTTOM
		fixed4 bottom_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexBottom);
		DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
		DC_UVFREE_LERP_RATIO_Y(c,(bottom_c*_ColorBottom),_PowerBottom,ratio);

		fixed3 bottom_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(bottom_n,_BumpMapBottom,_MainTexBottom,_BumpScaleBottom);
		DC_UVFREE_LERP(n,bottom_n,ratio);

		#endif
	}
	#endif

	DC_UVFREE_FINAL(i,c.rgb)
	
	float pa = projectorColor.a*projectorFalloff.a;

	o.Normal = n.rgb;//*pc.a;//lerp(float3(0,0,1), n.rgb, pa);
	o.Albedo = c.rgb;
	o.Alpha = pa*c.a;
}
ENDCG  
}
CustomEditor "UVFreeMobileShaderGUI"
}