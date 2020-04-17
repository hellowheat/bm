// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/appear"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VoiceTex("Texture",2D) = "white" {}
        _threshold("threshold",Range(0,1)) = 0

        _RimColor("rim color", Color) = (0, 0.4, 1.0, 1)    //光晕颜色
        _RimSize("rim size", Range(0,5)) = 1    //光晕颜色
    }
    SubShader
        {
            Tags { "Queue" = "Transparent" }
            LOD 100
            ZWrite off
            Blend SrcAlpha OneMinusSrcAlpha
            
            Pass{
               //ZTest always
               Cull Back
               Blend SrcAlpha OneMinusSrcAlpha

               CGPROGRAM
               #pragma vertex vert
               #pragma fragment frag
               #include"UnityCG.cginc"
               struct v2f{
                    float2 uv:TEXCOORD0;
                    float4 vertex:POSITION;
                    float3 normal:NORMAL;
                    fixed4 col:COLOR;
			   };
               
               sampler2D _VoiceTex;
               float4 _VoiceTex_ST;

               float _threshold;
               fixed4 _RimColor;
               float _RimSize;
                v2f vert(appdata_base v){
                    v2f o;
                   
                    float4 ve = mul(UNITY_MATRIX_MV,v.vertex);
                    float3 n = (mul((float3x3)UNITY_MATRIX_IT_MV,v.normal));
                   // ve.xyz+=normalize(n)*_RimSize*length(ObjSpaceViewDir(v.vertex))*0.00001f;
                    ve.xyz+=normalize(n)*lerp(10,1,_threshold)*0.00005f*length(ObjSpaceViewDir(v.vertex))*_RimSize;
                    //ve.xyz+=normalize(n)*_RimSize*100;
                    o.vertex=mul(UNITY_MATRIX_P,ve);
                    o.normal=v.normal;
                    o.vertex.z-=0.001;
                    o.col=_RimColor;
                    o.uv=TRANSFORM_TEX(v.texcoord,_VoiceTex);
                    //o.vertex.z-=0.001;
                    return o;
				}

                fixed4 frag(v2f v):SV_Target{
                    fixed4 voiceCol=tex2D(_VoiceTex,v.uv);
                    if (voiceCol.r > _threshold*0.9)v.col.a = 0;
                    else v.col.a = 1;
                    return v.col;
				}
               ENDCG
			}
            Pass
            {
             //   Cull Back
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    fixed3 normal : NORMAL;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    //UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    fixed3 normal : NORMAL;
                };

                sampler2D _MainTex;
                sampler2D _VoiceTex;
                float4 _MainTex_ST;
                float _threshold;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.normal = mul(v.normal, (float3x3)unity_WorldToObject);
                    //UNITY_TRANSFER_FOG(o,o.vertex);
                    //o.vertex.z-=10.001;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 voiceCol = tex2D(_VoiceTex, i.uv);
                    if (voiceCol.r > _threshold)col.a = 0;
                    else col.a = 1;
                    //UNITY_APPLY_FOG(i.fogCoord, col);
                    return col;
                }
                ENDCG
            }
            
            /*Pass{
            ZTest always
                ZWrite on
                ColorMask 0     
			}*/
    }
}
