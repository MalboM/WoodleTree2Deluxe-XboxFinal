Shader "Polymole/Particles/BaseParticlesShader"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
  }
  SubShader
  {
    Tags { "RenderType"="Transparent" "Queue"="Transparent" }
    LOD 100
    Blend SrcAlpha OneMinusSrcAlpha
    Zwrite Off
    Cull Off

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      // make fog work
      #pragma multi_compile_fog

      #include "UnityCG.cginc"

      struct VertexData
      {
        float4 objPos : POSITION;
        float2 uv : TEXCOORD0;
        float4 color : COLOR;
      };

      struct Varys
      {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;
        UNITY_FOG_COORDS(1)
        float4 color : COLOR;
      };


      sampler2D _MainTex;
      float4 _MainTex_ST;


      Varys vert (VertexData v)
      {
        Varys o;
        o.pos = UnityObjectToClipPos(v.objPos);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        UNITY_TRANSFER_FOG(o,o.pos);
        o.color = v.color;
        return o;
      }



      fixed4 frag (Varys i) : SV_Target
      {
        // sample the texture
        fixed4 col = tex2D(_MainTex, i.uv) * i.color;
        // apply fog
        UNITY_APPLY_FOG(i.fogCoord, col);
        return col;
      }
      ENDCG
    }
  }
}
