Shader "Hidden/VelocityVisualizer" {
	Properties 	{
		_MainTex("Texture", 2D) = "black" {}
	}
	SubShader 	{
		Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #define PI 3.141592
            #define NORMALIZE_RAD (0.5 / PI)
			
			#include "UnityCG.cginc"
            #include "Assets/Packages/Gist2/Shaders/HSV.cginc"

			struct appdata 	{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f	{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;

            float4 _VelocityVisualizer0;
			
			v2f vert (appdata v) {
				v2f o;
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target {
				float4 c = tex2D(_MainTex, i.uv);

				float h = frac(atan2(c.x, c.y) * NORMALIZE_RAD);
                float v = _VelocityVisualizer0.x * length(c.xy);
                return float4(HSV2RGB(float3(h, 1, saturate(v))), step(1e-3, v));
			}
			ENDCG
		}
	}
}
