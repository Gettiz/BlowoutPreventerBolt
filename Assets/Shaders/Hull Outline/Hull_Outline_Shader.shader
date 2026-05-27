Shader "Unlit/Hull_Outline_Shader"
{
    Properties
    {
        _OutlineThickness ("OutlineThickness", Range(0,1)) = 1
        _OutlineColor ("OutlineColor", Color) = (0,0,0,1)
        _Cutoff ("Alpha Cutoff", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent+1" 
            "RenderType"="Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        LOD 100

        Pass
        {
            ZTest Always
            ZWrite Off
            Cull Off

            Stencil
            {
                Ref 1
                Comp NotEqual
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
            };
            
            CBUFFER_START(UnityPerMaterial)
                float _OutlineThickness;
                float4 _OutlineColor;
                float _Cutoff;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;

                float3 norm = normalize(v.normal);
                float3 worldPos = v.vertex.xyz + (norm * _OutlineThickness);

                o.vertex = TransformObjectToHClip(worldPos);
                o.normal = norm;
                return o;
            }
            
            half4 frag(v2f i, bool facing : SV_IsFrontFace) : SV_Target
            {
                half4 col = _OutlineColor;
                half finalAlpha = facing ? 0.0 : 1.0;
                col.a = finalAlpha;
                
                clip(col.a - _Cutoff);
                
                return col;
            }
            ENDHLSL
        }


    }
}