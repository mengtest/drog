Shader "Custom/Scene/CustomTerrainDiffuse" 
{
	Properties
	{
		_Control("Control (RGBA)", 2D) = "red" {}
		_Splat3("Layer 3 (A)", 2D) = "black" {}
		_Splat2("Layer 2 (B)", 2D) = "black" {}
		_Splat1("Layer 1 (G)", 2D) = "black" {}
		_Splat0("Layer 0 (R)", 2D) = "black" {}
		_LightMap("LightMap", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry-99" "RenderType" = "Opaque"}
		Pass
		{
			Tags { "LightMode" = "Vertex" }
			CGPROGRAM
			#pragma vertex vertT
			#pragma fragment fragT
			#pragma multi_compile_fog
			#pragma multi_compile __ CUSTOM_SHADOW_ON 
			#pragma target 3.0
			#define TERRAIN
			#define TERRAIN_FIRSTPASS
			#define LM
			#define INPUT_LIGHTMAP
			#include "../Include/SceneHead_Include.cginc"
			#include "../Include/Scene_Include.cginc"
			ENDCG
		}		
	}
}
