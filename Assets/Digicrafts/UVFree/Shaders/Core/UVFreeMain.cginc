// Fix for Unity 5.4 upgrade
//UNITY_SHADER_NO_UPGRADE
#if UNITY_VERSION >= 540
	#define _Object2World unity_ObjectToWorld
	// #define _World2Object unity_WorldToObject
#endif

uniform int _UVSecMain;
uniform half _Shininess;
uniform sampler2D _BumpMap;
uniform half _BumpScale;

#if _UVFREE_UP_TOP || _UVFREE_UP_FRONT || _UVFREE_UP_LEFT
uniform float _PowerTop;
uniform fixed4 _ColorTop;
uniform sampler2D _MainTexTop;
uniform fixed4 _MainTexTop_ST;
uniform half _ShininessTop;
uniform sampler2D _BumpMapTop;
uniform half _BumpScaleTop;
#endif

#if _UVFREE_DOWN_BOTTOM || _UVFREE_DOWN_BACK || _UVFREE_DOWN_RIGHT
uniform float _PowerBottom;
uniform fixed4 _ColorBottom;
uniform sampler2D _MainTexBottom;
uniform fixed4 _MainTexBottom_ST;
uniform half _ShininessBottom;
uniform sampler2D _BumpMapBottom;
uniform half _BumpScaleBottom;
#endif

#if _UVFREE_WORLDSPACE
	#define DC_UVFREE_WORLD_POS float3 worldPos;
	#define _DC_UVFREE_WORLD_POS float3 worldPos : COLOR3;
	#define _DC_UVFREE_WORLD_POS_TRANSFER(output) output.worldPos = mul(_Object2World, v.vertex.xyz);
	#define DC_UVFREE_SETUP(input_normal)\
		half3 uv_normal = UnityObjectToWorldNormal(input_normal);\
		half3 blend = abs(uv_normal);\
		blend /= dot(blend,1.0);
	#define DC_UVFREE_UV_APPLY(input,tex)\
		_DC_UVFREE_UV_APPLY(i.worldPos,tex);	

	#define _DC_UVFREE_UV_APPLY(pos,tex)\
		float2 tex##_uv_raw_x = pos.xy*tex##_ST.xy+tex##_ST.zw;\
		float2 tex##_uv_raw_y = pos.xz*tex##_ST.xy+tex##_ST.zw;\
		float2 tex##_uv_raw_z = pos.zy*tex##_ST.xy+tex##_ST.zw;\
		float4 tex##_uv_x = float4(tex##_uv_raw_x,4,0);\
		float4 tex##_uv_y = float4(tex##_uv_raw_y,4,0);\
		float4 tex##_uv_z = float4(tex##_uv_raw_z,4,0);\
		float lz = clamp((blend.z-0.45)*10,0,1);\
		float ly = clamp((blend.y-0.45)*10,0,1);
#else
	#define DC_UVFREE_WORLD_POS
	#define _DC_UVFREE_WORLD_POS
	#define _DC_UVFREE_WORLD_POS_TRANSFER(output)
	#define DC_UVFREE_SETUP(input_normal)\
		half3 uv_normal = normalize(input_normal);\
		half3 blend = abs(uv_normal);\
		blend /= dot(blend,1.0);	
	#define DC_UVFREE_UV_APPLY(input,tex)\
		_DC_UVFREE_UV_APPLY(input.vertex,tex);
	
	#define _DC_UVFREE_UV_APPLY(pos,tex)\
		float2 tex##_uv_raw_x = pos.xy*tex##_ST.xy+tex##_ST.zw;\
		float2 tex##_uv_raw_y = pos.xz*tex##_ST.xy+tex##_ST.zw;\
		float2 tex##_uv_raw_z = pos.zy*tex##_ST.xy+tex##_ST.zw;\
		float4 tex##_uv_x = float4(tex##_uv_raw_x,4,0);\
		float4 tex##_uv_y = float4(tex##_uv_raw_y,4,0);\
		float4 tex##_uv_z = float4(tex##_uv_raw_z,4,0);\
		float lz = clamp((blend.z-0.45)*10,0,1);\
		float ly = clamp((blend.y-0.45)*10,0,1);
#endif			

#define DC_UVFREE_APPLY_MAIN_TEX(col) DC_UVFREE_TEX_APPLY(col,_MainTex,_MainTex);

