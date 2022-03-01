Shader "Hidden/MotionVectorCapture" {
    Properties {
        _MainTex ("Texture", 2D) = "black" {}
        _MotionVectorCapture0 ("Param0", Vector) = (1, 0, 0, 0)
    }
    SubShader {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraMotionVectorsTexture;

            float4 _MotionVectorCapture0;

            float4 frag (v2f i) : SV_Target {
                float4 cvec = tex2D(_CameraMotionVectorsTexture, i.uv);
                cvec.xy *= _ScreenParams.xy * _MotionVectorCapture0.x * unity_DeltaTime.y;
                cvec.z = length(max(0, -cvec.xy));
                return float4(cvec.xyz, 1);
            }
            ENDCG
        }
    }
}
