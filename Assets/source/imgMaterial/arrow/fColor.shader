// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/fColor"
{
    Properties
    {
        _MainTex ("base (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Transparent"}
        Pass{
        LOD 200
        ZWrite off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        
        CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
        sampler2D _MainTex;
    struct vertexOutput {
        float4 pos : SV_POSITION;
        float4 tex : TEXCOORD0;
        };
    vertexOutput vert(appdata_full input) {
        vertexOutput output;
        output.pos = UnityObjectToClipPos(input.vertex);
        output.tex = input.texcoord;
        //output.tex = input.texcoord * (1 / radio) - (1 / radio - 1) / 2;
        return output;
    }
    float4 frag(vertexOutput input) : COLOR
    {
        float radio = (sin(_Time.w)) / 16;//缩放倍数
        float com1 = input.tex.x > input.tex.y ? 1 : 0;
        float com2 = 1 - input.tex.x > input.tex.y ? 1 : 0;

        if (com1 && com2)input.tex.y -= radio;
        else if (!com1 && com2)input.tex.x -= radio;
        else if (!com1 && !com2)input.tex.y += radio;
        else if (com1 && !com2)input.tex.x += radio;
        float4 col = tex2D(_MainTex,input.tex);
        /*if (col.r == 1 && col.g == 1 && col.b == 1)col.a = 0;
        else col.a = 0.8;*/
        return col;
    }
        ENDCG

    }
    }
    FallBack "Diffuse"
}
