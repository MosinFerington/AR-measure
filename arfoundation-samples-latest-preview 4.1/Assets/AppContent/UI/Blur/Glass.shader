// https://light11.hatenadiary.com/entry/2019/09/24/213524?fbclid=IwAR0R-aNCwVJobCmKRCPx-yjWN1r8fB05oxCqqCo9QHtB1krOWFMzUfyxno4
Shader "Example"
{
    Properties
    {
        [PowerSlider(0.1)] _F0("F0", Range(0.0, 1.0)) = 0.02
        _CubeMap("Cube Map", Cube) = "white" {}
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

        // 内面を描画するパス
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            // 前面カリング
            Cull Front Lighting Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                half3 normal    : NORMAL;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half fresnel      : TEXCOORD1;
                half3 reflDir : TEXCOORD2;
            };

            UNITY_DECLARE_TEXCUBE(_CubeMap);
            float _F0;

            v2f vert(appdata v)
            {
                v2f o;
                // 厚み分だけ小さくしておく
                o.vertex = UnityObjectToClipPos(v.vertex * 0.97);
                half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));

                o.fresnel = _F0 + (1.0h - _F0) * pow(1.0h - dot(-viewDir, v.normal.xyz), 5);
                o.reflDir = mul(unity_ObjectToWorld, reflect(-viewDir, v.normal.xyz));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = UNITY_SAMPLE_TEXCUBE(_CubeMap, i.reflDir);
                // 内側は適当に0.5を掛けて影響を小さくしておく
                color.a = min(1, i.fresnel * 0.5);
                return color;
            }
            ENDCG
        }

        // 外面を描画するパス
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            // 背面カリング
            Cull Back Lighting Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex   : POSITION;
                half3 normal    : NORMAL;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                half fresnel      : TEXCOORD1;
                half3 reflDir : TEXCOORD2;
            };

            UNITY_DECLARE_TEXCUBE(_CubeMap);
            float _F0;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
                // グレージング角に近づくにつれて早めに1に持っていく
                half thicknessFactor = 0.3;
                o.fresnel = _F0 + (1.0h - _F0) * pow(1.0h - dot(viewDir, v.normal.xyz) + thicknessFactor, 5);
                o.fresnel = min(1, o.fresnel);
                o.reflDir = mul(unity_ObjectToWorld, reflect(-viewDir, v.normal.xyz));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = UNITY_SAMPLE_TEXCUBE(_CubeMap, i.reflDir);
                color.a = i.fresnel;
                return color;
            }
            ENDCG
        }
    }
}