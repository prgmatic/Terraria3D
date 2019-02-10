#define REMOVE_INNER 1
#define ADD_NOISE 1

float4x4 World;
float4x4 View;
float4x4 Projection;
float2 PixelOffset;
float2 CameraPosition;
float Depth;
bool AO;

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

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 LocalPosition : TEXCOORD1;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD2;
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

float slerp(float x)
{
	return x * x * 3 - x * x * x * 2;
}

float GetAO(float pos)
{
	return lerp(0.5, 1, slerp(pow(frac(pos), 1)));
}

float DoAOTwoPixel(float localPos, float2 direction, float2 start, float2 uv)
{
	float2 offset = float2(start);
	offset += direction;
	float4 offsetPixel = tex2D(_MainTexSampler, uv + offset * PixelOffset);
	if (offsetPixel.a > 0.1)
		return GetAO(frac(localPos) * 0.5);
	else
	{
		offset += direction;
		offsetPixel = tex2D(_MainTexSampler, uv + offset * PixelOffset);
		if (offsetPixel.a > 0.1)
			return GetAO(frac(localPos) * 0.5 + 0.5);
	}
	return 1;
}

float DoAOOnePixel(float localPos, float2 direction, float2 start, float2 uv)
{
	float2 offset = float2(start);
	offset += direction;
	float4 offsetPixel = tex2D(_MainTexSampler, uv + offset * PixelOffset);
	if (offsetPixel.a > 0.1)
		return GetAO(localPos);
	return 1;
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
	const float epsilon = 0.0001;
	bool onEdge = input.UV.x < epsilon || input.UV.x > 1 - epsilon ||
				  input.UV.y < epsilon || input.UV.y > 1 - epsilon;

	bool color1IsZero = color1.r == 0 && color1.g == 0 && color1.b == 0 && color1.a == 0;
	bool color2IsZero = color2.r == 0 && color2.g == 0 && color2.b == 0 && color2.a == 0;

	// If both pixels are transparent, clip it.
	clip(-(color1IsZero && color2IsZero));

#if REMOVE_INNER
	clip(-(!color1IsZero && !color2IsZero && !onEdge));
#endif

	float4 result = color2IsZero ? color1 : color2;

	if (AO && !onEdge)
	{
		float2 start;
		float2 dir;
		if (input.Normal.y < -0.5)
		{
			start = float2(0, color2IsZero ? -1 : 0);
			dir = float2(-1, 0);
			result.rgb *= DoAOTwoPixel(input.LocalPosition.x, dir, start, uv);
			result.rgb *= DoAOTwoPixel(-input.LocalPosition.x, -dir, start, uv);
		}
		else
		{
			start = float2(color2IsZero ? 1 : 0, 0);
			dir = float2(0, -1);
			result.rgb *= DoAOTwoPixel(-input.LocalPosition.y, dir, start, uv);
			result.rgb *= DoAOTwoPixel(input.LocalPosition.y, -dir, start, uv);
		}
	}

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

technique Technique1
{
	pass Pass1
	{
		// Premultiplied Alpha
		AlphaBlendEnable = True;
		SrcBlend = One;
		DestBlend = InvSrcAlpha;
		CullMode = None;

		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}