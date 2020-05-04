Shader "Unlit/trackBall"
{
    Properties
    {
        _MainColor("MainColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                fixed4 col :COLOR;
                float4 vertex : SV_POSITION;
            };

            fixed4 _MainColor;

            v2f vert (appdata v)
            {
                v.vertex+=normalize(v.vertex)*(((float)length(ObjSpaceViewDir(v.vertex)))/100);
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.col=_MainColor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.col;
            }
            ENDCG
        }
    }
}
