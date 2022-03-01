Shader "Unlit/ObjectVelocity-Unlit" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Tags { "RenderType" = "Opaque" "Velocity"="Vertex" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float4 _Color;

            v2f vert (appdata v) {
                UNITY_SETUP_INSTANCE_ID(v);

                v2f o;
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                UNITY_SETUP_INSTANCE_ID(i);
                return _Color;
            }
            ENDCG
        }
    }
}