#define DC_UVFREE_TEX_APPLY_LOD(col,tex,uv)\
	fixed4 tex##_z = tex2Dlod(tex, uv##_uv_z);\
	fixed4 tex##_x = tex2Dlod(tex, uv##_uv_x);\
	fixed4 tex##_y = tex2Dlod(tex, uv##_uv_y);\
	col = lerp(tex##_z,tex##_x,lz);\
	col = lerp(col,tex##_y,ly);DC_UNFREE_VERTEX_ALPHA(col);

#define DC_UVFREE_TEX_APPLY(col,tex,uv)\
	fixed4 tex##_z = tex2D(tex, uv##_uv_raw_z);\
	fixed4 tex##_x = tex2D(tex, uv##_uv_raw_x);\
	fixed4 tex##_y = tex2D(tex, uv##_uv_raw_y);\
	col = lerp(tex##_z,tex##_x,lz);\
	col = lerp(col,tex##_y,ly);

//#define DC_UVFREE_NORMAL_APPLY(col,tex,bump)\
//	fixed3 bump##_z = UnpackNormal(tex2Dlod(bump, tex##_uv_z));\
//	fixed3 bump##_x = UnpackNormal(tex2Dlod(bump, tex##_uv_x));\
//	fixed3 bump##_y = UnpackNormal(tex2Dlod(bump, tex##_uv_y));\
//	col = lerp(bump##_z,bump##_x,lz);\
//	col = lerp(col,bump##_y,ly);

#define DC_UVFREE_SCALE_NORMAL_APPLY(col,tex,uv,scale)\
	half3 tex##_n_z = UnpackScaleNormal(tex2Dlod(tex, uv##_uv_z),scale);\
	half3 tex##_n_x = UnpackScaleNormal(tex2Dlod(tex, uv##_uv_x),scale);\
	half3 tex##_n_y = UnpackScaleNormal(tex2Dlod(tex, uv##_uv_y),scale);\
	col = lerp(tex##_n_z,tex##_n_x,lz);\
	col = lerp(col,tex##_n_y,ly);

#define DC_UVFREE_METALLICGLOSS_APPLY(col,tex,uv)\
	half2 tex##_m_z = tex2Dlod(tex, uv##_uv_z).ra;\
	half2 tex##_m_x = tex2Dlod(tex, uv##_uv_x).ra;\
	half2 tex##_m_y = tex2Dlod(tex, uv##_uv_y).ra;\
	col = lerp(tex##_m_z,tex##_m_x,lz);\
	col = lerp(col,tex##_m_y,ly);

#define DC_UVFREE_OCC_APPLY(col,tex,uv)\
	half tex##_o_z = tex2Dlod(tex, uv##_uv_z).r;\
	half tex##_o_x = tex2Dlod(tex, uv##_uv_x).r;\
	half tex##_o_y = tex2Dlod(tex, uv##_uv_y).r;\
	col = lerp(tex##_o_z,tex##_o_x,lz);\
	col = lerp(col,tex##_o_y,ly);

#define _DC_UVFREE_POWER(b,power) clamp((b-((1-power)+0.1)/1.1)*5,0,1)

// Y
#define DC_UVFREE_VALUE_Y(col,input_col,power)\
	col = lerp(col,input_col,_DC_UVFREE_POWER(blend.y,(power*input_col.a)));

#define DC_UVFREE_LERP_RATIO_Y(col,input_col,power,r)\
	r = _DC_UVFREE_POWER(blend.y,(power*input_col.a));\
	col = lerp(col,input_col,r);

// X
#define DC_UVFREE_VALUE_X(col,input_col,power)\
	col = lerp(col,input_col,_DC_UVFREE_POWER(blend.x,(power*input_col.a)));

#define DC_UVFREE_LERP_RATIO_X(col,input_col,power,r)\
	r = _DC_UVFREE_POWER(blend.x,(power*input_col.a));\
	col = lerp(col,input_col,r);

// Z
#define DC_UVFREE_VALUE_Z(col,input_col,power)\
	col = lerp(col,input_col,_DC_UVFREE_POWER(blend.z,(power*input_col.a)));

#define DC_UVFREE_LERP_RATIO_Z(col,input_col,power,r)\
	r = _DC_UVFREE_POWER(blend.z,(power*input_col.a));\
	col = lerp(col,input_col,r);

#define DC_UVFREE_LERP(col,input_col,r) col = lerp(col,input_col,r);