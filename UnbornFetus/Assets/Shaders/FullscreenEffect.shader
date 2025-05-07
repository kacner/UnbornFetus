Shader"Hidden/URP/FullscreenEffect"
{
    Properties
    {
        _MainTex    ("Texture",     2D)   = "white" {}
        _Intensity  ("Intensity",   Range(0,1)) = 1.0
        _DitheringIntensity  ("DitheringIntensity",   Range(0,0.2)) = 1.0
        _ColorAmount("Color Amount",Range(1,255))= 16.0
        _PixelSize ("PixelFactor",  Range(0, 500)) = 5
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Name     "Fullscreen"
            ZTest    Always
            Cull     Off
            ZWrite   Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#pragma target 3.0
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float     _Intensity;
            float _DitheringIntensity;
            float _ColorAmount;
            int _PixelSize;
            

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv       : TEXCOORD0;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv       = v.uv;
                return o;
            }

            half4 frag(v2f IN) : SV_Target
            {
                float2 screenSize = float2(_MainTex_TexelSize.z, _MainTex_TexelSize.w);
                float2 blockUV = floor(IN.uv * screenSize / _PixelSize) * _PixelSize / screenSize;
                half4 col = tex2D(_MainTex, blockUV);
                
    
                static const int4x4 bayer4x4 =
                {
                    { 0, 8, 2, 10 },
                    { 12, 4, 14, 6 },
                    { 3, 11, 1, 9 },
                    { 15, 7, 13, 5 }
                };
    
                int x = fmod(blockUV.x * _MainTex_TexelSize.z, 4);
                int y = fmod(blockUV.y * _MainTex_TexelSize.w, 4);
    
                int M = bayer4x4[y][x];
    
                float FM = (M + 0.5) / 16 - 0.5;
                col.rgb += FM * _DitheringIntensity;
                half3 finalCol = (round(col.rgb * (_ColorAmount - 1) + 0.5)) / (_ColorAmount - 1);
                col.rgb = lerp(col.rgb, finalCol, _Intensity);
    
                return col;
            }
            ENDHLSL
        }
    }
}
