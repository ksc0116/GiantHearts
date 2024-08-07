Shader "Custom/Toon"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Normal("Normal (RGB)", 2D) = "white" {}
        _RampTex("RampTex (RGB)", 2D) = "white" {}
        _MetalTex("MetalTex (RGB)", 2D) = "white" {}
        _RoughnessTex("RoughnessTex (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows noambient

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Normal;
        sampler2D _RampTex;
        sampler2D _MetalTex;
        sampler2D _RoughnessTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Normal;
            float2 uv_RampTex;
            float2 uv_MetalTex;
            float2 uv_RoughnessTex;
            float3 worldPos;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        //float4 LightingWrap(SurfaceOutputStandard s, float3 lightDir, float3 viewDir, float atten)
        //{
        //    float ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;

        //    float ndotv = saturate(dot(s.Normal, viewDir));

        //    float H = normalize(lightDir + viewDir);
        //    float spec = dot(s.Normal, H);

        //    float4 ramp = tex2D(_RampTex, float2(ndotl, ndotv));

        //    float4 final;
        //    final.rgb = (s.Albedo.rgb * ramp.rgb) + (ramp.rgb * 0.1);
        //    final.a = s.Alpha;
        //    return final;
        //}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 lightDir = normalize(UnityWorldSpaceLightDir(IN.worldPos));

            float ndotl = dot(IN.worldNormal, lightDir)*0.5 +0.5;

            float ndotv = saturate(dot(IN.worldNormal, IN.viewDir));

            float4 ramp = tex2D(_RampTex, float2(ndotl, ndotv));

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            //o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
            o.Smoothness = tex2D(_RoughnessTex, IN.uv_RoughnessTex).r;
            o.Metallic = tex2D(_MetalTex, IN.uv_MetalTex).r;
            float3 rampRGB = ramp.rgb;
            o.Albedo = (c.rgb * rampRGB) + (rampRGB * 0.1);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
