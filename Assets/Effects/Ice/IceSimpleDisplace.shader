Shader "Polymole/Ice/IceSimpleDisplace"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
        _Contrast("Contrast", Range(0.0, 10)) = 1
        
        [Header(Rim Effect)]
        [Toggle(RIM)] _RIM("Activate Rim", Float) = 0        
        _RimColor("Rimlight Color", Color) = (0.1, 0.1, 0.9, 1.0)
        _RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
        _RimIntensityF("Rimlight Intensity", Range(0.0, 1.0)) = 1.0        
        
        [Header(Displacement)]
        [NoScaleOffset]
        _DisplaceTexture ("Displacement Texture (Normal)", 2D) = "bump" {}
        _DisplacementPower ("Displacement Power" , Range(0.0, 2.0)) = 1.0
        
        [Header(Alpha Blending)]
        [KeywordEnum(None, Add, Multiply, Overlay)] _Blend ("Blending mode", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        
        Cull Off
        //ZWrite Off
        Lighting Off
        Blend Off // implement alpha blending in our shader!!
        //Blend SrcAlpha OneMinusSrcAlpha
        
        GrabPass
        {
            "_BackgroundTexture"
        }        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // fog
            #pragma multi_compile_fog
            #pragma multi_compile _BLEND_NONE _BLEND_ADD _BLEND_MULTIPLY _BLEND_OVERLAY
            #pragma shader_feature RIM

            #include "UnityCG.cginc"
            //#include "Assets/Shaders/Libs/ColorMap.cginc"            

            struct VertexData
            {
                float4 objPos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;                
            };

            struct Varys
            {
                float3 uv : TEXCOORD0;
                float4 clpPos : SV_POSITION;
                float4 grabPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            
            // main texture
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed4 _TintColor;
            
            uniform float _Contrast;
            
            // displacement uniform
            uniform sampler2D _DisplaceTexture;
            uniform float _DisplacementPower;
            
            // grab pass uniform
            uniform sampler2D _BackgroundTexture;
            uniform float4 _BackgroundTexture_TexelSize;
            
            // rim uniform
            uniform fixed4 _RimColor;
            uniform fixed _RimSharpnessF;
            uniform fixed _RimIntensityF;
                        
            Varys vert (VertexData v)
            {
                Varys o;
                o.clpPos = UnityObjectToClipPos(v.objPos);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                
                #ifdef RIM
                o.uv.z = clamp(dot(normalize(ObjSpaceViewDir(v.objPos)), v.normal) * _RimSharpnessF, 0.0, 1.0);
                #else 
                o.uv.z = 0.0;
                #endif
                
                UNITY_TRANSFER_FOG(o,o.clpPos);                
                o.grabPos = ComputeGrabScreenPos(o.clpPos);
                
                return o;
            }
           
            fixed4 frag (Varys i) : SV_Target
            {                
                fixed4 displPos = tex2D(_DisplaceTexture, i.uv)*2 - 1;
                fixed4 offset = displPos * _DisplacementPower;
                //i.grabPos.xy = offset * i.grabPos.z + i.grabPos.xy;
                fixed4 texColor = tex2D(_MainTex, i.uv) * _TintColor;
                
                half3 grabColor = tex2Dproj(_BackgroundTexture, i.grabPos + offset).rgb;
                fixed3 color;
                fixed s = step(1.0, texColor.a);
                                
                #ifdef _BLEND_NONE
                color = texColor;
                #elif _BLEND_ADD
                color = texColor.rgb + grabColor*(1 - texColor.a);
                #elif _BLEND_OVERLAY
                color = grabColor < 0.5 ? 2.*grabColor*texColor : 1.-2*(1-texColor)*(1-grabColor);
                //color = s*texColor.rgb + (1-s)*color;
                #elif _BLEND_MULTIPLY
                color = grabColor*texColor;
                color = s*texColor.rgb + (1-s)*color;
                #endif
                
                color = pow(color, _Contrast);
                
                #ifdef RIM
                color = lerp(color, _RimColor, (1. - i.uv.z)*_RimIntensityF);
                #endif
                
                //return heatmapSimple(0,1,offset);
                UNITY_APPLY_FOG(i.fogCoord, color);                
                return float4(color, texColor.a);
            }
            ENDCG
        }
    }
}
