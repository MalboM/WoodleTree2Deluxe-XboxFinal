

Shader "WoodleTree/Special/SandSnow/SnowFakeAOV1"
{
	Properties
	{ 
		[Header(Texture Settings)]
		[NoScaleOffset]
		_BaseNormalTex ("Base Normal Tex", 2D) = "Color(0.5,0.5,1,1)" {}
		[KeywordEnum(No, Yes)] _DetailNormalFlag("Use Detail Normal", Float) = 0.0
		[NoScaleOffset]
		_DetailNormalTex("Detail Normal Tex", 2D) = "gray" {}
		_BaseTileF ("Base Tiling", Range(0.05, 20)) = 1
		_DetailTileF("Detail Tiling", Range(1, 10)) = 2
		_AttenuationF ("Normal Strength", Range(0.0, 2.0)) = 0.5
		[Header(Diffusive settings)]
		_OcclusionPowF("Occlusion pow", Range(0.1,5.)) = 1.78
		_TintColor ("Tint Color", Color) = (0.5, 0.5, 0.5, 1.0)
		//[KeywordEnum(Ground, Equator, Sky, Skybox, None)] _Position("Ambient Light Source", Float) = 0.0
		[Header(Rimlight Settings)]
		_RimColor("Rimlight Color", Color) = (0.8, 0.9, 0.6, 1.0)
		_RimSharpnessF("Rimlight Sharpness", Range(0.0, 4.0)) = 1.0
		_RimIntensityF("Rimlight Intensity", Range(0.0, 5.0)) = 1.0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog
			// #pragma multi_compile _POSITION_NONE _POSITION_GROUND _POSITION_EQUATOR _POSITION_SKY _POSITION_SKYBOX
			#pragma multi_compile _DETAILNORMALFLAG_NO _DETAILNORMALFLAG_YES
			#include "UnityCG.cginc"

			struct VertexData
			{
			    float4 objVertex : POSITION;
			    float2 uv : TEXCOORD0;
			    float3 objNormal : NORMAL;
			    float4 objTangent : TANGENT;
			};

			struct Varys
			{
				float4 pos : SV_POSITION;
				float3 worldPos : TEXCOORD0;			   
				float4 worldNormal : NORMAL;      //.w component used to transfer direction.x factor
				float4 worldTangent : TANGENT;      //.w component used to transfer direction.x factor
				float4 worldBitangent : TEXCOORD2; //.w component used to transfer direction.x factor
				float4 objPos : TEXCOORD3;
				UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;  //alpha channel used for sparkle texture
			sampler2D _BaseNormalTex;
			sampler2D _DetailNormalTex;
			
			fixed4 _TintColor;
			fixed4 _RimColor;
			float _RimSharpnessF;
			float _RimIntensityF;
			float _BaseTileF;
			float _DetailTileF;
			float _AttenuationF;
			float _OcclusionPowF;

			//  taken from http://blog.selfshadow.com/publications/blending-in-detail/, 
			//  reorient detail normal map to follow the base normal map direction
			
			inline float3 ReorientNormalMap(float3 baseNormal, float3 detailNormal)
			{
				float3 t = 2.*baseNormal + float3(-1., -1., 0);
				float3 u = -2. * detailNormal  + float3(1., 1., -1.);
				return normalize(t * dot(t, u) - u * t.z);	
			}

			inline float Intensity(float3 normal){

				return 0.299*normal.x + 0.587*normal.y + 0.114*normal.z;
			}


			Varys vert(VertexData v)
			{
				Varys o;

				o.pos = UnityObjectToClipPos(v.objVertex);
				o.objPos = v.objVertex;

                
				o.worldPos = mul(unity_ObjectToWorld, v.objVertex).xyz;
                
                float3 worldSpaceForward = normalize(mul(float3(0,0,1),UNITY_MATRIX_IT_MV));
                
				o.worldNormal = float4(UnityObjectToWorldNormal(v.objNormal),worldSpaceForward.x);
				o.worldTangent = float4(UnityObjectToWorldDir(v.objTangent), worldSpaceForward.y);
				o.worldBitangent = float4(cross(o.worldNormal.xyz, o.worldTangent.xyz), worldSpaceForward.z);

				UNITY_TRANSFER_FOG(o, o.position);

				return o;
			}



			fixed4 frag(Varys i) : SV_Target
			{


				// compute triplanar texture 

				half3 powNormal = abs(pow(i.worldNormal,4.));

                half3 blendWeights = powNormal/dot(1., powNormal);
                blendWeights = max(blendWeights - 0.1, 0);				

				float3x3 tangent2WorldTranspose = float3x3(
					i.worldTangent.xyz,
					i.worldBitangent.xyz,
					i.worldNormal.xyz
					);


				half4 baseNormalCol = tex2D(_BaseNormalTex, i.worldPos.yz * _BaseTileF) * blendWeights.x +
									  tex2D(_BaseNormalTex, i.worldPos.xz *  _BaseTileF) *blendWeights.y +
									  tex2D(_BaseNormalTex, i.worldPos.xy * _BaseTileF) * blendWeights.z ;				
				
		
            	half3 tangentNormal = UnpackNormal(baseNormalCol);
            	
            	
            	tangentNormal = normalize(tangentNormal);


            #ifdef _DETAILNORMALFLAG_YES



				half4 detailNormalCol = tex2D(_DetailNormalTex, i.worldPos.yz *_DetailTileF ) *  blendWeights.x +
										tex2D(_DetailNormalTex, i.worldPos.xz * _DetailTileF) *  blendWeights.y +
										tex2D(_DetailNormalTex, i.worldPos.xy * _DetailTileF ) *  blendWeights.z;


				detailNormalCol = half4(detailNormalCol.xy, detailNormalCol.zw);

				tangentNormal = ReorientNormalMap(baseNormalCol, detailNormalCol);
				
				half3 worldNormal = normalize(mul(tangentNormal, tangent2WorldTranspose)); 



            #else

				// convert normal from tangent space to world space

				half3 worldNormal = normalize(mul(tangentNormal, tangent2WorldTranspose));                
                
            #endif


				tangentNormal = half3(_AttenuationF*tangentNormal.xy,- tangentNormal.z);
				
				// compute rim factor

				float3 viewSpaceNormal = mul((float3x3)unity_MatrixV,worldNormal);

				float rimScale = clamp(_RimSharpnessF*(dot(viewSpaceNormal, float3(0,0,-1))),0,1);
				
				// calculate diffusive color component

				float occlusion = pow(Intensity(0.5 + tangentNormal*0.5),_OcclusionPowF);
 				float lightDiffuse = (1. - occlusion);
				float3 diffuseColor = lightDiffuse*_TintColor*2.;

				// compute interpolation factor

				float s = clamp((1 -lightDiffuse)*(1. - rimScale)*_RimIntensityF,0,1);

				fixed4 outputCol = fixed4(diffuseColor,1.);
		
				outputCol = lerp(outputCol, _RimColor, s);

				UNITY_APPLY_FOG(i.fogCoord, outputCol);
            

			    return outputCol;
			}
			
			ENDCG
		}
	}
}
