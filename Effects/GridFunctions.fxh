float4x4 World;
float4x4 View;
float4x4 Projection;
float2 PixelOffset;
float2 CameraPosition;
float Depth;

#if ADD_NOISE
float NoiseAmount;
#endif

texture _MainTex;
sampler2D _MainTexSampler = sampler_state
{
	Texture = (_MainTex);
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

Texture NoiseTexture;
sampler NoiseSampler = sampler_state {
	texture = <NoiseTexture>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
	AddressU = WRAP;
	AddressV = WRAP;
};

struct VertexShaderInput
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
	output.LocalPosition = input.Position;
	output.UV = input.UV;
	output.Normal = input.Normal;
	return output;
}


float4 GetColor(VertexShaderOutput input)
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

	bool color1IsZero = color1.r == 0 && color1.g == 0 && color1.b == 0 && color1.a == 0;
	bool color2IsZero = color2.r == 0 && color2.g == 0 && color2.b == 0 && color2.a == 0;

	// If both pixels are transparent, clip it.
	clip(-(color1IsZero && color2IsZero));

#if REMOVE_INNER
	clip(-(!color1IsZero && !color2IsZero &&
		   uv.x > 0 && uv2.x < 1 && uv2.y > 0 && uv.y < 1));
#endif

	float4 result = color2IsZero ? color1 : color2;

#if ADD_NOISE
	// Noise is 64x64, one noise color per 2 grid cells
	const float sampleOffset = 1.0 / 64 / 2;

	float x = dot(input.Normal, float3(0, 1, 0) > 0.5) ?
		(input.LocalPosition.x - CameraPosition.x) * sampleOffset :
		(input.LocalPosition.y - CameraPosition.y) * sampleOffset;
	float y = input.LocalPosition.z * sampleOffset * Depth;
	float2 sampleCoords = float2(x, y);

	float noise = tex2D(NoiseSampler, sampleCoords).r;
	result.rgb *= lerp(1, 0.5f + noise.r, NoiseAmount);
#endif

	return result;
}