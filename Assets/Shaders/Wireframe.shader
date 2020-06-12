// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Geometry/Wireframe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [PowerSlider(3.0)]
        _WireframeVal ("Wireframe width", Range(0., 0.34)) = 0.05
        _FrontColor ("Front color", color) = (1., 1., 1., 1.)
        _BackColor ("Back color", color) = (1., 1., 1., 1.)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
 
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};
 
            struct v2g {
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float3 worldPos : TEXCOORD1;
            };
 
            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
                float3 normal : NORMAL;
                float3 worldPos : TEXCOORD1;
            };
 
            v2g vert(appdata v) {
                v2g o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
 
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                g2f o;
                o.pos = IN[0].pos;
                o.bary = float3(1., 0., 0.);
                o.normal = IN[0].normal;
                o.worldPos = IN[0].worldPos;
                triStream.Append(o);
                o.pos = IN[1].pos;
                o.bary = float3(0., 0., 1.);
                o.normal = IN[1].normal;
                o.worldPos = IN[1].worldPos;
                triStream.Append(o);
                o.pos = IN[2].pos;
                o.bary = float3(0., 1., 0.);
                o.normal = IN[2].normal;
                o.worldPos = IN[2].worldPos;
                triStream.Append(o);
            }
 
            float _WireframeVal;
            fixed4 _FrontColor, _BackColor;
 
            fixed4 frag(g2f i) : SV_Target {
            if(!any(bool3(i.bary.x < _WireframeVal, i.bary.y < _WireframeVal, i.bary.z < _WireframeVal)))
                 discard;
 
				float3 viewDir = _WorldSpaceCameraPos - i.worldPos;
                float dotProduct = dot(viewDir, i.normal);
                if(dotProduct > 0) return _FrontColor;
                return _BackColor;
            }
 
            ENDCG
        }
 
    }
}