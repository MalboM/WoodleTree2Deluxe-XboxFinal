uniform half _OcclusionStrength;
uniform sampler2D _OcclusionMap;
uniform half _OcclusionStrengthTop;
uniform sampler2D _OcclusionMapTop;
uniform half _OcclusionStrengthBottom;
uniform sampler2D _OcclusionMapBottom;

#define DC_UVFREE_OCC\
	half oc = 1;\
	DC_UVFREE_OCC_APPLY(oc,_OcclusionMap,_MainTex)\
	oc=lerp(1,oc,_OcclusionStrength);

#define DC_UVFREE_OCC_UP\
		half top_occ = 1;\
		DC_UVFREE_OCC_APPLY(top_occ,_OcclusionMapTop,_MainTexTop)\
		top_occ=lerp(1,top_occ,_OcclusionStrengthTop);\
		DC_UVFREE_LERP(oc,top_occ,ratio);

#define DC_UVFREE_OCC_DOWN\
		half bottom_occ = 1;\
		DC_UVFREE_OCC_APPLY(bottom_occ,_OcclusionMapBottom,_MainTexBottom);\
		bottom_occ=lerp(1,bottom_occ,_OcclusionStrengthBottom);\
		DC_UVFREE_LERP(oc,bottom_occ,ratio);

#define DC_UVFREE_APPLY_OCC o.Occlusion=oc;