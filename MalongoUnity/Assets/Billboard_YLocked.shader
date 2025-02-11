// Original shader from https://gist.github.com/kaiware007/8ebad2d28638ff83b6b74970a4f70c9a
// Adapted to URP with instructions from https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.0/manual/writing-shaders-urp-basic-unlit-structure.html
// And using code from https://gist.github.com/kaiware007/8ebad2d28638ff83b6b74970a4f70c9a?permalink_comment_id=4464811#gistcomment-4464811

Shader "Unlit/Billboard_YLocked"
{
	Properties
	{
		[MainTexture] _BaseMap ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "DisableBatching" = "True" "RenderPipeline" = "UniversalPipeline" }

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			HLSLPROGRAM 
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			struct Attributes
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseMap_ST;
			CBUFFER_END



Varyings vert(Attributes IN)
{	
    Varyings OUT;
    OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

    // Get world position of the object (center pivot)
    float3 worldPos = TransformObjectToWorld(float3(0, 0, 0));
    
    // Get camera position
    float3 cameraPos = _WorldSpaceCameraPos;

    // Compute forward direction (Z-axis), locked to Y-axis
    float3 toCam = cameraPos - worldPos;
    toCam.y = 0; // Remove vertical component (locking Y-axis)
    toCam = normalize(toCam); // Re-normalize after Y-lock

    // Compute right (X-axis) and up (Y-axis) for the quad
    float3 up = float3(0, 1, 0); // Fixed world up
    float3 right = normalize(cross(up, toCam)); // Correct X-axis direction

    // Reconstruct final vertex position relative to the object's center
    float3 finalWorldPos = worldPos + (right * IN.vertex.x) + (up * IN.vertex.y);

    // Convert to clip space for rendering
    OUT.pos = TransformWorldToHClip(finalWorldPos);

OUT.pos = float4(0,0,0,1);

    return OUT;
}


			
			half4 frag (Varyings IN) : SV_Target
			{
				half4 texel = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
				return texel;
			}
			ENDHLSL
		}
	}
}