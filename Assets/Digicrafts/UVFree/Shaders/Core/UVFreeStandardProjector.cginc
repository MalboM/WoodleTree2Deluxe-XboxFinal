uniform sampler2D _MainTex;
uniform half4 _MainTex_ST;
uniform fixed4 _Color;

uniform half _Glossiness;
uniform half _GlossinessTop;
uniform half _GlossinessBottom;

uniform float _Top;
uniform float _Bottom;

uniform float _PowerTop;
uniform fixed4 _ColorTop;
uniform sampler2D _MainTexTop;
uniform fixed4 _MainTexTop_ST;
uniform half _ShininessTop;
uniform sampler2D _BumpMapTop;
uniform half _BumpScaleTop;

uniform float _PowerBottom;
uniform fixed4 _ColorBottom;
uniform sampler2D _MainTexBottom;
uniform fixed4 _MainTexBottom_ST;
uniform half _ShininessBottom;
uniform sampler2D _BumpMapBottom;
uniform half _BumpScaleBottom;

uniform sampler2D _ProjectorTex;
uniform sampler2D _ProjectorFalloffTex;
uniform float4x4 unity_Projector;
uniform float4x4 unity_ProjectorClip;

struct Input {
	float4 vertex;
	half3 objNormal;	
	DC_UVFREE_WORLD_POS
	INTERNAL_DATA
};

void vert (inout appdata_full v, out Input o) {
	UNITY_INITIALIZE_OUTPUT(Input,o);
  	DC_UVFREE_TRANSFER(o,_MainTex)
}

void surf (Input i, inout UV_FREE_STANDARD_OUTPUT o) {
	
	DC_UVFREE_SETUP(i.objNormal);		

	fixed4 projectorColor = tex2Dproj (_ProjectorTex, UNITY_PROJ_COORD(mul (unity_Projector, i.vertex)));
	fixed4 projectorFalloff = tex2Dproj (_ProjectorFalloffTex, UNITY_PROJ_COORD(mul (unity_ProjectorClip, i.vertex)));

	fixed4 c = _Color;
	DC_UVFREE_UV_APPLY(i,_MainTex)
	DC_UVFREE_APPLY_MAIN_TEX(c)
	c*=_Color;

	fixed3 n = fixed3(0.5,0.5,1);
	DC_UVFREE_SCALE_NORMAL_APPLY(n,_BumpMap,_MainTex,_BumpScale);

	DC_UVFREE_MS
	DC_UVFREE_OCC

	// X
	if(uv_normal.x>0){		
		if(_Top==3){
			fixed4 top_c = fixed4(1,1,1,1);half ratio;
			DC_UVFREE_UV_APPLY(i,_MainTexTop);
			DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
			DC_UVFREE_LERP_RATIO_X(c,(top_c*_ColorTop),_PowerTop,ratio);

			fixed3 top_n = fixed3(1,1,1);
			DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
			DC_UVFREE_LERP(n,top_n,ratio);

			DC_UVFREE_MS_UP
			DC_UVFREE_OCC_UP
		}
	} else {
		if(_Bottom==3){
			fixed4 bottom_c = fixed4(1,1,1,1);half ratio;
			DC_UVFREE_UV_APPLY(i,_MainTexBottom);
			DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
			DC_UVFREE_LERP_RATIO_X(c,(bottom_c*_ColorBottom),_PowerBottom,ratio);

			fixed3 bottom_n = fixed3(1,1,1);
			DC_UVFREE_SCALE_NORMAL_APPLY(bottom_n,_BumpMapBottom,_MainTexBottom,_BumpScaleBottom);
			DC_UVFREE_LERP(n,bottom_n,ratio);

			DC_UVFREE_MS_DOWN
			DC_UVFREE_OCC_DOWN
		}
	}
	// Z
	if(uv_normal.z>0){
		if(_Top==2){
			fixed4 top_c = fixed4(1,1,1,1);half ratio;
			DC_UVFREE_UV_APPLY(i,_MainTexTop);
			DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
			DC_UVFREE_LERP_RATIO_Z(c,(top_c*_ColorTop),_PowerTop,ratio);

			fixed3 top_n = fixed3(1,1,1);
			DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
			DC_UVFREE_LERP(n,top_n,ratio);

			DC_UVFREE_MS_UP
			DC_UVFREE_OCC_UP
		}
	} else {
		if(_Bottom==2){
			fixed4 bottom_c = fixed4(1,1,1,1);half ratio;
			DC_UVFREE_UV_APPLY(i,_MainTexBottom);
			DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
			DC_UVFREE_LERP_RATIO_Z(c,(bottom_c*_ColorBottom),_PowerBottom,ratio);

			fixed3 bottom_n = fixed3(1,1,1);
			DC_UVFREE_SCALE_NORMAL_APPLY(bottom_n,_BumpMapBottom,_MainTexBottom,_BumpScaleBottom);
			DC_UVFREE_LERP(n,bottom_n,ratio);

			DC_UVFREE_MS_DOWN
			DC_UVFREE_OCC_DOWN
		}		
	}
	// Y
	if(uv_normal.y>0)
	{		
		if(_Top==1){
			fixed4 top_c = _ColorTop;half ratio;
			DC_UVFREE_UV_APPLY(i,_MainTexTop);
			DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
			DC_UVFREE_LERP_RATIO_Y(c,(top_c*_ColorTop),_PowerTop,ratio);

			fixed3 top_n = fixed3(1,1,1);
			DC_UVFREE_SCALE_NORMAL_APPLY(top_n,_BumpMapTop,_MainTexTop,_BumpScaleTop);
			DC_UVFREE_LERP(n,top_n,ratio);
				
			DC_UVFREE_MS_UP
			DC_UVFREE_OCC_UP
		}
	} else {
		if(_Bottom==1){
			fixed4 bottom_c = fixed4(1,1,1,1);
			float ratio=1;
			DC_UVFREE_UV_APPLY(i,_MainTexBottom);
			DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);			
			DC_UVFREE_LERP_RATIO_Y(c,(bottom_c*_ColorBottom),_PowerBottom,ratio);			
	
			fixed3 bottom_n = fixed3(1,1,1);
			DC_UVFREE_SCALE_NORMAL_APPLY(bottom_n,_BumpMapBottom,_MainTexBottom,_BumpScaleBottom);
			DC_UVFREE_LERP(n,bottom_n,ratio);

			DC_UVFREE_MS_DOWN
			DC_UVFREE_OCC_DOWN
		}			
	}
	

	DC_UVFREE_FINAL(i,c.rgb)
	DC_UVFREE_APPLY_MS
	DC_UVFREE_APPLY_OCC
	
	float pa = projectorColor.a*projectorFalloff.a;

	o.Normal = n.rgb;
	o.Albedo = c.rgb;
	o.Alpha = pa*c.a;
}