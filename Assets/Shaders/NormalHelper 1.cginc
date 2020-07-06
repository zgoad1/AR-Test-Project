float3 GetTangentSpaceNormal(v2f i) {
  float3 normal = float3(0, 0, 1);
  #if defined(_NORMAL_MAP)
    #if defined(_SPLAT_MAP)
      // idk the correct way to partially blend 2 normals
      normal = lerp(
        UnpackScaleNormal(tex2D(_NormalMap, i.uv_normal.xy), _BumpScale),
        UnpackScaleNormal(tex2D(_NormalMap2, i.uv_normal.zw), _BumpScale),
        splat
      );
    #else
      normal = UnpackScaleNormal(tex2D(_NormalMap, i.uv_normal.xy), _BumpScale);
    #endif
  #endif
  return normal;
}

void InitializeFragmentNormal(inout v2f i) {
  float3 normal = GetTangentSpaceNormal(i);
  // using i.normal here fixes the north-south problem
  float3 binormal = cross(i.normal, i.tangent.xyz)
    * i.tangent.w * unity_WorldTransformParams.w;

  i.normal = normalize(
    normal.x * i.tangent +
    normal.y * binormal +
    normal.z * i.normal
  );
}
