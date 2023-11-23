Shader "Hidden/Roystan/Post Process Outline"
{
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
			// Custom post processing effects are written in HLSL blocks,
			// with lots of macros to aid with platform differences.
			// https://github.com/Unity-Technologies/PostProcessing/wiki/Writing-Custom-Effects#shader
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

			#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"


			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			// _CameraNormalsTexture contains the view space normals transformed
			// to be in the 0...1 range.
			TEXTURE2D_SAMPLER2D(_CameraNormalsTexture, sampler_CameraNormalsTexture);
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
        
			// Data pertaining to _MainTex's dimensions.
			// https://docs.unity3d.com/Manual/SL-PropertiesInPrograms.html
			float4 _MainTex_TexelSize;
			float _Scale;
            float _LowCutOff;
            float _FadeOutPower;
            float _FadeOutDistance;
            float _BrightnessClamp;
            float _BrightnessScale;
            float _DryBlend;
			float4x4 _ClipToView;

			float halfScaleFloor;
			float halfScaleCeil;

			float3 normal0;
			float3 normal1;
			float3 normal2;
			float3 normal3;

			float3 normalFiniteDifference0;
			float3 normalFiniteDifference1;
			float3 normalEdge;
			float4 color;
			float depth;
			float edgeValue;

			// Combines the top and bottom colors using normal blending.
			// https://en.wikipedia.org/wiki/Blend_modes#Normal_blend_mode
			// This performs the same operation as Blend SrcAlpha OneMinusSrcAlpha.
			/*float4 alphaBlend(float4 top, float4 bottom)
			{
				float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
				float alpha = top.a + bottom.a * (1 - top.a);

				return float4(color, alpha);
			}*/

			struct Varyings
			{
				float4 vertex : SV_POSITION;
				float3 viewSpaceDir : TEXCOORD2;
				float2 texcoord : TEXCOORD0;
				float2 texcoordStereo : TEXCOORD1;
			#if STEREO_INSTANCING_ENABLED
				uint stereoTargetEyeIndex : SV_RenderTargetArrayIndex;
			#endif
				float2 bottomLeftUV : TEXCOORD3;
				float2 topRightUV : TEXCOORD4;
				float2 bottomRightUV : TEXCOORD5;
				float2 topLeftUV : TEXCOORD6;
			};

			Varyings Vert(AttributesDefault v)
			{
				Varyings o;
				o.vertex = float4(v.vertex.xy, 0.0, 1.0);
				o.viewSpaceDir = mul(_ClipToView, o.vertex).xyz;
				o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);

			#if UNITY_UV_STARTS_AT_TOP
				o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
			#endif

				// Calculate UV for comparison
				halfScaleFloor = floor(_Scale * 0.5);
				halfScaleCeil = ceil(_Scale * 0.5);

				o.texcoordStereo = TransformStereoScreenSpaceTex(o.texcoord, 1.0);
				o.bottomLeftUV = o.texcoord - float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleFloor;
				o.topRightUV = o.texcoord + float2(_MainTex_TexelSize.x, _MainTex_TexelSize.y) * halfScaleCeil;
				o.bottomRightUV = o.texcoord + float2(_MainTex_TexelSize.x * halfScaleCeil, -_MainTex_TexelSize.y * halfScaleFloor);
				o.topLeftUV = o.texcoord + float2(-_MainTex_TexelSize.x * halfScaleFloor, _MainTex_TexelSize.y * halfScaleCeil);
				return o;
			}

			float4 Frag(Varyings i) : SV_Target
			{
				// Get normals
				normal0 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, i.bottomLeftUV).rgb;
				normal1 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, i.topRightUV).rgb;
				normal2 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, i.bottomRightUV).rgb;
				normal3 = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, i.topLeftUV).rgb;

				// Calculate edge
				normalFiniteDifference0 = normal1 - normal0;
				normalFiniteDifference1 = normal3 - normal2;
				normalEdge = sqrt(pow(normalFiniteDifference0, 2) + pow(normalFiniteDifference1, 2));

				// Color pre post-processing
				color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
					
				// Get depth from buffor
				depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord).r;

				// Convert to single float (don't ask me why I used such formula, it just works)
				edgeValue = max(normalEdge.r, max(normalEdge.g, normalEdge.b));
				
				// Cut off very low values
				edgeValue = edgeValue > _LowCutOff ? edgeValue : 0;

				// Krystian here - this is actually a terrible idea, because it gives away where portals are
				// Decrease color at distance using depth buffor
				//color *= clamp(pow(depth, _FadeOutPower) * _FadeOutDistance, 0, 0.5);
				
				// Mix edge with color for output
				color = clamp(color, 0, _BrightnessClamp);
				color *= _BrightnessScale;

				// this comment is a good thing to consider, it can be used to make the surfaces have a slight color to them so the game could be a bit more colorful and visible
				return edgeValue * color + color * _DryBlend; 
			}
			ENDHLSL
		}
    }
}