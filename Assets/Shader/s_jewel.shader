Shader "Custom/s_jewel"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_FrontClear("FrontClear", Range(0,3)) = 0.75
		_BackClear("BackClear", Range(0,3)) = 0.75
		_Power("Power", Range(0,3)) = 1
		_Power2("Power2", Range(0,3)) = 1
	}
		SubShader
		{
			Cull Front
			Tags { "RenderType" = "Fade" }
			LOD 200

				Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			//#pragma surface surf Standard fullforwardshadows
#pragma surface surf Standard alpha

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			//Pass {
			//}
			sampler2D _MainTex;

			struct Input
			{
				float3 worldPos;
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;	
			uniform float _BackClear;
			uniform float _Power;
			uniform float _Power2;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;

				// ワールド空間上の視点（カメラ）位置と法線との内積を計算
				float3 viewDirectionNormal = normalize((float4(_WorldSpaceCameraPos, 1.0) - IN.worldPos).xyz);
				float G = dot(viewDirectionNormal, o.Normal.rgb);
				o.Alpha = (G*G*_Power2 + _Power)*_BackClear;
				//o.Normal *= -1;
			}
			ENDCG

				Cull Back
				Tags{ "RenderType" = "Fade" }
				LOD 200


				CGPROGRAM
				// Physically based Standard lighting model, and enable shadows on all light types
//#pragma surface surf Standard fullforwardshadows
#pragma surface surf Standard alpha

// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

sampler2D _MainTex;

			struct Input
			{
				float3 worldPos;
				float2 uv_MainTex;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			uniform float _FrontClear;
			uniform float _Power;
			uniform float _Power2;
//
			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
				UNITY_INSTANCING_BUFFER_END(Props)

				void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;

				// ワールド空間上の視点（カメラ）位置と法線との内積を計算
				float3 viewDirectionNormal = normalize((float4(_WorldSpaceCameraPos, 1.0) - IN.worldPos).xyz);


				float G = dot(viewDirectionNormal, o.Normal.rgb);
				o.Alpha = (G*G*_Power2 + _Power)*_FrontClear;
			}
			ENDCG
		}
	FallBack "Diffuse"
		
}