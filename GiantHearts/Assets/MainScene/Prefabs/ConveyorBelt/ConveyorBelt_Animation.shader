Shader "Custom/ConveyorBelt_Animation"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _NormalTex("NormalTex (RGB", 2D) = "white" {}
        _ARMTex("ARMTex (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalTex;
        sampler2D _ARMTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalTex;
            float2 uv_ARMTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv_mainTex = IN.uv_MainTex;
            uv_mainTex.g += _Time.x;

            float2 uv_normalTex = IN.uv_NormalTex;
            uv_normalTex.g += _Time.x;

            fixed4 c = tex2D(_MainTex, uv_mainTex);
            fixed4 arm = tex2D(_ARMTex, uv_mainTex);
            o.Normal = UnpackNormal(tex2D(_NormalTex, uv_normalTex));
            o.Albedo = (c.rgb);
            o.Metallic = arm.b;
            o.Smoothness = arm.g;
            o.Occlusion = arm.r;
            o.Alpha = c.a;
        }
        ENDCG
    }
        FallBack "Diffuse"
}
