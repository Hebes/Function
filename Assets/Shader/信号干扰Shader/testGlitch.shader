Shader "Test/Glitch"
{
    Properties
    {
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }
    CGINCLUDE

    #include "UnityCG.cginc"

	sampler2D _MainTex;
	float4 _MainTex_ST;
    float2 _MainTex_TexelSize;

    float2 _ScanLineJitter; // (displacement, threshold)
    float2 _VerticalJump;   // (amount, time)
    float _HorizontalShake;
    float2 _ColorDrift;     // (amount, time)

	//计算噪音公式
    float nrand(float x, float y)
    {
        return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
    }

    half4 frag(v2f_img i) : SV_Target
    {
        float u = i.uv.x;
        float v = i.uv.y;

        //扫描线
        float jitter = nrand(v, _Time.x) * 2 - 1;
        jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;

        //画面垂直跳
        float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);

        //画面左右震动
        float shake = (nrand(_Time.x, 2) - 0.5) * _HorizontalShake;

        //颜色偏移
        float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

        half4 src1 = tex2D(_MainTex, frac(float2(u + jitter + shake, jump)));
        half4 src2 = tex2D(_MainTex, frac(float2(u + jitter + shake + drift, jump)));
        return half4(src1.r, src2.g, src1.b, src1.a);
    }

    ENDCG
    SubShader
    {
        Pass
        {
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #pragma target 3.0
            ENDCG
        }
    }
}
