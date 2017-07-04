#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif



float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 Rotation;

float4x4 Bones[50];

float4 LightDir = float4(1, 1, 1, 1);
float4 LightColor = float4(1, 1, 1, 1);
float intesity = 0.9;
float EdgeWidth = 0.1;


texture Texture;
sampler2D textureSampler = sampler_state {
	Texture = (Texture);
	MinFilter = Linear;
	MagFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};



struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 Normal : NORMAL0;

	int4 BoneIndices : BLENDINDICES0;
	float4 BoneWeights : BLENDWEIGHT0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoords : TEXCOORD0;
	float4 Normal : TEXCOORD1;
};



VertexShaderOutput MainVS(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4x4 skinTransform = 0;

	float4x4 world = mul(Rotation, World);

	skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x
		+ Bones[input.BoneIndices.y] * input.BoneWeights.y
		+ Bones[input.BoneIndices.z] * input.BoneWeights.z;
		+ Bones[input.BoneIndices.w] * input.BoneWeights.w;

	float4 worldPosition = mul(input.Position, mul(skinTransform, world));
	//float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.TexCoords = input.TexCoords;
	output.Normal = normalize(mul(input.Normal, Rotation));

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	//Read Texture
	float4 color = tex2D(textureSampler, input.TexCoords);

	//Light
	float4 normal = input.Normal;
	float4 diffuse = saturate(dot(-LightDir, normal));

	float4 colorWithLight = float4((color*0.4 + 0.6*(color * LightColor * intesity * diffuse)).xyz, 1);


	return colorWithLight;
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};