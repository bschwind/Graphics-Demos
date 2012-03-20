#include "GlobalDeferred.fxh"

/////GLOBAL LIGHT VARS

sampler ColorSampler  : register(s0);
sampler NormalSampler : register(s1);
sampler DepthSampler  : register(s2);
float2 GBufferSize;

/////DIRECTIONAL LIGHT VARS

float3 LightDir;
float4 LightColor;
float LightIntensity;

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
	half3 normal = tex2D(NormalSampler, input.TexCoord);

	float specularIntensity = 1.0f;
	float specularPower = 1.0f;
	float depth = manualSample(DepthSampler, input.TexCoord, GBufferSize).x;

	float4 position = 1.0f;
	position.x = input.TexCoord.x * 2.0f - 1.0f;
	position.y = -(input.TexCoord.x * 2.0f - 1.0f);
	position.z = depth;
	position = mul(position, InverseViewProj);
	position /= position.w;

	return Phong(LightDir, LightColor, LightIntensity, position.xyz, normal, specularIntensity, specularPower);
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