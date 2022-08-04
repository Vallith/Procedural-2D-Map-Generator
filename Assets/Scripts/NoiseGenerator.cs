using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;
public static class NoiseGenerator
{

    public static float[,] GenerateNoiseMap(int mapSize, int seed, float scale, int octaves, float persistence, float lacunarity, MapGenerator.NoiseType noiseType, 
        FastNoiseLite.FractalType fractalType, FastNoiseLite.CellularDistanceFunction cellularDistanceFunction, FastNoiseLite.CellularReturnType cellularReturnType,
        FastNoiseLite.DomainWarpType domainWarpType, bool isWarping, float domainWarpAmplitude)
    {
        FastNoiseLite fastNoise = new FastNoiseLite();
        float[,] noiseMap = new float[mapSize, mapSize];

        if (scale <= 0) {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        fastNoise.SetFractalType(fractalType);
        fastNoise.SetCellularDistanceFunction(cellularDistanceFunction);
        fastNoise.SetCellularReturnType(cellularReturnType);
        fastNoise.SetFractalOctaves(octaves);
        fastNoise.SetFractalLacunarity(lacunarity);
        fastNoise.SetFractalGain(persistence);
        fastNoise.SetDomainWarpAmp(domainWarpAmplitude);
        fastNoise.SetSeed(seed);
        fastNoise.SetDomainWarpType(domainWarpType);

        Parallel.For(0, mapSize,
            y =>
            {
                Parallel.For(0, mapSize,
                    x =>
                    {
                        float amplitude = 1;
                        float frequency = 1;
                        float noiseHeight = 0;

                        fastNoise.SetFrequency(frequency);


                        float noiseValue;
                        float xMod = (x - mapSize) / scale;
                        float yMod = (y - mapSize) / scale;

                        // 1000 * 1000 map seems to cost about 10 - 20ms
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

        // 1000 * 1000 map seems to cost about 15 ms
        Parallel.For(0, mapSize, y =>
        {
            Parallel.For(0, mapSize, x =>
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            });
        });

        return noiseMap;
    }
}
