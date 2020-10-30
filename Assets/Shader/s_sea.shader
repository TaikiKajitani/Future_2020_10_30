Shader "Unlit/s_sea"
{
	/*struct Fish{
		float2 position;
		float scale;
		int texture_num;
	};*/
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SubTex("Texture", 2D) = "white" {}
		_FishPosition1("Fish1", Vector) = (0,0,1,100)
		_FishPosition2("Fish2", Vector) = (0,0,1,100)
		_FishPosition3("Fish3", Vector) = (0,0,1,100)
		_FishPosition4("Fish4", Vector) = (0,0,1,100)
		//_FishPosition1("Fish1", Vector) = (0,0,0,0) {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };

            sampler2D _MainTex;

			sampler2D _SubTex;
            float4 _MainTex_ST;

			float4 _FishPosition1;
			float4 _FishPosition2;
			float4 _FishPosition3;
			float4 _FishPosition4;

		

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color;
                return o;
            }

			float3 Leap(float3 a, float3 b, float c) {
				return a * c + b * (1 - c);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 _FishPosition[] = { _FishPosition1,
			 _FishPosition2,
			 _FishPosition3,
			 _FishPosition4 };


			//float f =( atan(uv2.x / uv2.y))+6;

			//f = abs((f + _Time.y) % 0.4f-0.2f);
			////if (f> 0.1f) {//-length(uv2)*0.02f 
			////	return float4(1,1,1,1);
			////}else{
			//	// sample the texture
			//	fixed4 col = tex2D(_MainTex, i.uv)*i.color;
			//	float2 cuv = i.uv;
			//	cuv.y = cuv.y * 4 - 2;
			//	cuv.x = cuv.x*4 - 2;
			//	cuv.y/=(cuv.x+1)*0.4+0.2;
			//	if (-1 < cuv.x && 1 > cuv.x && -1 < cuv.y && 1 > cuv.y) {
			//		cuv *= 0.5f + 0.5f;
			//		return tex2D(_SubTex, cuv);
			//	}
			//uv2.y += (int(_Time.y/4)*3.14159265*int(_Time.y / 4)*3.14159265) % 1.0f-0.5f;//
			//uv2.x +=_Time.y%4-2;

			/*
			xyzそれぞれの座標
			w大きさ
			*/

			for (int j = 0; j < 4; j++) {
				float2 uv2 = i.uv - 0.5f;
				float2 pos = _FishPosition[j].xy/ _FishPosition[j].z;

				float zbuffer = _FishPosition[j].z/_FishPosition[j].w;
				pos.x = (pos.x  + uv2.x) * zbuffer + 0.5f;
				pos.y = (pos.y  + uv2.y) * abs(zbuffer) + 0.5f;

				float4 col = tex2D(_SubTex, pos);
				if (col.a > 0.1f) {
					col.rgb = Leap(col.rgb, i.color.rgb, 1.0f / abs(_FishPosition[j].z));
					col.a = 1;
					return col;
				}
			}
				UNITY_APPLY_FOG(i.fogCoord, i.color);
				return i.color;
				//	}
			}
            ENDCG
        }
    }
}
