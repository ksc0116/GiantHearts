Shader "Custom/Emission"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("RampTex (RGB)", 2D) = "white" {}
        _EmssionTex("EmssionTex (RGB)", 2D) = "white" {}
        _MetallicTex("MetallicTex (RGB)", 2D) = "white" {}
        _RTex("RTex (RGB)", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows noambient

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _RampTex;
        sampler2D _EmssionTex;
        sampler2D _MetallicTex;
        sampler2D _RTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_RampTex;
            float2 uv_EmssionTex;
            float2 uv_MetallicTex;
            float2 uv_RTex;
            float3 worldPos;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

                o.Emission = tex2D(_EmssionTex, IN.uv_EmssionTex).rgb;

                // ±¤¿ø °è»ê
                float3 toLight = _WorldSpaceLightPos0 - IN.worldPos;
                float distToLight = length(toLight);
                float3 lightDir = toLight / distToLight;
                float3 normal = normalize(o.Normal);
                float atten = 1.0 / (1.0 + 0.1 * distToLight + 0.01 * distToLight * distToLight);
                float3 diffuse = _LightColor0.rgb * max(0.0, dot(normal, lightDir)) * atten;
                o.Normal = normal;
                o.Metallic = tex2D(_MetallicTex, IN.uv_MetallicTex);
                o.Smoothness = tex2D(_RTex, IN.uv_RTex);
                o.Alpha = 1.0;
                //o.Emission *= diffuse;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
