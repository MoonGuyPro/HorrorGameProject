Shader "Custom/BaseColorWithSheen"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0, 1, 1) // Bazowy kolor obiektu
        _EmissionColor ("Emission Color", Color) = (0, 0, 1, 1) // Kolor emisji
        _EmissionStrength ("Emission Strength", Float) = 1.0 // Intensywnoœæ emisji

        _MainTex ("Base Texture", 2D) = "white" {} // Tekstura bazowa
        _SheenColor ("Sheen Color", Color) = (1, 0.5, 0.2, 1) // Kolor migotania
        _SheenIntensity ("Sheen Intensity", Float) = 1.0 // Intensywnoœæ migotania
        _SheenWidth ("Sheen Width", Float) = 0.2 // Szerokoœæ linii migotania
        _SheenSpeed ("Sheen Speed", Float) = 2.0 // Szybkoœæ przesuwania
        _SheenAngle ("Sheen Angle", Range(0, 360)) = 45 // K¹t migotania
    }
    
    SubShader
    {   
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 worldNormal : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionStrength;

            float4 _SheenColor;
            float _SheenIntensity;
            float _SheenWidth;
            float _SheenSpeed;
            float _SheenAngle;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            float MovingSheen(float3 worldPos, float time, float width, float speed, float angle)
            {
                float radians = angle * 0.0174533f; // Zamiana stopni na radiany
                float2 direction = float2(cos(radians), sin(radians));
                float sheenPosition = dot(worldPos.xy, direction) + time * speed;
                return smoothstep(0.0, width, frac(sheenPosition));
            }

            half4 frag(v2f i) : SV_Target
            {
                float time = _Time.y;
                half4 baseColor = tex2D(_MainTex, i.uv) * _BaseColor; // Pobranie koloru obiektu
                
                // Efekt Sheen (migotania)
                float sheenMask = MovingSheen(i.worldPos, time, _SheenWidth, _SheenSpeed, _SheenAngle);
                
                // Mieszanie koloru migotania z bazowym
                float3 finalColor = lerp(baseColor.rgb, _SheenColor.rgb, sheenMask * _SheenIntensity);
                
                // Dodanie efektu emisji
                float3 emission = _EmissionColor.rgb * _EmissionStrength * sheenMask;

                return half4(finalColor + emission, 1.0);
            }
            ENDCG
        }
    }
}
