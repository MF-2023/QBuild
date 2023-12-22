Shader "Unity/DitherTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BayerTex ("BayerTex", 2D) = "black" {}
        _BlockSize ("BlockSize", int) = 4
        _Radius ("Radius", Range(0.001, 100)) = 10
    }
    SubShader
    {
        Tags
        {
            // �����ł���K�v�͂Ȃ�
            "RenderType"="Opaque"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 wpos : TEXCOORD1;
                float4 spos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _BayerTex;
            float4 _MainTex_ST;

            float _BlockSize;
            float _Radius;

            v2f vert(appdata v) {
                v2f o;
                // MVP�s��������� 
                o.vertex = UnityObjectToClipPos(v.vertex);
                // UV���W
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // ���[���h���W
                o.wpos = mul(unity_ObjectToWorld, v.vertex);
                // �X�N���[�����W
                o.spos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);

                // �J��������̋���
                float dist = distance(i.wpos, _WorldSpaceCameraPos);
                // �̈����0~1�̋�����Clamp
                float clamp_distance = saturate(dist / _Radius);
                // BlockSize�s�N�Z������BayerMatrix�̊��蓖��
                float2 uv_BayerTex = (i.spos.xy / i.spos.w) * (_ScreenParams.xy / _BlockSize);
                // BayerMatrix����臒l���Ƃ��Ă���
                float threshold = tex2D(_BayerTex, uv_BayerTex).r;
                // 臒l�����͕`�悵�Ȃ�
                clip(clamp_distance - threshold);

                return col;
            }
            ENDCG
        }
    }
}