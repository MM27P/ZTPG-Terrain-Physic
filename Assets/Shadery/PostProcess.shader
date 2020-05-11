Shader "PostEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			// note: no SV_POSITION in this struct
			struct v2f {
				float2 uv : TEXCOORD0;
			};

			v2f vert(
				float4 vertex : POSITION, // vertex position input
				float2 uv : TEXCOORD0, // texture coordinate input
				out float4 outpos : SV_POSITION // clip space position output
				)
			{
				v2f o;
				o.uv = uv;
				outpos = UnityObjectToClipPos(vertex);
				return o;
			}

            sampler2D _MainTex;


			uniform float _x_center;
			uniform float _y_center;
			uniform int _crack_density;
			uniform float _Rlenth[40];
			uniform float _Rangle[40];
			uniform float _Factor_Y[40];
			uniform float _Factor_X[40];
			uniform float _Far_Y[40];
			uniform float _Far_X[40];

			float temp_lenth;
			float temp_col;
			float point_factor;



            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 pixel = i.uv;

				for (int i = 0; i < _crack_density; i++) 
				{
					point_factor = (pixel.y - _x_center)*(pixel.y - _x_center) + (pixel.x - _y_center)*(pixel.x - _y_center) - _Rlenth[i] * _Rlenth[i];

					temp_lenth = sqrt((pixel.y - _Far_X[i])*(pixel.y - _Far_X[i]) + (pixel.x - _Far_Y[i])*(pixel.x - _Far_Y[i]));

					temp_col = 1 - temp_lenth / _Rlenth[i];

					if ((i%2 == 0)&&point_factor > -0.00012f && point_factor < 0.00012f)
					{

						if (i % 4 == 0&& pixel.y - 0.04 > _Far_X[i] && pixel.x < _Far_Y[i]+0.04)
						{
							col.rgb += abs(float3(col.r*temp_col, col.g*temp_col, col.b*temp_col));
						}						
					}

					if (((pixel.x - 0.003f) < (pixel.y * _Factor_X[i] + _Factor_Y[i])) && ((pixel.x) > ((pixel.y + 0.003f) * _Factor_X[i] + _Factor_Y[i])))
					{

						if (temp_lenth < _Rlenth[i])
						{
							col.rgb += abs(float3(col.r*temp_col, col.g*temp_col, col.b*temp_col));
						}
					}					
				}

				//_x_center + _Rlenth[i] * cos(_Rangle[i]) 
				//_y_center + _Rlenth[i] * sin(_Rangle[i]) 
					
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
