Shader "Hidden/PostProcessing/Copy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 pos : POSITION;
            };

            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                float4 clipPos : SV_POSITION;
            };

            VertexOutput vert (VertexInput v)
            {
                VertexOutput o;
                o.clipPos = float4(v.pos.xy, 0.0, 1.0);
                o.uv = v.pos.xy *0.5 + 0.5;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraColorTexture;
            fixed4 frag (VertexOutput i) : SV_Target
            {
                fixed4 col = tex2D(_CameraColorTexture, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
