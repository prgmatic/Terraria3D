float4x4 World;
float4x4 View;
float4x4 Projection;

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
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;
    return output;
}


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 color = tex2D(_MainTexSampler, input.UV);
	//color.a *= 0.5;
	if (color.a == 0)
		clip(-1);
	return color;
}

technique Technique1
{
    pass Pass1
    {
		AlphaBlendEnable = True;
		SrcBlend = One;
		DestBlend = InvSrcAlpha;

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
