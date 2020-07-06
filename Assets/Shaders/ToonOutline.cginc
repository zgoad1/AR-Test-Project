#if !defined(TOON_OUTLINE_INCLUDED)
#define TOON_OUTLINE_INCLUDED

#include "UnityCG.cginc"

struct appdata {
  float4 vertex : POSITION;
  float3 normal : NORMAL;
  #if defined(_RENDERING_CUTOUT) && defined(_FRESNEL_MAP)
    float2 uv : TEXCOORD0;
  #endif
  #if defined(_ADAPTIVE)
    float3 normalsXY : TEXCOORD2; // set in OutlineSmoother
    float3 normalsZW : TEXCOORD3;
  #endif
};

struct v2f {
  float4 vertex : SV_POSITION;
  #if defined(_RENDERING_CUTOUT) && defined(_FRESNEL_MAP)
    float2 uv : TEXCOORD0;
  #endif
  UNITY_FOG_COORDS(1)
};



half4 _OutlineColor;
float _OutlineThickness;
sampler2D _FresnelMap;
float4 _FresnelMap_ST;
float _AlphaCutoff;



v2f vert(appdata v) {
  v2f o;

  #if !defined(_RENDERING_TRANSPARENT)
    float3 normal;
    #if defined(_ADAPTIVE)
      normal = float3(v.normalsXY.x, v.normalsXY.y, v.normalsZW.x);
    #else
      normal = v.normal;
    #endif

    #if defined(_RENDERING_CUTOUT) && defined(_FRESNEL_MAP)
      o.uv = TRANSFORM_TEX(v.uv, _FresnelMap);
    #endif

    o.vertex = UnityObjectToClipPos(v.vertex);

    // Project the outline outward & keep screen-space size
    float3 fixedNorm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));
    float2 offset = TransformViewToProjection(fixedNorm.xy);
    #ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE
      o.vertex.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z) * _OutlineThickness;
    #else
      o.vertex.xy += offset * o.vertex.z * _OutlineThickness;
    #endif

    UNITY_TRANSFER_FOG(o, o.vertex);

    return o;
  #else
    o.vertex = float4(0, 100000, 0, 0);
    return o;
  #endif
}

half4 frag(v2f i) : SV_TARGET {
  #if !defined(_RENDERING_TRANSPARENT)
    #if defined(_RENDERING_CUTOUT) && defined(_FRESNEL_MAP)
      float alpha = tex2D(_FresnelMap, i.uv).a;
      clip(alpha - _AlphaCutoff);
    #endif
    half4 col = _OutlineColor;
    UNITY_APPLY_FOG(i.fogCoord, col);
    return col;
  #else
    return float4(0, 0, 0, 0);
  #endif
}

#endif
