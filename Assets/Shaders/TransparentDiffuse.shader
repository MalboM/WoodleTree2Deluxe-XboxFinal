// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Wipe" {

	// Editor controllers
	Properties{
		_tex0("Texture1", 2D) = "white" {}
	_tex1("Texture2", 2D) = "white" {}
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		CGPROGRAM
#pragma target 3.0
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		sampler2D _tex0;
	sampler2D _tex1;
	float4 _tex0_ST;

	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR0;
		float4 fragPos : COLOR1;
		float2  uv : TEXCOORD0;
	};

	v2f vert(appdata_base v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.fragPos = o.pos;
		o.uv = TRANSFORM_TEX(v.texcoord, _tex0);
		o.color = float4 (1.0, 1.0, 1.0, 1);
		return o;
	}

	half4 frag(v2f i) : COLOR{
		float4 oricol = tex2D(_tex0, i.uv);
		float4 col = tex2D(_tex1, i.uv);
		float  animtime = _Time * 10.0;
		float  comp = smoothstep(0.2, 0.7, sin(animtime));
		float  coeff = clamp(-2.0 + 2.0 * i.uv.x + 3.0 * comp, 0.0, 1.0);
		float4 result = lerp(col, oricol, coeff);

		return result;
	}
		ENDCG
	}
	}
		//FallBack "VertexLit"
}