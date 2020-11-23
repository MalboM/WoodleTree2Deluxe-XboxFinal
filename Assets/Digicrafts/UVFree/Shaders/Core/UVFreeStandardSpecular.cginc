//uniform half3 _SpecColor;
uniform sampler2D _SpecGlossMap;
uniform half3 _SpecColorTop;
uniform sampler2D _SpecGlossMapTop;
uniform half3 _SpecColorBottom;
uniform sampler2D _SpecGlossMapBottom;

#define UV_FREE_STANDARD_OUTPUT SurfaceOutputStandardSpecular

#if _SPECGLOSSMAP
	#define DC_UVFREE_MS\
		half4 sg = fixed4(_SpecColor.rgb, _Glossiness);\
		DC_UVFREE_TEX_APPLY(sg,_SpecGlossMap,_MainTex)

#else
	#define DC_UVFREE_MS half4 sg = fixed4(_SpecColor.rgb, _Glossiness);
#endif

#if _SPECGLOSSMAP_TOP
	#define DC_UVFREE_MS_UP\
		half4 top_sg = half4(_SpecColorTop.rgb, _GlossinessTop);\
		DC_UVFREE_TEX_APPLY_LOD(top_sg,_SpecGlossMapTop,_MainTexTop)\
		DC_UVFREE_LERP(sg,top_sg,ratio);
#else
	#define DC_UVFREE_MS_UP\
		half4 top_sg = half4(_SpecColorTop.rgb, _GlossinessTop);\
		DC_UVFREE_LERP(sg,top_sg,ratio);
#endif

#if _SPECGLOSSMAP_BOTTOM
	#define DC_UVFREE_MS_DOWN\
		half4 bottom_sg = half4(_SpecColorBottom.rgb, _GlossinessBottom);\
		DC_UVFREE_TEX_APPLY_LOD(bottom_sg,_SpecGlossMapBottom,_MainTexBottom)\
		DC_UVFREE_LERP(sg,bottom_sg,ratio);

#else
	#define DC_UVFREE_MS_DOWN\
		half4 bottom_sg = half4(_SpecColorBottom.rgb, _GlossinessBottom);\
		DC_UVFREE_LERP(sg,bottom_sg,ratio);

#endif

#define DC_UVFREE_APPLY_MS\
	o.Specular = sg.rgb;\
	o.Smoothness = sg.a;