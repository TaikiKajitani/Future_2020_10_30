// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/CookTorrance" {
	Properties{
		_Color("Diffuse Color", Color) = (1,1,1,1)
		_Roughness("Roughness", Range(0,1)) = 0.5
		_FresnelReflectance("Fresnel Reflectance", Range(0,1)) = 0.5
		_Clear("Clear", Range(0,3)) = 0.75
		_BackClear("BackClear", Range(0,3)) = 0.75
		_Power("Power", Range(0,3)) = 1
	}
		SubShader{


		Pass {
		Cull Front
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

		#include "UnityCG.cginc"
		#include "Lighting.cginc"

					uniform float4 _Color;
					uniform float _Roughness;
					uniform float _FresnelReflectance;
					uniform float _BackClear;
					uniform float _Power;

					struct appdata {
						float4 vertex : POSITION;
						float3 normal : NORMAL;
					};

					struct v2f {
						float4 pos : SV_POSITION;
						float3 normal : TEXCOORD1;
						float4 vpos : TEXCOORD2;
					};

					#pragma vertex vert
					#pragma fragment frag

					// D（GGX）の項
					float D_GGX(float3 H, float3 N) {
						float NdotH = saturate(dot(H, N));
						float roughness = saturate(_Roughness);
						float alpha = roughness * roughness;
						float alpha2 = alpha * alpha;
						float t = ((NdotH * NdotH) * (alpha2 - 1.0) + 1.0);
						float PI = 3.1415926535897;
						return min(1.0f, alpha2 / (PI * t * t));
					}

					// フレネルの項
					float Flesnel(float3 V, float3 H) {
						float VdotH = saturate(dot(V, H));
						float F0 = saturate(_FresnelReflectance);
						float F = pow(1.0 - VdotH, 5.0);
						F *= (1.0 - F0);
						F += F0;
						return F;
					}

					// G - 幾何減衰の項（クック トランスモデル）
					float G_CookTorrance(float3 L, float3 V, float3 H, float3 N) {
						float NdotH = saturate(dot(N, H));
						float NdotL = saturate(dot(N, L));
						float NdotV = saturate(dot(N, V));
						float VdotH = saturate(dot(V, H));

						float NH2 = 2.0 * NdotH;
						float g1 = (NH2 * NdotV) / VdotH;
						float g2 = (NH2 * NdotL) / VdotH;
						//float G = max(0.0f,min(1.0, max(g1, g2)));
						float G = min(1.0, min(g1, g2));
						return G;
					}


					v2f vert(appdata v) {
						v2f o;
						o.pos = UnityObjectToClipPos(v.vertex);

						// ワールド空間での法線を計算
						o.normal = normalize(mul(unity_ObjectToWorld, float4(-v.normal, 0.0)).xyz);

						// 該当ピクセルのライティングに、ワールド空間上での位置を保持しておく
						o.vpos = mul(unity_ObjectToWorld, v.vertex);

						return o;
					}

					float4 frag(v2f i) : COLOR{
						// 環境光とマテリアルの色を合算
						float3 ambientLight = unity_AmbientEquator.xyz * _Color.rgb;

						// ワールド空間上のライト位置と法線との内積を計算
						float3 lightDirectionNormal = normalize(_WorldSpaceLightPos0.xyz);
						float NdotL = saturate(dot(i.normal, lightDirectionNormal));

						// ワールド空間上の視点（カメラ）位置と法線との内積を計算
						float3 viewDirectionNormal = normalize((float4(_WorldSpaceCameraPos, 1.0) - i.vpos).xyz);
						float NdotV = saturate(dot(i.normal, viewDirectionNormal));

						float alpha =  1.0f - (abs(dot(lightDirectionNormal, viewDirectionNormal)));

						// ライトと視点ベクトルのハーフベクトルを計算
						float3 halfVector = normalize(lightDirectionNormal + viewDirectionNormal);

						// D_GGXの項
						float D = D_GGX(halfVector, i.normal);

						// Fの項
						float F = Flesnel(viewDirectionNormal, halfVector);

						// Gの項
						float G = G_CookTorrance(lightDirectionNormal, viewDirectionNormal, halfVector, i.normal);

						// スペキュラおよびディフューズを計算
						float specularReflection = (D *F*G) / (4.0 * NdotV * NdotL + 0.000001);
						float3 diffuseReflection = _LightColor0.xyz * _Color.xyz;// *NdotL;

							// 最後に色を合算して出力
						//return  float4(ambientLight + diffuseReflection + specularReflection - (1 - G), _BackClear*(_Power + 1 - G));
						///return  float4(ambientLight + diffuseReflection + specularReflection + (1 - G), (_Power + G)*_BackClear);
						return float4(ambientLight + diffuseReflection + specularReflection - G, (_Power + G * G)*_BackClear);
					}
						ENDCG
			}
			Pass{
			Cull Back
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

						uniform float4 _Color;
						uniform float _Roughness;
						uniform float _FresnelReflectance;
						uniform float _Clear;
						uniform float _Power;

						struct appdata {
							float4 vertex : POSITION;
							float3 normal : NORMAL;
						};

						struct v2f {
							float4 pos : SV_POSITION;
							float3 normal : TEXCOORD1;
							float4 vpos : TEXCOORD2;
						};

						#pragma vertex vert
						#pragma fragment frag

						// D（GGX）の項
						float D_GGX(float3 H, float3 N) {
							float NdotH = saturate(dot(H, N));
							float roughness = saturate(_Roughness);
							float alpha = roughness * roughness;
							float alpha2 = alpha * alpha;
							float t = ((NdotH * NdotH) * (alpha2 - 1.0) + 1.0);
							float PI = 3.1415926535897;
							return min(1.0f, alpha2 / (PI * t * t));
						}

						// フレネルの項
						float Flesnel(float3 V, float3 H) {
							float VdotH = saturate(dot(V, H));
							float F0 = saturate(_FresnelReflectance);
							float F = pow(1.0 - VdotH, 5.0);
							F *= (1.0 - F0);
							F += F0;
							return F;
						}

						// G - 幾何減衰の項（クック トランスモデル）
						float G_CookTorrance(float3 L, float3 V, float3 H, float3 N) {
							float NdotH = saturate(dot(N, H));
							float NdotL = saturate(dot(N, L));
							float NdotV = saturate(dot(N, V));
							float VdotH = saturate(dot(V, H));

							float NH2 = 2.0 * NdotH;
							float g1 = (NH2 * NdotV) / VdotH;
							float g2 = (NH2 * NdotL) / VdotH;
							float G = min(1.0, min(g1, g2));
							return G;
						}


						v2f vert(appdata v) {
							v2f o;
							o.pos = UnityObjectToClipPos(v.vertex);

							// ワールド空間での法線を計算
							o.normal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0.0)).xyz);

							// 該当ピクセルのライティングに、ワールド空間上での位置を保持しておく
							o.vpos = mul(unity_ObjectToWorld, v.vertex);

							return o;
						}

						float4 frag(v2f i) : COLOR{
							// 環境光とマテリアルの色を合算
							float3 ambientLight = unity_AmbientEquator.xyz * _Color.rgb;

							// ワールド空間上のライト位置と法線との内積を計算
							float3 lightDirectionNormal = normalize(_WorldSpaceLightPos0.xyz);
							float NdotL = saturate(dot(i.normal, lightDirectionNormal));

							// ワールド空間上の視点（カメラ）位置と法線との内積を計算
							float3 viewDirectionNormal = normalize((float4(_WorldSpaceCameraPos, 1.0) - i.vpos).xyz);
							float NdotV = saturate(dot(i.normal, viewDirectionNormal));

							float alpha = 1.0f - (abs(dot(lightDirectionNormal, viewDirectionNormal)));

							// ライトと視点ベクトルのハーフベクトルを計算
							float3 halfVector = normalize(lightDirectionNormal + viewDirectionNormal);

							// D_GGXの項
							float D = D_GGX(halfVector, i.normal);

							// Fの項
							float F = Flesnel(viewDirectionNormal, halfVector);

							// Gの項
							float G = G_CookTorrance(lightDirectionNormal, viewDirectionNormal, halfVector, i.normal);

							// スペキュラおよびディフューズを計算
							float specularReflection = (D *F*G) / (4.0 * NdotV * NdotL + 0.000001);
							float3 diffuseReflection = _LightColor0.xyz * _Color.xyz;// *NdotL;

								// 最後に色を合算して出力
							return  float4(ambientLight + diffuseReflection + specularReflection + G,(_Power + G*G)*_Clear);// specularReflection);
						}
							ENDCG
			}
	}
		FallBack "Diffuse"
}