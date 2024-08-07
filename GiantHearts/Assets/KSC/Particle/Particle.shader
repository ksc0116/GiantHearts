Shader "Custom/Particle"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _RimColor("RimColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:blend noambient noshadow

        #pragma target 3.0

        struct Input
        {
            float3 viewDir;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _RimColor;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float rim = (dot(o.Normal,IN.viewDir));
            rim = (pow(rim, 4));
            rim = smoothstep(0.0, 0.05, rim);
            o.Albedo = (_RimColor.rgb * rim);
            o.Emission = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 0.3;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
