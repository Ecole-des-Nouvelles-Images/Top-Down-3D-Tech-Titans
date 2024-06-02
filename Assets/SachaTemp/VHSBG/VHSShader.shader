Shader "Custom/VHSShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _ScanlineTex ("Scanline Texture", 2D) = "black
       " {}
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.5
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.5
        _ColorDistortion ("Color Distortion", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            sampler2D _ScanlineTex;
            float _NoiseIntensity;
            float _ScanlineIntensity;
            float _ColorDistortion;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Add noise
                float2 noiseUV = float2(i.uv.x * _Time.y, i.uv.y * _Time.y);
                fixed4 noise = tex2D(_NoiseTex, noiseUV);
                col.rgb += noise.rgb * _NoiseIntensity;

                // Add scanlines
                float scanline = sin(i.uv.y * 300.0 + _Time.y * 3.0) * 0.5 + 0.5;
                col.rgb *= lerp(1.0, scanline, _ScanlineIntensity);

                // Add color distortion
                float2 offsetUV = i.uv + float2(sin(i.uv.y * 20.0 + _Time.y) * 0.005, 0.0);
                fixed4 distortedCol = tex2D(_MainTex, offsetUV);
                col.rgb = lerp(col.rgb, distortedCol.rgb, _ColorDistortion);

                return col;
            }
            ENDCG
        }
    }
}