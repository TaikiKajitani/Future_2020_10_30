﻿Shader "Unlit/Shader_Refraction_01"
{
	Properties{
			_NormalTex("Normal Tex", 2D) = "bump" {}
			_Distortion("Distortion", Float) = 1
	}

		SubShader{
			Tags {
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}

			GrabPass {}

			CGPROGRAM
				#pragma target 3.0
				#pragma surface surf Standard fullforwardshadows

				sampler2D _GrabTexture;

				sampler2D _NormalTex;
				float _Distortion;

				struct Input {
					float2 uv_NormalTex;
					float4 screenPos;
				};

				void surf(Input IN, inout SurfaceOutputStandard o) {
					float2 grabUV = (IN.screenPos.xy / IN.screenPos.w);

					fixed2 normalTex = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex)).rg;
					grabUV += normalTex * _Distortion;

					fixed3 grab = tex2D(_GrabTexture, grabUV).rgb;

					o.Emission = grab;
					o.Albedo = fixed3(0, 0, 0);
				}
			ENDCG
			}

				FallBack "Transparent/Diffuse"
}