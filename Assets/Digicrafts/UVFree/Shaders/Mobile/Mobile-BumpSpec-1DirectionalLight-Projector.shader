// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Digicrafts/UV-Free/Mobile/Projector/Bumped Specular (1 Directional Light)" {
Properties {
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
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

	_ShininessTop ("_ShininessTop", Range (0.03, 1)) = 0.078125
	_ShininessBottom ("_ShininessBottom", Range (0.03, 1)) = 0.078125

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
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview novertexlights vertex:vert alpha
#pragma target 3.0

#pragma shader_feature _UVFREE_UP_NONE _UVFREE_UP_TOP _UVFREE_UP_FRONT _UVFREE_UP_LEFT
#pragma shader_feature _UVFREE_DOWN_NONE _UVFREE_DOWN_BOTTOM _UVFREE_DOWN_BACK _UVFREE_DOWN_RIGHT
#pragma shader_feature _UVFREE_WORLDSPACE
#pragma shader_feature _UVFREE_VERTEX_COLOR
#include "../Core/UVFreeCommon.cginc"

uniform sampler2D _MainTex;
uniform fixed4 _MainTex_ST;
uniform fixed4 _Color;

uniform sampler2D _ProjectorTex;
uniform sampler2D _ProjectorFalloffTex;
uniform float4x4 unity_Projector;
uniform float4x4 unity_ProjectorClip;

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
	c.a=s.Alpha;
	return c;
}

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

void surf (Input i, inout SurfaceOutput o) {

	DC_UVFREE_SETUP(i.objNormal)

	fixed4 projectorColor = tex2Dproj (_ProjectorTex, UNITY_PROJ_COORD(mul (unity_Projector, i.vertex)));
	fixed4 projectorFalloff = tex2Dproj (_ProjectorFalloffTex, UNITY_PROJ_COORD(mul (unity_ProjectorClip, i.vertex)));

	fixed4 c = fixed4(1,1,1,1);
	DC_UVFREE_UV_APPLY(i,_MainTex);
	DC_UVFREE_TEX_APPLY(c,_MainTex,_MainTex);
	c*=_Color;

	fixed3 n = fixed3(1,1,1);
	DC_UVFREE_SCALE_NORMAL_APPLY(n,_BumpMap,_MainTex,_BumpScale);

	o.Specular = _Shininess;

// Add top/bottom
	#if _UVFREE_UP_LEFT || _UVFREE_DOWN_RIGHT
	if(uv_normal.x>0){
		#if _UVFREE_UP_LEFT
		fixed4 top_c = fixed4(1,1,1,1);half ratio;
		DC_UVFREE_UV_APPLY(i,_MainTexTop);
		DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
		DC_UVFREE_LERP_RATIO_X(c,(top_c*_ColorTop),_PowerTop,ratio);

		fixed3 top_n = fixed3(1,1,1);
		DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
		DC_UVFREE_LERP(n,top_n,ratio);
		DC_UVFREE_LERP(o.Specular,_ShininessTop,ratio);

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
		DC_UVFREE_LERP(o.Specular,_ShininessBottom,ratio);

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
		DC_UVFREE_LERP(o.Specular,_ShininessTop,ratio);

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
		DC_UVFREE_LERP(o.Specular,_ShininessBottom,ratio);

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
		DC_UVFREE_LERP(o.Specular,_ShininessTop,ratio);

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
		DC_UVFREE_LERP(o.Specular,_ShininessBottom,ratio);

		#endif
	}
	#endif

	DC_UVFREE_FINAL(i,c.rgb)

	float pa = projectorColor.a*projectorFalloff.a;	

	o.Albedo = c.rgb;
	o.Gloss = c.a;
	o.Alpha = pa*c.a;
	o.Normal = n.rgb;
}
ENDCG
}
FallBack "Mobile/VertexLit"
CustomEditor "UVFreeMobileShaderGUI"
}