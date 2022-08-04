using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Diagnostics;
public static class NoiseGenerator
{

    public static float[,] GenerateNoiseMap(int mapSize, int seed, float scale, int octaves, float persistence, float lacunarity, FastNoiseLite.NoiseType noiseType, 
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
        fastNoise.SetFrequency(1);

        Parallel.For(0, mapSize,
            y =>
            {
                Parallel.For(0, mapSize,
                    x =>
                    {
                        float noiseValue;
                        float xMod = (x - mapSize) / scale;
                        float yMod = (y - mapSize) / scale;

                        // 1000 * 1000 map seems to cost about 10 - 20ms
                        if(isWarping)
                        {
                            fastNoise.DomainWarp(ref xMod, ref yMod);
                        }

                        fastNoise.SetNoiseType(noiseType);
                        noiseValue = fastNoise.GetNoise(xMod, yMod) * 2 - 1;
                      
                        noiseValue = (noiseValue + 1) / 2;

                        if (noiseValue > maxNoiseHeight)
                        {
                            maxNoiseHeight = noiseValue;
                        }
                        else if (noiseValue < minNoiseHeight)
                        {
                            minNoiseHeight = noiseValue;
                        }

                        noiseMap[x, y] = noiseValue;
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
