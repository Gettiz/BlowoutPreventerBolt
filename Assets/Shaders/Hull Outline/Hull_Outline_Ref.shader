Shader "Custom/Hull_Outline_Ref"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" "RenderPipeline" = "UniversalPipeline" }
        
        ColorMask 0
        ZWrite Off

        Pass
        {
            Name "StencilMask"

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
                ZFail Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                return half4(1,1,1,1);
            }
            ENDHLSL
        }
    }
}