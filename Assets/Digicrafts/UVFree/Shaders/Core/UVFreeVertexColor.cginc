#if _UVFREE_VERTEX_COLOR
	uniform half _VertexColorPower;
	#define DC_UVFREE_VCOLOR fixed4 v_color;
	#define DC_UVFREE_INPUT \
		fixed4 color : COLOR;
	#define DC_UVFREE_OUPUT(id1,id2)\
		float4 vertex : VERTEX;\
		_DC_UVFREE_WORLD_POS\
		half3 objNormal : NORMAL;\
		fixed4 v_color : COLOR;
	#define DC_UVFREE_TRANSFER(output,tex)\
		output.vertex = v.vertex;\
		_DC_UVFREE_WORLD_POS_TRANSFER(output)\
		output.objNormal = v.normal;\
		output.v_color=lerp(fixed4(1,1,1,1),v.color,_VertexColorPower);			
	#define DC_UNFREE_VERTEX_ALPHA(input_color) input_color.a*=i.v_color.a;
	#define DC_UVFREE_FINAL(input,col) col.rgb*=input.v_color.rgb;
#else
	#define DC_UVFREE_VCOLOR
	#define DC_UVFREE_INPUT
	#define DC_UVFREE_OUPUT(id1,id2)\
		float4 vertex : VERTEX;\
		_DC_UVFREE_WORLD_POS\
		half3 objNormal : NORMAL;
	#define DC_UVFREE_TRANSFER(output,tex)\
		output.vertex = v.vertex;\
		_DC_UVFREE_WORLD_POS_TRANSFER(output)\
		output.objNormal = v.normal;			
	#define DC_UNFREE_VERTEX_ALPHA
	#define DC_UVFREE_FINAL(input,col)
#endif
