#include "GlobalDeferred.fxh"

/////DIRECTIONAL LIGHT VARS

float3 LightDir;
float4 LightColor;
float LightIntensity;

sampler ColorSampler  : register(s0);
sampler NormalSampler : register(s1);
sampler DepthSampler  : register(s2);

/////DIRECTIONAL LIGHT STRUCTS

struct DirectionalLightVInput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

struct DirectionalLightVOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

/////DIRECTIONAL LIGHT FUNCTIONS

DirectionalLightVOutput DirectionalLightV(DirectionalLightVInput input)
{
    DirectionalLightVOutput output;

    output.Position = float4(input.Position.xyz, 1);
	output.TexCoord = input.TexCoord;

    return output;
}

float4 DirectionalLightP(DirectionalLightVOutput input) : COLOR0
{
	//This isn't exactly right, but use it for now
	half3 Normal = tex2D(NormalSampler, input.TexCoord);
	return float4(1,0,0,1);
}

/////TECHNIQUES

technique DirectionalLight
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 DirectionalLightV();
        PixelShader = compile ps_2_0 DirectionalLightP();
    }
}