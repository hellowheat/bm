Shader "Unlit/lifeLine"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Threshold("threshold",Range(0,1))=1
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
            float _Threshold;

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
                if((i.locv.y+1)/2 > _Threshold)i.col.a=0;
                return i.col;
            }
            ENDCG
        }
    }
}
