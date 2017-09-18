#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

//------------------------------ TEXTURE PROPERTIES ----------------------------
// This is the texture that SpriteBatch will try to set before drawing
texture2D ScreenTexture;
texture2D PointLight;
 
// Our sampler for the texture, which is just going to be pretty simple
sampler TextureSamplerA = sampler_state
{
    Texture = <ScreenTexture>;
};

sampler TextureSamplerB = sampler_state
{
    Texture = <PointLight>;
};
 
//------------------------ PIXEL SHADER ----------------------------------------
// This pixel shader will simply look up the color of the texture at the
// requested point
float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
    float4 color = ScreenTexture.Sample(TextureSamplerA, texCoord.xy);
    float4 colorPL = PointLight.Sample(TextureSamplerB, texCoord.xy);
    color *= colorPL;
    return color;
}
 
//-------------------------- TECHNIQUES ----------------------------------------
// This technique is pretty simple - only one pass, and only a pixel shader
technique PointLightLayer
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}