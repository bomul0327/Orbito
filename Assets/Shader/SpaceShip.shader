Shader "Orbito/SpaceShip"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" { }
        _MaskTex ("Mask Texture", 2D) = "black" { }
        _MaskColor ("Mask Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Cull Off
        Lighting Off
        ZWrite Off
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_local _ PIXELSNAP_ON
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD0;
                float4 color: COLOR;
            };
            
            struct v2f
            {
                float2 uv: TEXCOORD0;
                float4 vertex: SV_POSITION;
                float4 color: COLOR;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _MaskTex;
            fixed3 _MaskColor;
            
            v2f vert(appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                return o;
            }
            
            fixed4 frag(v2f i): SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                fixed3 mask = tex2D(_MaskTex, i.uv) * _MaskColor;
                
                fixed grayMask = saturate(mask.r + mask.g + mask.b) / 3;
                col.rgb = lerp(saturate(1 - 2 * (1 - col.rgb) * (1 - mask)), saturate(2 * col.rgb * mask), grayMask);
                
                return col;
            }
            ENDCG
            
        }
    }
}
