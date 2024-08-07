Shader "Custom/Door_Sign"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _AlphaValue("AlphaValue",Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows 
        #pragma target 3.0

        sampler2D _MainTex;
        float _AlphaValue;

        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = 1.0f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
