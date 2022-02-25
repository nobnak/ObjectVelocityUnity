Shader "Hidden/VelocityVisualizerFilter" {
	Properties 	{
		_MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
	}
	SubShader 	{
		Tags { "RenderType"="Overlay" "Queue"="Overlay" }
		LOD 100
        ColorMask RGB

		Pass 		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

            #define PI 3.141592
            #define NORMALIZE_RAD (0.5 / PI)
			
			#include "UnityCG.cginc"
            #include "Assets/Shaders/ColorSpace.cginc"

			struct appdata 	{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f	{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

            float _Scale;
			
			v2f vert (appdata v) {
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                float2 uvFromBottom = uv;
                if (_ProjectionParams.x < 0)
                    uvFromBottom.y = 1 - uvFromBottom.y;

				v2f o;
				o.uv = uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target {
				float4 c = tex2D(_MainTex, i.uv);
				float h = frac(atan2(c.x, c.y) * NORMALIZE_RAD);
                float v = _Scale * length(c.xy);
                return float4(HSV2RGB(float3(h, 1, saturate(v))), 0);
			}
			ENDCG
		}
	}
}
