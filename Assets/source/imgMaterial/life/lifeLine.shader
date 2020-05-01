Shader "Unlit/lifeLine"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Threshold("threshold",Range(0,1))=1
        _Threshold2("threshold2",Range(0,1))=1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite off
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
            };

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 col :COLOR;
                float4 locv: TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _Color2;
            float _Threshold;
            float _Threshold2;

            v2f vert (appdata v)
            {
                v2f o;
                o.locv=v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.col=_Color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_APPLY_FOG(i.fogCoord, i.col);
                float testPos =(i.locv.y+1)/2;
                if(testPos >= _Threshold2)i.col.a=0;
                else if(testPos >= _Threshold)i.col=_Color2;
                return i.col;
            }
            ENDCG
        }
    }
}
