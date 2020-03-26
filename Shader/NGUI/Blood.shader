﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/UI/Blood" 
{
	Properties {}

	SubShader
	{
		Pass
		{
			Tags{ "RenderType" = "Opaque" }
			Cull Back ZWrite On Fog{ Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t 
			{
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f 
			{
				half4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};


			v2f vert(appdata_t v)
			{
				v2f o = (v2f)0;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
		}
	}
}
