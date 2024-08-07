Shader "Custom/ARMToon"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_RampTex("RampTex (RGB)", 2D) = "white" {}
		_ARMTex("ARMTex (RGB)", 2D) = "white" {}
		_NormalTex("NormalTex (RGB)", 2D) = "white" {}
		//_OpacityTex("OpacityTex (RGB)", 2D) = "white" {}
		//_EmissionTex("_EmissionTex (RGB)", 2D) = "white" {}
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
		//sampler2D _OpacityTex;
		//sampler2D _EmissionTex;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_RampTex;
			float2 uv_ARMTex;
			float2 uv_NormalTex;
			//float2 uv_OpacityTex;
			//float2 uv_EmissionTex;
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

				float4 ramp = tex2D(_RampTex, float2(ndotl, ndotv));

				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 arm = tex2D(_ARMTex, IN.uv_ARMTex);
				//o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
				o.Smoothness = arm.g;
				o.Metallic = arm.b;
				//o.Emission = tex2D(_EmissionTex, IN.uv_EmissionTex).rgb;
				float3 rampRGB = ramp.rgb;
				o.Albedo = (c.rgb * rampRGB);
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}
