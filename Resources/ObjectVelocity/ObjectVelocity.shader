// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/ObjectVelocity" {
	Properties {
        _Power ("Power", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

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
				float2 velocity : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

            float4x4 _PrevMV;
            float4 _Power;
			
			v2f vert (appdata v) {
				UNITY_SETUP_INSTANCE_ID(v);

				float4 dt = unity_DeltaTime;
				float3 viewPrev = mul(_PrevMV, float4(v.vertex.xyz, 1)).xyz;
                float3 viewCurr = UnityObjectToViewPos(v.vertex);

				float2 velocity = _Power.w * (viewCurr - viewPrev).xy * dt.y;

				v2f o;
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.velocity = velocity;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target {
				UNITY_SETUP_INSTANCE_ID(i);
				return float4(i.velocity, 0, 1);
			}
			ENDCG
		}
	}
}
