// Get brightness map texture's brightness at this pixel
float GetInversion(v2f i) {
  float inv;
  #if defined(_INVERSION_MAP)
    #if defined(_SPLAT_MAP)
      inv = lerp(
        tex2D(_Texture, i.uv_tex.xy).r,
        tex2D(_Texture2, i.uv_tex.zw).r,
        splat
      );
    #else
      inv = tex2D(_Texture, i.uv_tex.xy).r;
    #endif
  #else
    inv = 0;
  #endif
  return inv;
}

// Get brightness map influence
float GetBrightness(v2f i) {
  // Gray by default
  float brightness = 0.5;
  float alpha = 1;
  #if defined(_FRESNEL_MAP)
    #if defined(_SPLAT_MAP)
      float4 sample = lerp(
        tex2D(_FresnelMap, i.uv_fres.xy),
        tex2D(_FresnelMap2, i.uv_fres.zw),
        splat
      );
    #else
      float4 sample = tex2D(_FresnelMap, i.uv_fres.xy); // TODO: check if removing ".g" from here broke anything
    #endif
    brightness = sample.g;
    #if defined(_RENDERING_CUTOUT)
      alpha = sample.a;
      clip(alpha - _AlphaCutoff);
    #endif
  #endif
  // Multiply by 2 to account for _FresnelMap
  return brightness * 2;
}

// Calculate fresnel effect influence. Use results of brightness and inversion maps
float GetFresnel(v2f i, float inversion, float brightness) {
  // Interpolating viewDir gives noticeably imprecise results
  float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
  float ndotl = saturate(dot(viewDir, i.normal));
  // 0.9 instead of 1 here is just a magic number that makes the brightness falloff nicer
  float finalBrightness = (0.9 - inversion) * ndotl + inversion * (0.9 - ndotl);
  return (_FresnelThickness - finalBrightness) / _FresnelThickness * brightness;
}
