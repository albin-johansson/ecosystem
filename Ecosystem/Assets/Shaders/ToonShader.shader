Shader "Custom/ToonTest"
{
    Properties
    {
        _Color ("Tint", Color) = (0,0,0,1)
        _MainTex ("Texture", 2D) = "white"{}
        _Smoothness ("Smoothness", Range(0,1)) = 0
        _Metallic ("Metalness", Range(0,1)) = 0
        [HDR] _Emission ("Emission", Color) = (0,0,0)

        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0,1)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "Queue" = "Geometry"
        }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;

        half _Smoothness;
        half _Metallic;
        half3 _Emission;

        struct Input
        {
            float uv_MainTex;
        };

        void surf(Input i, inout SurfaceOutputStandard o)
        {
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            col *= _Color;
            o.Albedo = col.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Emission = _Emission;
        }
        ENDCG

        /*
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            fixed4 _OutlineColor;
            fixed4 _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                //float3 normal = normalize(v.normal);
                //float3 outlineOffset = normal * _OutlineThickness;
                //float3 position = v.vertex + outlineOffset;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET
            {
                //return _OutlineColor;

                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                return col;

                //return fixed4(i.uv.x, i.uv.y, 0, 1);
            }
            ENDCG
        }
        */
        Pass
        {
            Cull Front

            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag


            //sampler2D _MainTex;
            //fixed4 _Color;

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
                //o.position = UnityObjectToClipPos(v.vertex + normalize(v.normal) * _OutlineThickness);
                float3 normal = normalize(v.normal);
                float3 outlineOffset = normal * _OutlineThickness;
                float3 position = v.vertex + outlineOffset;
                o.position = UnityObjectToClipPos(position);

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