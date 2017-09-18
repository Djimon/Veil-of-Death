#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection;
float4 AmbientColor = float4(0, 0.5f, 0.5, 1);
float4 Transparency = 0.3f;
float AmbientIntensity = 0.9;   

struct VertexShaderInput
{
    float4 Position : SV_POSITION0;
    //float4 Light : TEXCOORD0;
    float3 Normal : NORMAL0;
    //float3 Tangent : TANGENT;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    //float3 View : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
    /*
    float3x3 world2TangSpace;
    world2TangSpace[0] = mul(input.Tangent, World);
    world2TangSpace[1] = mul(cross(input.Tangent, input.Normal), World);
    world2TangSpace[2] = mul(input.Normal, World);
    output.Light.xyz = mul(world2TangSpace, LightDirection);
    */

    //float3 viewer = EyeVector - worldPosition;
    //output.View = mul(World, viewer);

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 ambient = AmbientColor * AmbientIntensity;
    return ambient ;
}

float4 PixelShaderFunction2(VertexShaderOutput input) : COLOR
{
    float4 ambient = AmbientColor * AmbientIntensity;
    float4 diffuse = { 1.0f, 0.0f, 0.0f, 1.0f };
    ambient.a = Transparency;

    return ambient;
}

technique Transparent
{
	pass p0
	{
		AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction2();
	}
}

technique Ambient
{
    pass P0
    {
        AlphaBlendEnable = TRUE;
        DestBlend = INVSRCALPHA;
        SrcBlend = SRCALPHA;
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
};