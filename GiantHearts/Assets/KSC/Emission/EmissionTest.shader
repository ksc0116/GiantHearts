Shader "Custom/EmissionTest"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _EmissionTex("EmissionTex (RGB)", 2D) = "white" {}
        _GlowIntensity("Intensity",Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _EmissionTex;
        float _GlowIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

            o.Emission = tex2D(_EmissionTex, IN.uv_EmissionTex).rgb;

            o.Emission *= pow(2, _GlowIntensity);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
