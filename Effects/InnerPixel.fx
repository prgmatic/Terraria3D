float2 PixelOffset;

sampler s0;

float4 InnerPixelPS(float2 uv: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, uv);

	if (color.a > 0)
	{
		// Go in 2 pixels
		float2 ScaledPixelOffset = PixelOffset * 2;

		// Sample neighboring pixels
		float4 sampleLeft  = tex2D(s0, uv + float2(-ScaledPixelOffset.x, 0.0));
		float4 sampleRight = tex2D(s0, uv + float2( ScaledPixelOffset.x, 0.0));
		float4 sampleAbove = tex2D(s0, uv + float2(0.0, -ScaledPixelOffset.y));
		float4 sampleBelow = tex2D(s0, uv + float2(0.0,  ScaledPixelOffset.y));

		// Store neighboring pixel states
		bool leftPixel  =  sampleLeft.a > 0.0f;
		bool rightPixel = sampleRight.a > 0.0f;
		bool abovePixel = sampleAbove.a > 0.0f;
		bool belowPixel = sampleBelow.a > 0.0f;

		float2 offset = float2(0, 0);

		if (rightPixel && !leftPixel)
			offset.x += ScaledPixelOffset.x;
		else if (leftPixel && !rightPixel)
			offset.x -= ScaledPixelOffset.x;
		if (abovePixel && !belowPixel)
			offset.y -= ScaledPixelOffset.y;
		else if (belowPixel && !abovePixel)
			offset.y += ScaledPixelOffset.y;
		float4 newColor = tex2D(s0, uv + offset);

		if (newColor.a > 0)
		{
			// If inner pixel is black, move in one more pixel
			if (newColor.r == 0 && newColor.g == 0 && newColor.b == 0)
			{
				newColor = tex2D(s0, uv + offset * 2);
				if (newColor.a > 0)
					color = newColor;
			}
			else
				color = newColor;
		}
	}
	return color;
}

technique InnerPixel
{
	pass Pass0
	{
		PixelShader = compile ps_2_0 InnerPixelPS();
	}
}