Shader "Custom/EntranceDoor"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _ARMTex ("ARMTex  (RGB)", 2D) = "white" {}
        _AlphaValue("AlphaValue", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows 
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _ARMTex;
        float _AlphaValue;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_ARMTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 arm = tex2D(_ARMTex, IN.uv_ARMTex);
            o.Albedo = c.rgb;
            o.Metallic = arm.b;
            o.Smoothness = arm.g;
            o.Alpha = 1.0f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
