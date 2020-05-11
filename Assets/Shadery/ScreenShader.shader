Shader "Hidden/ScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex2 ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,0.37)
        _ColorMult("Color Power", Range(-1,2)) = 1.1
        _intensity("Intenstity of effect", Range(0,1)) = 1
        _brithness("Brithness of effect", Range(1,5)) = 1
         _crack_number("Number of crack", Range(1,6)) = 1
         _circlePower("Thick of circles", Range(0.1,5))=1
         _rayPower("Thick of rays", Range(0.1,2))=1
         _Force("Power of force", Range(0.5,10))=1
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _ColorMult;
            fixed4 _Color;
            float _intensity;
            float _brithness;
            float _rayOffest;
            float _rayDistance;

            //Przemo Part        
			uniform float _x_center[6];
			uniform float _y_center[6];
			uniform int _crack_density;
            uniform int _crack_number;
			uniform float _Rlenth[600];
			uniform float _Rangle[600];
			uniform float _Factor_Y[600];
			uniform float _Factor_X[600];
			uniform float _Far_Y[600];
			uniform float _Far_X[600];
            float  _circlePower ;
            float  _rayPower ;
            float  _Force ;
			float temp_lenth;
			float temp_col;

			float point_factor;
            
            fixed4 frag (v2f i) : SV_Target
            {
               //Dodawanie efektu szkła
               float2 c = i.uv;
               float tempLol=(abs(c.x- 0.5)/ 0.5)+(abs(c.y- 0.5)/ 0.5)*_brithness;
               fixed4 col = tex2D(_MainTex, i.uv);               
                col.rgb = col.rgb*_ColorMult;
                fixed4 resultCol = (_Color)*tempLol*_intensity;

                float temp2 = dot( col, resultCol);
                col.rgb = col.rgb+resultCol;

                //Przemo Part
				float2 pixel = i.uv;

                for(int j =0; j<_crack_number;j++)
                {
                        for (int i = 0; i < _crack_density; i++) 
				        {
                        int index = _crack_density*j+i;
					        point_factor = (pixel.y - _x_center[j])*(pixel.y - _x_center[j]) + (pixel.x - _y_center[j])*(pixel.x - _y_center[j]) - _Rlenth[index] * _Rlenth[index];

					        temp_lenth = sqrt((pixel.y - _Far_X[index])*(pixel.y - _Far_X[index]) + (pixel.x - _Far_Y[index])*(pixel.x - _Far_Y[index]));

					        temp_col = 1 - temp_lenth / _Rlenth[index];
                              float circleTemp =_circlePower*  0.00012f;
					        if ((index%1 == 0)&&point_factor > -circleTemp && point_factor < circleTemp)
					        {

						        if (index % 2 == 0&& pixel.y - 0.04 > _Far_X[index] && pixel.x < _Far_Y[index]+0.04)
						        {
							        col.rgb += abs(float3(_Color.r*temp_col, _Color.g*temp_col, _Color.b*temp_col));
						        }						
					        }
                      

                            float rayTemp =_rayPower*  (0.003f -(0.003f *temp_lenth/0.5 ));
					        if (((pixel.x - rayTemp) < (pixel.y * _Factor_X[index] + _Factor_Y[index])) && ((pixel.x) > ((pixel.y + rayTemp) * _Factor_X[index] + _Factor_Y[index])))
					        {

						        if (temp_lenth < _Rlenth[index])
						        {
							        col.rgb += abs(float3(_Color.r*temp_col, _Color.g*temp_col, _Color.b*temp_col));
						        }
					        }					
				      }
                  }
				
                return col;
										
            }
            ENDCG
        }
    }
}
