Shader "Unlit/ObjectVelocity" {
	Properties { }
	SubShader {
		Tags { "RenderType" = "Opaque" "Velocity" = "Vertex" }

		Pass {
			CGPROGRAM
            #pragma target 5.0
            #pragma multi_compile_instancing
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float4 velocity : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            float4x4 _PrevMV;
            float4 _ObjectVelocity0;
			
			v2f vert (appdata v) {
				UNITY_SETUP_INSTANCE_ID(v);

				float idt = _ObjectVelocity0.y;
				float3 viewPrev = mul(_PrevMV, float4(v.vertex.xyz, 1)).xyz;
                float3 viewCurr = UnityObjectToViewPos(v.vertex);
				//float2 velocity = (_ObjectVelocity0.x * (viewCurr - viewPrev).xy) * idt;

				float4 projPrev = mul(UNITY_MATRIX_P, mul(_PrevMV, float4(v.vertex.xyz, 1)));
				float4 projCurr = UnityObjectToClipPos(v.vertex);

				float2 uvPrev = 0.5 * projPrev.xy / projPrev.w;
				float2 uvCurr = 0.5 * projCurr.xy / projCurr.w;

				float2 vel = (_ObjectVelocity0.x * (uvCurr - uvPrev) * _ScreenParams.xy * float2(1, -1)) * idt;
				float4 velocity = float4(vel, length(max(0, -vel)), 0);

				v2f o;
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex = projCurr;
				o.velocity = velocity;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);

				return float4(i.velocity.xyz, 1);
			}
			ENDCG
		}
	}
}
