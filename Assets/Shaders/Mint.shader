Shader "Custom/Mint" { 
  Properties {
    _OutlineColor ("Outline color", Color) = (0.3215686, 0.6392157, 0.490196, 1)
    _FresnelThickness ("Fresnel thickness", Range(0, 3)) = 0.5
    _OutlineThickness ("Outline thickness", Range(0.002, 0.03)) = 0.005
    // Black = no effect, white = inverted
    _Texture ("Fresnel inversion", 2D) = "black" {}
    _Texture2 ("Secondary inversion", 2D) = "black" {}
    // Black = 0, white = 2
	  _FresnelMap ("Fresnel brightness map", 2D) = "gray" {}
    _FresnelMap2 ("Secondary brightness", 2D) = "gray" {}
    [Normal] _NormalMap ("Normal map", 2D) = "bump" {}
    [Normal] _NormalMap2 ("Secondary normals", 2D) = "bump" {}
    _BumpScale ("Bump scale", Range(-5, 5)) = 1
    _AlphaCutoff ("Alpha cutoff", Range(0, 1)) = 0.5
    // Black = primary tex, white = secondary tex
    _SplatMap ("Splat map", 2D) = "black" {}  // TODO: NoScaleOffset this

    [HideInInspector] _CullMode ("Cull mode", Int) = 2
    [HideInInspector] _SrcBlend ("_SrcBlend", Int) = 1
    [HideInInspector] _DstBlend ("_DstBlend", Int) = 0
    [HideInInspector] _ZWrite ("_ZWrite", Int) = 1
  }
  SubShader {

    // Toon outline
    Pass {
      Name "Toon Outline"
      Tags {"LightMode" = "ForwardBase"}
      Cull Front

      CGPROGRAM

      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fog
      #pragma shader_feature _RENDERING_CUTOUT
      #pragma shader_feature _RENDERING_TRANSPARENT
      #pragma shader_feature _ADAPTIVE
      #pragma shader_feature _FRESNEL_MAP

      #include "ToonOutline.cginc"

      ENDCG
    }

    // Fresnel effect
    Pass {
      Name "Fresnel Outline"
      Tags {"LightMode" = "ForwardBase"}
      Cull [_CullMode]
      Blend [_SrcBlend] [_DstBlend]
      ZWrite [_ZWrite]

      CGPROGRAM

      #pragma target 3.0  // required for UnpackScaleNormal() to take _BumpScale
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fog
      #pragma shader_feature _RENDERING_CUTOUT
      #pragma shader_feature _NORMAL_MAP
      #pragma shader_feature _FRESNEL_MAP
      #pragma shader_feature _INVERSION_MAP
      #pragma shader_feature _SPLAT_MAP

      #include "UnityCG.cginc"
      #include "UnityPBSLighting.cginc" // UnpackScaleNormal

      #include "FresnelOutline.cginc"

      ENDCG
    }
  }
  CustomEditor "MintShaderEditor"
  Fallback "VertexLit"  // Need this for the ShadowCaster pass so we render to
                        // the depth buffer
}
