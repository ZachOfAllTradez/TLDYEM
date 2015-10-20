Shader "UChromaKey/Bumped transparent" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)	
	_CKCol ("ChromaKey Color", Color) = (1,1,1,1)	
	_Range ("Range", Range (0.0, 2.83)) = 0.01
	_HueRange ("Hue Range", Range (0.0, 5.0)) = 0.1
	_EdgeSharp ("Edge sharpness", Range (1.0, 20.0)) = 20.0
	_Opacity ("Opacity", Range (0.0, 1.0)) = 1.0
}
SubShader {
	Tags{ "Queue" = "Transparent"}
	Blend SrcAlpha OneMinusSrcAlpha
	LOD 300
	
CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0


	sampler2D _BumpMap;
	sampler2D _UChromaKeyTex;	
			
	fixed4 _Color;
	fixed4 _CKCol;
	fixed4 _NewColor;
	half _Range;
	half _HueRange;
	half _EdgeSharp;
	half _uvDefX;		
	half _uvCoefX;		
	half _uvDefY;		
	half _uvCoefY;
	half _Opacity;
	
struct Input {
	float2 uv_UChromaKeyTex;
	float2 uv_BumpMap;	
};

void surf (Input IN, inout SurfaceOutput o) {
	half2 nuv;
	nuv.x = (_uvDefX + _uvCoefX * IN.uv_UChromaKeyTex.x);
	nuv.y = (_uvDefY + _uvCoefY * IN.uv_UChromaKeyTex.y);
	fixed4 c = tex2D(_UChromaKeyTex, nuv) * _Color.a;
	
	half hueDiff = abs(atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001) - atan2(1.73205 * (_CKCol.g - _CKCol.b), 2 * _CKCol.r - _CKCol.g - _CKCol.b + 0.001));
	
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	o.Albedo = c.rgb;	
	o.Alpha = (1 - saturate((1 - ((c.r - _CKCol.r)*(c.r - _CKCol.r) + (c.g - _CKCol.g)*(c.g - _CKCol.g) + (c.b - _CKCol.b)*(c.b - _CKCol.b)) / (_Range * _Range)) * _EdgeSharp)
				                  * saturate(1.0 - min(hueDiff,6.28319 - hueDiff)/(_HueRange * _HueRange)) * _EdgeSharp) * _Opacity;
}
ENDCG
}

FallBack "Transparent/VertexLit"
}