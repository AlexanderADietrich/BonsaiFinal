Shader "Unlit/NodeShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			


			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
						
			#include "UnityCG.cginc"
			//Using https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html
			#include "UnityLightingCommon.cginc" // for _LightColor0


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 diff : COLOR0; // diffuse lighting color, from example
			};

			sampler2D _MainTex;
            float4x4 MyTRSMatrix;
            fixed4 MyColor;
			vector lightPos;
			fixed4 lightColor;
			float random;

			
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(MyTRSMatrix, v.vertex);
				o.vertex = mul(UNITY_MATRIX_VP, o.vertex);



				//normal in world context
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				//dot product between light normal and obj normal
				half nl = max(0, dot(worldNormal, lightPos.xyz));
				//use light 
				o.diff = nl * lightColor;

				o.uv = v.uv;
				return o;
			}
			
			float rand(float2 uv)
			{
				//more lines change 1st
				return (100 * uv.x * uv.y);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                col += MyColor;
				col *= (i.diff+0.25);
				//col *= (i.diff+0.5 + sin((_Time.y + rand(i.uv))*10));
				return col;
			}
			ENDCG
		}
	}
}
