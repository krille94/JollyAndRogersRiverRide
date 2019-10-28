Shader "Hidden/FadeToBlack"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_bwBlend ("FadeValue", Range(0, 1)) = 1
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

            uniform sampler2D _MainTex;
			uniform float _bwBlend;

			float4 frag(v2f i) : SV_Target
			{
				float4 c = tex2D(_MainTex, i.uv);

				return float4(c.r * _bwBlend, c.g * _bwBlend, c.b * _bwBlend, 1);
			}

			ENDCG
        }
    }
}
