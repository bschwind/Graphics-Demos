#include "GlobalDeferred.fxh"

////RENDER QUAD VARS
texture QuadTex;

sampler TexSampler : register (s0) = 
sampler_state 
{ 
	Texture = <QuadTex>;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

/////RENDER QUAD STRUCTS

struct RenderQuadVInput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct RenderQuadVOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

//RENDER QUAD FUNCTIONS

RenderQuadVOutput RenderQuadV(RenderQuadVInput input)
{
    RenderQuadVOutput output;
    output.Position = input.Position;
	output.TexCoord = float2(input.TexCoord.x-HalfPixelWidth, input.TexCoord.y-HalfPixelHeight);
    return output;
}

float4 RenderQuadP(RenderQuadVOutput input) : COLOR0
{
	float4 color = tex2D(TexSampler, input.TexCoord);
    return float4(color.rgb, 1);
}




/////CLEAR GBUFFER STRUCTS

struct ClearBufferVInput
{
    float4 Position : POSITION0;
};

struct ClearBufferVOutput
{
    float4 Position : POSITION0;
};

struct ClearBufferPOutput
{
	float4 Color    : COLOR0;
	float4 Normal   : COLOR1;
	float4 Depth    : COLOR2;
};

//CLEAR GBUFFER FUNCTIONS

ClearBufferVOutput ClearBufferV(ClearBufferVInput input)
{
	ClearBufferVOutput output;
    output.Position = input.Position;
	return output;
}

ClearBufferPOutput ClearBufferP(ClearBufferVOutput input)
{
	ClearBufferPOutput output;
	
	output.Color = 0.0f;
	output.Color.a = 0.0f;
	output.Normal.rgb = 0.0f;
	output.Normal.a = 0.0f;
	output.Depth = 1.0f;

	return output;
}




/////RENDER G BUFFER VARS
float4x4 World;
float4x4 View;
float4x4 Projection;

texture ColorTex;

sampler ColorSampler : register (s0) = 
sampler_state 
{ 
	Texture = <ColorTex>;
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

/////RENDER GBUFFER STRUCTS

struct GenerateBufferVInput
{
    float4 Position : POSITION0;
	float4 Normal   : NORMAL;
	float2 TexCoord : TEXCOORD0;
};

struct GenerateBufferVOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
	float2 Depth    : TEXCOORD2;
};

struct GenerateBufferPOutput
{
	float4 Color    : COLOR0;
	float4 Normal   : COLOR1;
	float4 Depth    : COLOR2;
};

//RENDER GBUFFER FUNCTIONS

GenerateBufferVOutput GenerateBufferV(GenerateBufferVInput input)
{
    GenerateBufferVOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.TexCoord = input.TexCoord;
	output.Normal = input.Normal;
	output.Depth.x = output.Position.z;
	output.Depth.y = output.Position.w;

    return output;
}

GenerateBufferPOutput GenerateBufferP(GenerateBufferVOutput input)
{
	GenerateBufferPOutput output;
	float4 color = tex2D(ColorSampler, input.TexCoord);

    output.Color = float4(color.rgb, 1);
	output.Normal = float4(input.Normal,1);
	float depth = input.Depth.x / input.Depth.y;
	output.Depth = float4(depth,depth,depth,1);
	
	return output;
}

/////TECHNIQUES

technique GenerateBuffer
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 GenerateBufferV();
        PixelShader = compile ps_2_0 GenerateBufferP();
    }
}

technique RenderQuad
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 RenderQuadV();
		PixelShader = compile ps_2_0 RenderQuadP();
	}
}

technique ClearBuffer
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 ClearBufferV();
		PixelShader = compile ps_2_0 ClearBufferP();
	}
}
