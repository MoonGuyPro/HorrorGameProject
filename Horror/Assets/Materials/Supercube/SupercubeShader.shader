Shader "Custom/SupercubeShader"
{
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
        //Cull Front

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

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

        void vert (inout appdata_full v) {
            
            float _TimeScale = 100;
            float _DeformScale = 0.05;
            float3 oldVertex = v.vertex;
            float oldX = v.vertex.x;
            v.vertex.x += cos(_Time * _TimeScale * v.vertex.y * 1.1) * _DeformScale;
            v.vertex.y += sin(_Time * _TimeScale * v.vertex.z * 1.2 + 1) * _DeformScale;
            v.vertex.z += cos(_Time * _TimeScale * oldX + 2) * _DeformScale;

            //v.normal += normalize(v.vertex - oldVertex);
            
            /*float3 ddxPos = ddx(v.vertex);
            float3 ddyPos = ddy(v.vertex)  * _ProjectionParams.x;
            v.normal = normalize( cross(ddxPos, ddyPos));*/

        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            /*if(min(IN.uv_MainTex.x, IN.uv_MainTex.y) < 0.01
                || max(IN.uv_MainTex.x, IN.uv_MainTex.y) > 0.99)
            {
                o.Albedo = fixed3(1, 1, 1);
            }
            else
            {
                o.Albedo = fixed3(0, 0, 0);
            }*/
            // Metallic and smoothness come from slider variables
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Alpha = 0.1;
            o.Emission = c.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
