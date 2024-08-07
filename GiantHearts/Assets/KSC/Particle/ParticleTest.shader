Shader "Custom/ParticleTest"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _OutLine("Outline",Range(0.01,1)) = 1
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            cull front

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf NoLight vertex:vert noshadow noambient

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            float _OutLine;


            void vert(inout appdata_full v)
            {
                v.vertex.xyz = v.vertex.xyz + v.vertex.xyz * _OutLine;
            }

            struct Input
            {
                float4 color:COLOR;
            };

            void surf(Input IN, inout SurfaceOutput o)
            {
            }

             float4 LightingNoLight(SurfaceOutput s,float3 lightDir,float atten)
             {
                 return float4(s.Albedo,1);
             }
            ENDCG

            cull back

                //2pass
                CGPROGRAM
                // Physically based Standard lighting model, and enable shadows on all light types
                #pragma surface surf Toon noambient

                // Use shader model 3.0 target, to get nicer looking lighting
                #pragma target 3.0

                sampler2D _MainTex;

                struct Input
                {
                    float2 uv_MainTex;
                };

                void surf(Input IN, inout SurfaceOutput o)
                {
                    // Albedo comes from a texture tinted by color
                    fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                    o.Albedo = c.rgb;
                    o.Alpha = c.a;
                }

                float4 LightingToon(SurfaceOutput s, float3 lightDir,float viewDir, float atten)
                {
                    /*float ndotl = dot(s.Normal,lightDir) * 0.5 + 0.5;

                    if (ndotl >= 0.5)
                    {
                        ndotl = 1;
                    }
                    else
                    {
                        ndotl = 0.3;
                    }

                     return ndotl;*/
                    return float4(1,1,1,1);
                }
                ENDCG
        }
            FallBack "Diffuse"
}
