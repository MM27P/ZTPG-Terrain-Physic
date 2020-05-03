Shader "Hidden/ScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex2 ("Texture", 2D) = "white" {}
        _TexSize ("Texture Size", Float) = 512
        _Color ("Color", Color) = (1,1,1,0.37)
        _ColorMult("Luminocity", Range(-1,2)) = 1.1
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
            float   _TexSize;
            float _ColorMult;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                 float2 c = i.uv;
                 float2 center = float2(_TexSize/2,_TexSize/2);
                 float2 point = float2(c.x,c.y);
                 float distanceValue =abs( distance(center,point));
                 float center =   _TexSize/2;

                 fixed4 col = tex2D(_MainTex, i.uv);
                
                float temp = (c.x/_TexSize) * (c.y/_TexSize);
                // just invert the colors
                col.rgb = col.rgb;
                fixed4 resultCol = fixed4(1, 1, 1, 0);

                float temp2 = dot( col, resultCol);
                col.rgb = col.rgb*temp2;
                return col;
            }
            ENDCG
        }
    }
}
