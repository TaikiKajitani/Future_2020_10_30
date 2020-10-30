Shader "Unlit/s_water"
{
	Properties{
			_Color("Color", Color) = (1,1,1,1)
			_MainTex("Water Texture", 2D) = "white" {}
	_Normal("Water Normal", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Fade" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard alpha:fade
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _Normal;
			fixed4 _Color;

			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
				UNITY_DEFINE_INSTANCED_PROP(float, _Alpha)
				UNITY_INSTANCING_BUFFER_END(Props)

			struct Input {
				float2 uv_MainTex;
				float3 viewDir;
			};

			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed2 uv = IN.uv_MainTex;
				/*uv.x += 0.1 * _Time;
				uv.y += 0.2 * _Time;*/
				float3 N = tex2D(_Normal, uv).rgb;
				N = N * 2 - 1;
				N = normalize(N);
				float3 L = normalize(_WorldSpaceLightPos0.rgb);
				float4 T = tex2D(_MainTex, uv);
				o.Albedo.rgb = T.r*_Color.rgb;
				o.Alpha = max(0, dot(L, N)*T.a*_Color.a*_Alpha);
			}
			ENDCG
	}
		FallBack "Diffuse"
}