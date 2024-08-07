Shader "Custom/SelectDelete"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _OpacityTex ("_OpacityTex (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:blend 

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _OpacityTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_OpacityTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Emission = c.rgb;
            fixed4 alphaVal = tex2D(_OpacityTex, IN.uv_OpacityTex);
            o.Alpha = (alphaVal.r + alphaVal.g + alphaVal.b) / 10.0f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
