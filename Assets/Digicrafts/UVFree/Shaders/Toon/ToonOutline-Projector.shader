﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Digicrafts/UV-Free/Toon/Outline-Projector"
{
	Properties
	{			
		_MainTex ("_MainTex", 2D) = "white" {}
		_Color ("_Color", Color) = (1,1,1,1)
		// _OutlineColor ("_OutlineColor", Color) = (0,0,0,0)
		// _OutlineWidth ("_OutlineWidth", Range (0, 10)) = 0.5
		[Toggle] _Shade ("Eanble", Float) = 1
		_ShadeTex ("_ShadeTex", CUBE) = "gray" {}
      	_ShadePower ("_ShadePower",Range(0,1)) = 1
      	_ShadeColor ("_ShadeColor", Color) = (1,1,1,1)
		[Toggle] _Shadow ("_Shadow", Float) = 0
		_ShadowPower ("_ShadowPower",Range(0,1)) = 0.2
		[Toggle] _Diffuse ("_Diffuse", Float) = 0
		_DiffusePower ("_DiffusePower",Range(0,1)) = 1
		[Toggle] _Ambient ("_Ambient", Float) = 0
		_AmbientPower ("_AmbientPower",Range(0,1)) = 1

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

		_ProjectorTex ("Projector Texture", 2D) = "white" {}
		_ProjectorFalloffTex ("Falloff Texture", 2D) = "white" {}
	}
	SubShader
	{				
        // base
		Pass
		{
			Name "BASE"
			Tags { "RenderType"="Transparent" "Queue"="Transparent"}
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha 
			Offset -1, -1
			LOD 100

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight alpha
			#pragma multi_compile_fog
			#pragma shader_feature _DIFFUSE_ON
        	#pragma multi_compile _AMBIENT_ON
        	#pragma shader_feature _SHADOW_ON
        	#pragma shader_feature _SHADE_ON
            
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "AutoLight.cginc"

			#pragma shader_feature _UVFREE_UP_NONE _UVFREE_UP_TOP _UVFREE_UP_FRONT _UVFREE_UP_LEFT
			#pragma shader_feature _UVFREE_DOWN_NONE _UVFREE_DOWN_BOTTOM _UVFREE_DOWN_BACK _UVFREE_DOWN_RIGHT
			#pragma shader_feature _UVFREE_WORLDSPACE
			#pragma shader_feature _UVFREE_VERTEX_COLOR
//			#pragma shader_feature _UVFREE_UV_FREE _UVFREE_UV_UV0 _UVFREE_UV_UV1
			#include "../Core/UVFreeCommon.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform fixed4 _Color;

			#if _DIFFUSE_ON
			uniform fixed4 _DiffuseColor;
			uniform float _DiffusePower;
			#endif

			// #if _AMBIENT_ON
			uniform float _AmbientPower;
			// #endif

			#if _SHADOW_ON
			uniform fixed4 _ShadowColor;
			uniform fixed _ShadowPower;
			#endif

			#if _SHADE_ON
			samplerCUBE _ShadeTex;
         	uniform fixed _ShadePower;
         	uniform fixed _SpecToonEffectPower;
         	uniform fixed3 _ShadeColor;
			#endif

			uniform sampler2D _ProjectorTex;
			uniform sampler2D _ProjectorFalloffTex;
			uniform float4x4 unity_Projector;
			uniform float4x4 unity_ProjectorClip;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				DC_UVFREE_INPUT
			};

			struct v2f
			{				
				float4 pos : SV_POSITION;

				#if _DIFFUSE_ON
				fixed3 diff : COLOR1;
				#endif

				// #if _AMBIENT_ON
                fixed3 ambient : COLOR2;
                // #endif

				#if _SHADOW_ON
				SHADOW_COORDS(3)
                #endif

                #if _SHADE_ON
				float3 cubenormal : NORMAL1;
				#endif

				UNITY_FOG_COORDS(5)
				DC_UVFREE_OUPUT(6,7)

				float4 projector : TEXCOORD8;
				float4 projectorFalloff : TEXCOORD9;

			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				DC_UVFREE_TRANSFER(o,_MainTex)

				#if _SHADE_ON
					o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				#endif

				#if _DIFFUSE_ON
					half nl = max(0, dot(UnityObjectToWorldNormal(o.objNormal), _WorldSpaceLightPos0.xyz));
					o.diff = nl * _LightColor0 * _DiffusePower;
				#endif

				// #if _AMBIENT_ON
					o.ambient = ShadeSH9(half4(UnityObjectToWorldNormal(o.objNormal),1)) * _AmbientPower;
				// #endif

				#if _SHADOW_ON
					TRANSFER_SHADOW(o)
				#endif

				UNITY_TRANSFER_FOG(o,o.pos);

				o.projector = mul (unity_Projector, v.vertex);
				o.projectorFalloff = mul (unity_ProjectorClip, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{				
				DC_UVFREE_SETUP(i.objNormal)

				fixed4 projectorColor = tex2Dproj (_ProjectorTex, UNITY_PROJ_COORD(i.projector));
				fixed4 projectorFalloff = tex2Dproj (_ProjectorFalloffTex, UNITY_PROJ_COORD(i.projectorFalloff));				

				fixed4 c = fixed4(1,1,1,1);
				DC_UVFREE_UV_APPLY(i,_MainTex);				
				DC_UVFREE_APPLY_MAIN_TEX(c)
				c*=_Color;

				// Add top/bottom
				#if _UVFREE_UP_LEFT || _UVFREE_DOWN_RIGHT
				if(uv_normal.x>0){
					#if _UVFREE_UP_LEFT
					fixed4 top_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexTop);
					DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
					DC_UVFREE_VALUE_X(c,(top_c*_ColorTop),_PowerTop);
					#endif
				} else {
					#if _UVFREE_DOWN_RIGHT
					fixed4 bottom_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexBottom);
					DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
					DC_UVFREE_VALUE_X(c,(bottom_c*_ColorBottom),_PowerBottom);
					#endif
				}
				#endif

				#if _UVFREE_UP_FRONT || _UVFREE_DOWN_BACK
				if(uv_normal.z>0){
					#if _UVFREE_UP_FRONT
					fixed4 top_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexTop);
					DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
					DC_UVFREE_VALUE_Z(c,(top_c*_ColorTop),_PowerTop);
					#endif
				} else {
					#if _UVFREE_DOWN_BACK
					fixed4 bottom_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexBottom);
					DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
					DC_UVFREE_VALUE_Z(c,(bottom_c*_ColorBottom),_PowerBottom);
					#endif
				}
				#endif

				#if _UVFREE_UP_TOP || _UVFREE_DOWN_BOTTOM
				if(uv_normal.y>0){
					#if _UVFREE_UP_TOP
					fixed4 top_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexTop);
					DC_UVFREE_TEX_APPLY_LOD(top_c,_MainTexTop,_MainTexTop);
					DC_UVFREE_VALUE_Y(c,(top_c*_ColorTop),_PowerTop);
					#endif
				} else {
					#if _UVFREE_DOWN_BOTTOM
					fixed4 bottom_c = fixed4(1,1,1,1);
					DC_UVFREE_UV_APPLY(i,_MainTexBottom);
					DC_UVFREE_TEX_APPLY_LOD(bottom_c,_MainTexBottom,_MainTexBottom);
					DC_UVFREE_VALUE_Y(c,(bottom_c*_ColorBottom),_PowerBottom);
					#endif
				}
				#endif


				#if _SHADOW_ON
					fixed shadow = clamp(SHADOW_ATTENUATION(i)+1-_ShadowPower,0,1);
					#if _SHADOWTOONEFFECT_ON
					shadow = round(shadow*_ShadowToonEffectPower)/_ShadowToonEffectPower;
					#endif
				#else
					fixed shadow = 1;
				#endif
								           
				#if _SHADE_ON
					fixed4 cube = clamp(texCUBE(_ShadeTex, i.cubenormal),(1-_ShadePower),1);
					c.rgb = cube.rgb*c.rgb;//lerp(fixed4(0,0,0,0),_ShadeColor
	            #endif
	            	                            
                #if _DIFFUSE_ON && _AMBIENT_ON
                	c.rgb=c.rgb*i.diff*shadow+i.ambient;					
				#else
					#if _DIFFUSE_ON
						c.rgb*=i.diff*shadow;
					#else 
						#if _AMBIENT_ON
							c.rgb=c.rgb*shadow+i.ambient;
						#else
							c.rgb=c.rgb*shadow;
						#endif
					#endif
				#endif									

				#if _SHADOW_ON
					c.rgb = c.rgb + (1-shadow)*_ShadowColor.rgb*_ShadowPower;
				#endif

				DC_UVFREE_FINAL(i,c)
				UNITY_APPLY_FOG(i.fogCoord,c);
				
				float kkk = projectorColor.a*projectorFalloff.a;
				c = lerp(fixed4(1,1,1,0), c, projectorColor.a*projectorFalloff.a);

				return c;
			}
			ENDCG
		}

		// shadow pass
	    Pass {
	        Name "ShadowCaster"
	        Tags { "LightMode" = "ShadowCaster" }
	       
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"
			 
			struct v2f {
			    V2F_SHADOW_CASTER;
			};
			 
			v2f vert( appdata_base v )
			{
			    v2f o;
			    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
			    return o;
			}
			 
			float4 frag( v2f i ) : SV_Target
			{
			    SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
			 
	    }
	}
CustomEditor "UVFreeToonShaderGUI"
}
