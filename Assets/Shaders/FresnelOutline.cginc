#if !defined(FRESNEL_INCLUDED)
#define FRESNEL_INCLUDED

struct appdata {
  float4 vertex : POSITION;
  float3 normal : NORMAL;
  float4 tangent : TANGENT;
  float2 uv : TEXCOORD0;
};

struct v2f {
  float4 vertex : SV_POSITION;  // has to be named 'vertex' to work with fog
  float3 normal : NORMAL;
  float4 tangent : TANGENT;
  float4 uv_tex : TEXCOORD0;
  float4 uv_normal : TEXCOORD1;
  float4 uv_fres : TEXCOORD2;
  float2 uv_splat : TEXCOORD3;
  float3 worldPos : TEXCOORD4;
  UNITY_FOG_COORDS(5)
};



half4 _OutlineColor;
float _FresnelThickness;
sampler2D _Texture, _FresnelMap, _NormalMap,
          _Texture2, _FresnelMap2, _NormalMap2,
          _SplatMap;
float4    _Texture_ST, _FresnelMap_ST, _NormalMap_ST,
          _Texture2_ST, _FresnelMap2_ST, _NormalMap2_ST,
          _SplatMap_ST;
float _BumpScale;
float _AlphaCutoff;

float splat = 0;



////////////////////
// helper methods //
////////////////////

#include "NormalHelper.cginc"
#include "MintFresnel.cginc"



/////////////////////
// shader programs //
/////////////////////

v2f vert (appdata v) {
  v2f o;
  o.vertex = UnityObjectToClipPos(v.vertex);
  // NOTE: Secondary textures are not supported with empty primary textures,
  // but it DOES work the other way around - secondary textures can be empty.
  #if defined(_INVERSION_MAP)
    o.uv_tex.xy = TRANSFORM_TEX(v.uv, _Texture);
    #if defined(_SPLAT_MAP)
      o.uv_tex.zw = TRANSFORM_TEX(v.uv, _Texture2);
    #endif
  #endif
  #if defined(_NORMAL_MAP)
    o.uv_normal.xy = TRANSFORM_TEX(v.uv, _NormalMap);
    #if defined(_SPLAT_MAP)
      o.uv_normal.zw = TRANSFORM_TEX(v.uv, _NormalMap2);
    #endif
  #endif
  #if defined(_FRESNEL_MAP)
    o.uv_fres.xy = TRANSFORM_TEX(v.uv, _FresnelMap);
    #if defined(_SPLAT_MAP)
      o.uv_fres.zw = TRANSFORM_TEX(v.uv, _FresnelMap2);
    #endif
  #endif
  #if defined(_SPLAT_MAP)
    o.uv_splat.xy = TRANSFORM_TEX(v.uv, _SplatMap);
  #endif
  o.normal = UnityObjectToWorldNormal(v.normal);
  o.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
  o.worldPos = mul(unity_ObjectToWorld, v.vertex);
  UNITY_TRANSFER_FOG(o, o.vertex);
  return o;
}

fixed4 frag (v2f i) : SV_Target {
  #if defined(_SPLAT_MAP)
    splat = tex2D(_SplatMap, i.uv_splat);
  #endif
  InitializeFragmentNormal(i);

  float inversion = GetInversion(i);
  float brightness = GetBrightness(i);
  float fresnel = GetFresnel(i, inversion, brightness);

  fixed4 col = _OutlineColor * fresnel;
  UNITY_APPLY_FOG(i.fogCoord, col);
  return col;
}

#endif
