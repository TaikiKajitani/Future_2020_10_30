Shader "Custom/FadeFragment"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Range("_Range", Range(0,1)) = 0.0
	}
	SubShader
	{
			Tags { "Queue" = "Transparent" }
			LOD 200

			Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
				CGPROGRAM
				#include "UnityCG.cginc"

				#pragma vertex vert_img
				#pragma fragment frag
				#pragma target 3.0


				struct Input
				{
					float2 uv_MainTex;
				};

				float _Range;
				fixed4 _Color;


				fixed4 frag(v2f_img i) : SV_Target
				{
					float4 output = _Color;
					if ((i.uv.x - 0.5f)*(i.uv.x - 0.5f) + (i.uv.y - 0.5f)*(i.uv.y - 0.5f) > _Range*0.4f) {
						output.a = 1.0f;
					}
					else {
						output.a = 0.f;
					}
					return output;
				}
			ENDCG
		}
	}	// FallBack "Diffuse"
}