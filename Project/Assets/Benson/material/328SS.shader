Shader "Custom/328SS"
{
    /*
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
    */
    Properties{
        _MainTex("Texture", 2D) = "white" {}
    }

        SubShader{
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            #pragma surface surf Standard

            sampler2D _MainTex;

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            void surf(Input IN, inout SurfaceOutputStandard o) {
                // Calculate the gray-scale value of the texture
                float gray = dot(tex2D(_MainTex, IN.uv_MainTex).rgb, float3(0.299, 0.587, 0.114));

                // Apply a power curve to create a stylized effect
                gray = pow(gray, 2.0);

                // Adjust the brightness and contrast of the color based on the gray-scale value
                float3 color = lerp(float3(1.0, 1.0, 1.0), tex2D(_MainTex, IN.uv_MainTex).rgb, gray);

                // Apply gamma correction to the final color
                color = pow(color, 2.2);

                // Set the output color and opacity
                o.Albedo = color;
                o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;
            }
            ENDCG
    }

        FallBack "Diffuse"
}
