using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Mathematics;
public static class NoiseGenerator
{

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset, MapGenerator.NoiseType noiseType, 
        FastNoiseLite.FractalType fractalType, FastNoiseLite.CellularDistanceFunction cellularDistanceFunction, FastNoiseLite.CellularReturnType cellularReturnType,
        FastNoiseLite.DomainWarpType domainWarpType, bool isWarping, float domainWarpAmplitude)
    {
        FastNoiseLite fastNoise = new FastNoiseLite();
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random random = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetY = random.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;



        fastNoise.SetFractalType(fractalType);
        fastNoise.SetCellularDistanceFunction(cellularDistanceFunction);
        fastNoise.SetCellularReturnType(cellularReturnType);
        fastNoise.SetFractalOctaves(octaves);
        fastNoise.SetFractalLacunarity(lacunarity);
        fastNoise.SetFractalGain(persistence);
        fastNoise.SetDomainWarpAmp(domainWarpAmplitude);
        fastNoise.SetSeed(seed);
        fastNoise.SetDomainWarpType(domainWarpType);

        Parallel.For(0, mapHeight,
            y =>
            {
                Parallel.For(0, mapWidth,
                    x =>
                    {
                        float amplitude = 1;
                        float frequency = 1;
                        float noiseHeight = 0;

                        fastNoise.SetFrequency(frequency);


                        float noiseValue;
                        float xMod = (x - halfWidth) / scale;
                        float yMod = (y - halfWidth) / scale;
                        if(isWarping)
                        {
                            fastNoise.DomainWarp(ref xMod, ref yMod);
                        }
                        switch (noiseType)
                        {
                            case MapGenerator.NoiseType.PerlinFast:
                                fastNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
                                noiseValue = fastNoise.GetNoise(xMod, yMod) * 2 - 1;
                                break;
                            case MapGenerator.NoiseType.Simplex:
                                fastNoise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
                                noiseValue = fastNoise.GetNoise(xMod, yMod) * 2 - 1;
                                break;
                            case MapGenerator.NoiseType.Cellular:
                                fastNoise.SetNoiseType(FastNoiseLite.NoiseType.Cellular);
                                noiseValue = fastNoise.GetNoise(xMod, yMod) * 2 - 1;
                                break;
                            case MapGenerator.NoiseType.Value:
                                fastNoise.SetNoiseType(FastNoiseLite.NoiseType.Value);
                                noiseValue = fastNoise.GetNoise(xMod, yMod) * 2 - 1;
                                break;
                            default:
                                fastNoise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
                                noiseValue = fastNoise.GetNoise(xMod, yMod) * 2 - 1;
                                break;
                        }
                        noiseValue = (noiseValue + 1) / 2;
                        noiseHeight += noiseValue * amplitude;
                        amplitude *= persistence;
                        frequency *= lacunarity;


                        if (noiseHeight > maxNoiseHeight)
                        {
                            maxNoiseHeight = noiseHeight;
                        }
                        else if (noiseHeight < minNoiseHeight)
                        {
                            minNoiseHeight = noiseHeight;
                        }

                        noiseMap[x, y] = noiseHeight;
                    });
            });

        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
                return noiseMap;
    }
}
