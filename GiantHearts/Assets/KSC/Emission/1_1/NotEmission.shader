Shader "Custom/NotEmission"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _RampTex("RampTex (RGB)", 2D) = "white" {}
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
        sampler2D _MetallicTex;
        sampler2D _RTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_RampTex;
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
                float3 lightDir = normalize(UnityWorldSpaceLightDir(IN.worldPos));

                float ndotl = dot(IN.worldNormal, lightDir) * 0.5 + 0.5;

                float ndotv = saturate(dot(IN.worldNormal, IN.viewDir));

                //float4 ramp = tex2D(_RampTex, float2(ndotl, ndotv));

                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                //fixed4 e = tex2D(_EmssionTex, IN.uv_EmssionTex);
                //float3 rampRGB = ramp.rgb;
                //o.Metallic = tex2D(_EmssionTex, IN.uv_EmssionTex).r;
                o.Smoothness = tex2D(_RTex, IN.uv_RTex).r;
                o.Albedo = (c.rgb);
                //o.Emission = e.rgb;
                o.Alpha = c.a;
            }
            ENDCG
    }
        FallBack "Diffuse"
}
