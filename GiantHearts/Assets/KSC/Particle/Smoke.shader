Shader "Custom/Smoke"
{
    Properties
    {
        _MainTex("MainTex (RGB)",2D) = "white" {}
         _Color("Color", Color) = (1,1,1,1)
         _RimColor("RimColor", Color) = (1,1,1,1)
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:blend noshadow noambient

        #pragma target 3.0

        fixed4 _Color;
        fixed4 _RimColor;

        struct Input
        {
            float3 viewDir;
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // rim
            float rim = dot(o.Normal, IN.viewDir);
            rim = pow(1 - rim, 2);
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = _Color.rgb;
            o.Emission = (_RimColor.rgb * rim);
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = 0.6;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
