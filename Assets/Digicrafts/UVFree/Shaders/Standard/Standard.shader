Shader "Digicrafts/UV-Free/Standard/Standard" {
Properties {

	_Color("Color", Color) = (1,1,1,1)
	_MainTex("Albedo", 2D) = "white" {}
	
	_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

	_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
	[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
	_MetallicGlossMap("Metallic", 2D) = "white" {}

	_BumpScale("Scale", Float) = 1.0
	_BumpMap("Normal Map", 2D) = "bump" {}

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

	_GlossinessTop("Smoothness", Range(0.0, 1.0)) = 0.5
	[Gamma] _MetallicTop("Metallic", Range(0.0, 1.0)) = 0.0
	_MetallicGlossMapTop("Metallic", 2D) = "white" {}
	_BumpScaleTop("Scale", Float) = 1.0
	_BumpMapTop ("_BumpMapTop", 2D) = "bump" {}

	_GlossinessBottom("Smoothness", Range(0.0, 1.0)) = 0.5
	[Gamma] _MetallicBottom("Metallic", Range(0.0, 1.0)) = 0.0
	_MetallicGlossMapBottom("Metallic", 2D) = "white" {}
	_BumpScaleBottom("Scale", Float) = 1.0
	_BumpMapBottom ("_BumpMapBottom", 2D) = "bump" {}

//	_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
//	_OcclusionMap("Occlusion", 2D) = "white" {}
//	_OcclusionStrengthTop("Strength", Range(0.0, 1.0)) = 1.0
//	_OcclusionMapTop("Occlusion", 2D) = "white" {}
//	_OcclusionStrengthBottom("Strength", Range(0.0, 1.0)) = 1.0
//	_OcclusionMapBottom("Occlusion", 2D) = "white" {}

	// Blending state
	[HideInInspector] _Mode ("__mode", Float) = 0.0
	[HideInInspector] _SrcBlend ("__src", Float) = 1.0
	[HideInInspector] _DstBlend ("__dst", Float) = 0.0
	[HideInInspector] _ZWrite ("__zw", Float) = 1.0
}
SubShader {
Tags { "RenderType"="Opaque" "PerformanceChecks"="False"  }
LOD 300
Blend [_SrcBlend] [_DstBlend]
ZWrite [_ZWrite]

CGPROGRAM
#pragma surface surf Standard vertex:vert
#pragma target 3.0

#pragma shader_feature _UVFREE_WORLDSPACE
#pragma shader_feature _UVFREE_VERTEX_COLOR

// Standard shader feature variants
#pragma shader_feature _NORMALMAP
#pragma shader_feature _METALLICGLOSSMAP
#pragma shader_feature _METALLICGLOSSMAP_TOP
#pragma shader_feature _METALLICGLOSSMAP_BOTTOM

#include "Assets/Digicrafts/UVFree/Shaders/Core/UVFreeVertexColor.cginc"
#include "Assets/Digicrafts/UVFree/Shaders/Core/UVFreeStandardNoOcc.cginc"
#include "Assets/Digicrafts/UVFree/Shaders/Core/UVFreeMain.cginc"
#include "Assets/Digicrafts/UVFree/Shaders/Core/UVFreeStandardMetallic.cginc"
#include "Assets/Digicrafts/UVFree/Shaders/Core/UVFreeStandard.cginc"


ENDCG  
}
Fallback "Diffuse"
CustomEditor "UVFreeStandardShaderGUI"
}