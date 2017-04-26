// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Magnifying Glass"
{

	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" {}
	}

		SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Tags{ "RenderType" = "Transparent" }
		LOD 100

		Pass
	{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"

		struct VertIn
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 tangent : TANGENT;
		float2 uv : TEXCOORD0;
	};

	struct FragIn
	{
		float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
		float3 screen_uv : TEXCOORD1;
	};

	FragIn vert(VertIn v)
	{
		float4 pos;
		pos.xy = v.uv;

		pos.xy = 1 - pos.xy;

		pos.z = 0;
		pos.w = 1;

		FragIn o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = pos;

		return o;
	}

	sampler2D _MainTex;
	sampler2D _TimeCrackTexture;

	fixed4 frag(FragIn i) : SV_Target
	{

		// Perspective correction for screen uv coordinate
		// float2 screen_uv = i.screen_uv.xy / i.screen_uv.z;

		// Do the thing!

		float a = tex2D(_TimeCrackTexture, i.uv).a;
		if (a == 0) discard;
		
		fixed4 col = tex2D(_MainTex, i.uv) * a;
		
		return col;
	}
		ENDCG
	}
	}
}
