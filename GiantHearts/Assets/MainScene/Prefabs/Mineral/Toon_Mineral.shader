Shader "Custom/Toon_Mineral"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_RampTex("RampTex (RGB)", 2D) = "white" {}
		_ARMTex("ARMTex (RGB)", 2D) = "white" {}
		_NormalTex("NormalTex (RGB)", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _RampTex;
		sampler2D _ARMTex;
		sampler2D _NormalTex;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_RampTex;
			float2 uv_ARMTex;
			float2 uv_NormalTex;
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

				//float ndotl = dot(IN.worldNormal, lightDir) * 0.5 + 0.5;
				float ndotl = dot(o.Normal, lightDir) * 0.5 + 0.5;

				//float ndotv = saturate(dot(IN.worldNormal, IN.viewDir));
				float ndotv = saturate(dot(o.Normal, IN.viewDir));

				float4 ramp = tex2D(_RampTex, float2(ndotl, ndotv));

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 arm = tex2D(_ARMTex, IN.uv_ARMTex);
				o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
				o.Smoothness = arm.g;
				o.Metallic = arm.b;
				float3 rampRGB = ramp.rgb;
				o.Albedo = (c.rgb * rampRGB) +(rampRGB * 0.01);
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}
