#define REMOVE_INNER 1

float4x4 World;
float4x4 View;
float4x4 Projection;
float2 PixelOffset;

texture _MainTex;
sampler2D _MainTexSampler = sampler_state 
{
	Texture = (_MainTex);
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};


struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;
	output.Normal = input.Normal;

    return output;
}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
#if REMOVE_INNER
	float2 uv = input.UV - input.Normal.xy * PixelOffset.xy * 0.5;
	float2 uv2 = uv + input.Normal.xy * PixelOffset;
	float4 color1 = tex2D(_MainTexSampler, uv);
	float4 color2 = tex2D(_MainTexSampler, uv2);
#else
	float2 uv = input.UV - input.Normal.xy * PixelOffset.xy * 0.5;
	float4 color1 = tex2D(_MainTexSampler, uv);
	float4 color2 = tex2D(_MainTexSampler, uv + input.Normal.xy * PixelOffset);
#endif

	// If both pixels are transparent, clip it.
	float cutoff = 0.1;
	clip(max(color1.a - cutoff, color2.a - cutoff));

#if REMOVE_INNER
	clip(-(color1.a > cutoff && color2.a >= cutoff &&
		   uv.x > 0 && uv2.x < 1 && uv2.y > 0 && uv.y < 1));
#endif

	return color1.a > color2.b ? color1 : color2;
}

technique Technique1
{
    pass Pass1
    {
		AlphaBlendEnable = True;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
		CullMode = None;

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}
