// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/outline2"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Factor("宽度", float) = 0
		_Color("边缘颜色", Color) = (0,0,0,1)
	}
		SubShader
		{
			LOD 100
			tags {"Queue" = "Transparent"}
			Pass
			{
				//ZWrite On
				//ColorMask 0
			}

			/*Pass
			{
				//Tags{  "LightMode" = "ForwardBase" }
				Blend SrcAlpha OneMinusSrcAlpha
				ZWrite Off
				
				Cull Back        // 剔除背面
				CGPROGRAM
			//ZWrite Off
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
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.vertex.z;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			// apply fog
			UNITY_APPLY_FOG(i.fogCoord, col);
			//discard;
			return float4(col.xyz, 0.2);
				}
				ENDCG
			}*/

			Pass
			{
				Tags{ "Queue" = "Geometry" "LightMode" = "ForwardBase" }
				Cull Front   //剔除正面
				ZWrite On
					//Offset -300, -5
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				struct v2f
				{
					float4 vetex : POSITION;
				};
				float _Factor;
				half4 _Color;
				v2f vert(appdata_full data)
				{
					v2f o;
					data.vertex.xyz += data.normal*0.5;
					float4 view_vetex = UnityObjectToClipPos(data.vertex);
					//float3 view_normal =mul((float3x3)UNITY_MATRIX_IT_MV,data.normal);
					//float3 offset = mul(UNITY_MATRIX_P, view_normal);
					//float3 view_normal = mul(UNITY_MATRIX_IT_MV, data.normal);
					//float3 offset = mul(UNITY_MATRIX_P, view_normal);
					o.vetex = view_vetex;
					//o.vetex.xyz += offset * _Factor * 0.0001;
					//o.vetex = UnityObjectToClipPos(data.vertex);
					//o.vetex.z += 0.01;
					return o;
				}
				half4 frag(v2f o) :COLOR
				{
					return _Color;
				}

				ENDCG
				}
			}
}

