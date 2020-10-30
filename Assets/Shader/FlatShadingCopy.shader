// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Geometry/FlatShadingCopy"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_PosX("PosX", Range(-10,10)) = 0
		_PosY("PosY", Range(-10,10)) = 0
		_PosZ("PosZ", Range(-10,10)) = 0
	}

		SubShader
	{

		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

			Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"
			#pragma vertex vert
			//Geometry Shader ステージのときに呼び出される
			#pragma geometry geom
			#pragma fragment frag

			float4 _Color;
			sampler2D _MainTex;
			float _PosX;
			float _PosY;
			float _PosZ;


		float	_StartTime;

			struct v2g
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 vertex : TEXCOORD1;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float light : TEXCOORD1;
			};

			v2g vert(appdata_full v)
			{
				v2g o;
				o.vertex = v.vertex;
				o.pos = UnityObjectToClipPos(v.vertex);// *10.0f;
				o.uv = v.texcoord;
				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
			{
				g2f o;

				//法線ベクトルの計算(ライティングで使用)
				float3 vecA = IN[1].vertex - IN[0].vertex;
				float3 vecB = IN[2].vertex - IN[0].vertex;
				float3 normal = cross(vecA, vecB);
				float3 c_normal = normalize(normal);
				normal = normalize(mul(normal, (float3x3) unity_WorldToObject));

				//ライティングの計算
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				o.light = max(0., dot(normal, lightDir));

				//o.uv = (IN[0].uv + IN[1].uv + IN[2].uv) / 3;


				/*vecA = IN[1].pos - IN[0].pos;
				vecB = IN[2].pos - IN[0].pos;
				normal = cross(vecA, vecB);
				normal = normalize(normal);*/


				float3 rot_pos, av_pos;

				//中心座標
				av_pos = (IN[0].pos + IN[1].pos + IN[2].pos) *0.3333333333;
				//中心座標
				float3 avv_pos = (IN[0].vertex + IN[1].vertex + IN[2].vertex) *0.3333333333;
				avv_pos -= float3(_PosX,_PosY,_PosZ);
				float l_time = _Time.z%10.f;

				float len = l_time - length(avv_pos);

				//回転値
				float rot_f = max(0, (len - 1.5f))*2.0f;
				//色を黒くする
				o.light *= min(1, max(0, 1 - len * 0.5f));


				float _cos, _sin, _acos;
				_cos = cos(rot_f*6.2831853);
				_sin = sin(rot_f*6.2831853);
				_acos = 1 - _cos;
				float cx, cy, cz;
				cx = normal.x*_acos;
				cy = normal.y*_acos;
				cz = normal.z*_acos;



				float3x3 rotmat = float3x3(cx*normal.x + _cos, cx*normal.y- normal.z*_sin, cx*normal.z + normal.y*_sin,
					cy*normal.x + normal.z*_sin, cy*normal.y + _cos, cy*normal.z - normal.x*_sin, 
					cz*normal.x - normal.y*_sin, cz*normal.y + normal.x*_sin, cz*normal.z + _cos
					);


				//メッシュ作成
				for (int i = 0; i < 3; i++)
				{
					o.uv = IN[i].uv;

					rot_pos = IN[i].pos - av_pos;
					o.pos = IN[i].pos;

					if (len > 2) {
						//1点でしか描画意思ない
						o.pos.rgb = av_pos;
					}
					else {
						o.pos.rgb = av_pos + mul(rot_pos, rotmat);
					}
					//o.pos.rgb = av_pos + mul(rot_pos, rotmat);// +normal * len;
					//o.pos = IN[i].pos;
					triStream.Append(o);
				}
				//tristream.RestartStrip();//さらに他の三角メッシュを作成する時に必要
			}

			half4 frag(g2f i) : COLOR
			{
				float4 col = tex2D(_MainTex, i.uv);
				col.rgb *= i.light * _Color;
				return col;
			}

			ENDCG
		}
	}
		Fallback "Diffuse"
}