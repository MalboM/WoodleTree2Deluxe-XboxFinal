uniform half _Metallic;
uniform sampler2D _MetallicGlossMap;
uniform half _MetallicTop;
uniform sampler2D _MetallicGlossMapTop;
uniform half _MetallicBottom;
uniform sampler2D _MetallicGlossMapBottom;

#define UV_FREE_STANDARD_OUTPUT SurfaceOutputStandard

#if _METALLICGLOSSMAP
	#define DC_UVFREE_MS\
		fixed2 mg = half2(_Metallic, _Glossiness);\
		DC_UVFREE_METALLICGLOSS_APPLY(mg,_MetallicGlossMap,_MainTex)
#else
	#define DC_UVFREE_MS fixed2 mg = half2(_Metallic, _Glossiness);
#endif

#if _METALLICGLOSSMAP_TOP
	#define DC_UVFREE_MS_UP\
		half2 top_mg = half2(_MetallicTop, _GlossinessTop);\
		DC_UVFREE_METALLICGLOSS_APPLY(top_mg,_MetallicGlossMapTop,_MainTexTop)\
		DC_UVFREE_LERP(mg,top_mg,ratio)
#else
	#define DC_UVFREE_MS_UP\
		half2 top_mg = half2(_MetallicTop, _GlossinessTop);\
		DC_UVFREE_LERP(mg,top_mg,ratio)
#endif

#if _METALLICGLOSSMAP_BOTTOM
	#define DC_UVFREE_MS_DOWN\
		half2 bottom_mg = half2(_MetallicBottom, _GlossinessBottom);\
		DC_UVFREE_METALLICGLOSS_APPLY(bottom_mg,_MetallicGlossMapBottom,_MainTexBottom)\
		DC_UVFREE_LERP(mg,bottom_mg,ratio)
#else
	#define DC_UVFREE_MS_DOWN\
		half2 bottom_mg = half2(_MetallicBottom, _GlossinessBottom);\
		DC_UVFREE_LERP(mg,bottom_mg,ratio)
#endif

#define DC_UVFREE_APPLY_MS\
	o.Metallic = mg.x;\
	o.Smoothness = mg.y;