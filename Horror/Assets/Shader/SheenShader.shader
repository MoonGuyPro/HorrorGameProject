Shader "Custom/SheenMovingLine"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("Sheen Color", Color) = (1, 0.5, 0.2, 1)
        _SheenIntensity ("Sheen Intensity", Float) = 1.0
        _SheenWidth ("Sheen Width", Float) = 0.2
        _SheenSpeed ("Sheen Speed", Float) = 2.0
        _SheenAngle ("Sheen Angle", Range(0, 360)) = 45
    }
    SubShader
    {   
        Tags {  "Queue"="Overlay+100" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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
            float4 _Color;
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
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Œwiatowa pozycja wierzcho³ka
                return o;
            }

            float MovingSheen(float3 worldPos, float time, float width, float speed, float angle)
            {
                float radians = angle * 0.0174533f;// (3.14159265 / 180.0); // Konwersja stopni na radiany
                float2 direction = float2(cos(radians), sin(radians)); // Wektor kierunku

                // Projekcja pozycji œwiata na wektor kierunku
                float sheenPosition = dot(worldPos.xy, direction) + time * speed;

                // Tworzenie efektu linii wzd³u¿ okreœlonego kierunku
                float sheen = smoothstep(0.0, width, frac(sheenPosition));

                return sheen;
            }

            half4 frag(v2f i) : SV_Target
            {
                float time = _Time.y;
                half4 baseColor = tex2D(_MainTex, i.uv);

                float sheenMask = MovingSheen(i.worldPos, time, _SheenWidth, _SheenSpeed, _SheenAngle);

                float3 finalColor = lerp(_Color.rgb, baseColor.rgb, sheenMask);
                float alpha = 1 - sheenMask; // Przezroczystoœæ poza lini¹

                return half4(finalColor, alpha);
            }
            ENDCG
        }
    }
}
