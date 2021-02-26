//Credit: todo
Shader "Custom/ToonTest"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0,0.1)) = 0.03
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white"{}
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "Queue" = "Geometry"
        }

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 _OutlineColor;
            fixed4 _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                //float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                //float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 normal = normalize(v.normal);
                float3 outlineOffset = normal * _OutlineThickness;
                float3 position = v.vertex + outlineOffset;
                o.position = UnityObjectToClipPos(position);
                //o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                return _OutlineColor;
                /*
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                return 0;
                */
                //return fixed4(i.uv.x, i.uv.y, 0, 1);
            }
            ENDCG
        }
        Pass
        {
            Cull front

            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            fixed4 _Color;

            fixed4 _OutlineColor;
            fixed4 _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                //float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                //float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 normal = normalize(v.normal);
                float3 outlineOffset = normal * _OutlineThickness;
                float3 position = v.vertex + outlineOffset;
                o.position = UnityObjectToClipPos(position);
                //o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                return _OutlineColor;
                /*
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                return 0;
                */
                //return fixed4(i.uv.x, i.uv.y, 0, 1);
            }
            ENDCG

        }
    }
    Fallback "Standard"
}