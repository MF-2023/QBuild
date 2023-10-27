Shader "Neko/Dissolve"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        [NoScaleOffset]_GradationMap( "Gradation Map", 2D) = "white" {}
        [Normal] _NormalMap("Normal Map", 2D) = "bump" {}
        _Alpha( "Alpha", Range(0.0, 1.0)) = 1.0
        _NearRange( "Near Range", Range(0.0, 100.0)) = 0.0
        _FarRange( "Far Range", Range(0.0, 100.0)) = 1.0
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
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.texcoord = float2(v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw);

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
            float _FarRange;

            half4 frag(v2f i) : SV_Target {
                float3 dist = _WorldSpaceCameraPos - i.worldPos;

                //カメラの前方を取得
                float3 cameraForward = UNITY_MATRIX_V._m20_m21_m22;
                //Y軸以外を0にする
                cameraForward.y = 0;

                //前方ベクトルとカメラからオブジェクトまでのベクトルの内積を取得
                float cameraToObjLength = dot(dist, cameraForward);

                const float value = smoothstep(_NearRange, _NearRange + _FarRange, cameraToObjLength);
                if (value == 1) {
                    return tex2D(_MainTex, i.texcoord);
                }
                float2 localuv = fmod(i.vpos.xy, 3) / _GradationMap_TexelSize.zw;
                float alpha = floor(value * 64.0) / 64.0;
                localuv.x += alpha;

                float3 gradation = tex2D(_GradationMap, localuv).rgb;

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