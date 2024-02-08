Shader "Custom/RingShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _BorderColor ("Border Color", Color) = (1,1,1,1)
        _BackgroundColor ("Background Color", Color) = (0,0,0,0)
        _BorderWidth ("Border Width", Range(0.0, 0.5)) = 0.1
        _Feather ("Feather", Range(0.0, 0.1)) = 0.01
    }
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:blend

        sampler2D _MainTex;
        fixed4 _BorderColor;
        fixed4 _BackgroundColor;
        float _BorderWidth;
        float _Feather;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // テクスチャから色を取得
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // 円の中心からの距離を計算（UVは0から1なので、0.5を中心とする）
            float2 uvCentered = IN.uv_MainTex - float2(0.5, 0.5);
            uvCentered *= 2.0; // 円の直径を1にするためにスケールを調整
            float dist = length(uvCentered);

            // 内側と外側の円の境界を定義
            float innerEdge = 0.5 - _BorderWidth;
            float outerEdge = 0.5;
            float alpha = smoothstep(innerEdge, innerEdge + _Feather, dist) * (1 - smoothstep(outerEdge - _Feather, outerEdge, dist));

            // マスクに基づいてアルファ値を設定
            o.Alpha = alpha;
            o.Albedo = _BorderColor.rgb;
            o.Emission = _BackgroundColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
