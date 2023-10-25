Shader "Neko/Dissolve"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        [NoScaleOffset]_GradationMap( "Gradation Map", 2D) = "white" {}
        _Alpha( "Alpha", Range(0.0, 1.0)) = 1.0
        _NearRange( "Near Range", Range(0.0, 1.0)) = 0.0
    }//Properties
    CGINCLUDE
    #include "UnityCG.cginc"
    ENDCG

    SubShader
    {
        Cull Off
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            struct appdata {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 texcoord : TEXCOORD0;
                UNITY_VPOS_TYPE vpos : VPOS;
                float3 worldPos : WORLD_POS;
                float2 aspect_scale : TEXCOORD2;
            };

            v2f vert(appdata v, out float4 vertex : SV_POSITION) {
                v2f o = (v2f)0;
                vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = float2(v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex); //ローカル座標系をワールド座標系に変換

                // アスペクト算出
                float4 projectionSpaceUpperRight = float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y);
                float4 viewSpaceUpperRight = mul(unity_CameraInvProjection, projectionSpaceUpperRight);
                float aspect = viewSpaceUpperRight.x / viewSpaceUpperRight.y;
                o.aspect_scale = float2(aspect, 1);
                return o;
            }

            sampler2D _GradationMap;
            float4 _GradationMap_ST;
            float4 _GradationMap_TexelSize;
            float _Alpha;
            float _NearRange;

            half4 frag(v2f i) : SV_Target {
                float cameraToObjLength = length(_WorldSpaceCameraPos - i.worldPos);

                float value = min(cameraToObjLength * _Alpha, 1.0);
                value *= value * value * value * value * value * value * value * value * value * value * value * value * value;
                if (value == 1) {
                    // 不透明なら問答無用でレンダリング
                    return tex2D(_MainTex, i.texcoord);
                }
                float2 localuv = fmod(i.vpos.xy, 3) / _GradationMap_TexelSize.zw;
                float alpha = floor(value * 32.0) / 32.0;
                localuv.x += alpha;

                float3 gradation = tex2D(_GradationMap, localuv).rgb;
                if (value < _NearRange) discard;
                if (value < _NearRange + 0.008) return float4(0.1, 0.1, 0.1, 1);

                if (gradation.r < 0.5) {
                    discard; // 中止
                }

                return tex2D(_MainTex, i.texcoord);
            }
            ENDCG
        }//Pass
    }//SubShader
    FallBack "Diffuse"
}//Shader