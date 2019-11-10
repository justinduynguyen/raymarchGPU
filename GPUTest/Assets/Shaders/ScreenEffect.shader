Shader "Hidden/ScreenEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	   _Src("Src",2D)="white"{}
	    
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
            #define vec2 float2
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
			sampler2D _Src;
            float4 frag (v2f i) : SV_Target
            {
            float4 add = tex2D(_MainTex, i.uv);
			float4 src = tex2D(_Src, i.uv);
			float4 col = lerp(src, add, add.w);
			return add;
            }
            ENDCG
        }
    }
}
