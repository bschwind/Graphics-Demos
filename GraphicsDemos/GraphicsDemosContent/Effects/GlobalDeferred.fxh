////GLOBAL VARS
float HalfPixelWidth;
float HalfPixelHeight;
float4x4 InverseViewProj;
float4x4 InverseView;
float3 CamPos;

//Manual Linear Sampling
float4 manualSample(sampler Sampler, float2 UV, float2 textureSize)
{
	float2 texelpos = textureSize * UV; 
	float2 lerpVals = frac(texelpos); 
	float texelSize = 1.0 / textureSize;                 
	float4 sourcevals[4]; 
	sourcevals[0] = tex2D(Sampler, UV); 
	sourcevals[1] = tex2D(Sampler, UV + float2(texelSize, 0)); 
	sourcevals[2] = tex2D(Sampler, UV + float2(0, texelSize)); 
	sourcevals[3] = tex2D(Sampler, UV + float2(texelSize, texelSize));   
         
	float4 interpolated = lerp(lerp(sourcevals[0], sourcevals[1], lerpVals.x), 
						  lerp(sourcevals[2], sourcevals[3], lerpVals.x ), lerpVals.y); 
	return interpolated;
}

//Phong Shader
float4 Phong(float3 L, float3 LightColor, float LightIntensity, float3 Position, float3 N, float SpecularIntensity, float SpecularPower)
{
	//Calculate Reflection vector
	float3 R = normalize(reflect(L, N));
	//Calculate Eye vector
	float3 E = normalize(CamPos - Position.xyz);
	//Calculate N.L
	float NL = dot(N, -L);
	//Calculate Diffuse
	float3 Diffuse = NL * LightColor;
	//Calculate Specular
	float Specular = SpecularIntensity * pow(saturate(dot(R, E)), SpecularPower);
	//Calculate Final Product
	return LightIntensity * float4(Diffuse.rgb, Specular);
}