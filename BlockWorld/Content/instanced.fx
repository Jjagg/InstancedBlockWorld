#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;
matrix WorldInverseTranspose;
float3 EyeVector;

float4 Ambient;

float3 LightDir0;
float3 LightDir1;
float3 LightDir2;

float3 Diffuse0;
float3 Diffuse1;
float3 Diffuse2;

int NumLights;

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
    float3 Offset : POSITION1;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(input.Position + float4(input.Offset, 0), WorldViewProjection);
	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float3x3 lightDirections = 0;   
    float3x3 lightDiffuse = 0;

    [unroll(3)]
    for (int i = 0; i < NumLights; i++)
    {
        lightDirections[i] = float3x3(LightDir0, LightDir1, LightDir2)[i];
        lightDiffuse[i] = float3x3(Diffuse0, Diffuse1, Diffuse2)[i];
    }

    float3 dotL = mul(-lightDirections, input.Normal);
    float3 zeroL = step(float3(0,0,0), dotL);
    float3 diffuse = zeroL * dotL;

	return float4(mul(diffuse, lightDiffuse) + Ambient, 1);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
