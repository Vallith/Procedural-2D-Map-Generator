using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{

    public enum FalloffType
    {
        Square,
        HardRadial,
        FeatheredRadial
    };

    public enum DrawMode
    {
        NoiseMap,
        ColourMap,
        FalloffMap
    };

    public enum NoiseType
    {
        PerlinFast,
        Simplex,
        Cellular,
        Value
    };

    FastNoiseLite noise = new FastNoiseLite();
    public DrawMode drawMode;
    public FalloffType falloffType;
    public FastNoiseLite.FractalType fractalType;
    public FastNoiseLite.CellularReturnType cellularReturnType;
    public FastNoiseLite.CellularDistanceFunction cellularDistanceFunction;
    public FastNoiseLite.DomainWarpType domainWarpType;
    public bool isWarping;
    [Range(0f, 10f)]
    public float domainWarpAmplitude;
    public bool autoUpdate;

    [Header("Flood Fill Settings")]
    public bool drawOutlines;
    // How many pixels to skip before checking for flood fill
    [Range(3, 100)]
    public int scanStride;
    public float threshold;
    public Color outlineColour;

    [Header("Map Settings")]
    public NoiseType noiseType;
    [Range(0, 1000)]
    public int mapSize;
    public float noiseScale;
    [Range(1, 10)]
    public int octaves;
    [Range(0f, 1f)]
    public float persistence;
    [Range(0, 23)]
    public float lacunarity;
    public int seed;
    public Vector2 offset;


    float[,] falloffMap;
    [Header("Falloff Settings")]
    public bool useFalloff;
    [Header("Square Falloff Settings")]
    [Range(0f, 10f)]
    public float falloffGradient;
    public float falloffSize;

    [Header("Hard Radial Falloff Settings")]
    public float radialFalloffRadius;
    [Header("Feathered Radial Falloff Settings")]
    public float featheredRadialFalloffRadius1;
    public float featheredRadialFalloffRadius2;

    public TerrainType[] regions;

    public float[,] noiseMap;

    private void Awake()
    {
        GetFalloffMap();
    }

    public void UpdateMap()
    {
        float x = mapSize * 0.5f - 0.5f;
        float y = mapSize * 0.5f - 0.5f;
        Camera.main.orthographicSize = (((mapSize > mapSize * Camera.main.aspect) ? (float)mapSize / (float)Camera.main.pixelWidth * Camera.main.pixelHeight : mapSize) / 2 + 2) * 10f;
        DrawMap();
    }

    public float[,] GetFalloffMap()
    {
        falloffMap = new float[mapSize, mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                falloffMap[x, y] = 1f;
            }
        }


        switch (falloffType)
        {
            case FalloffType.Square:
                falloffMap = FalloffGenerator.GenerateSquareFalloffMap(mapSize, mapSize, falloffGradient, falloffSize);
                break;
            case FalloffType.HardRadial:
                if(radialFalloffRadius > 0f)
                    falloffMap = FalloffGenerator.GenerateRadialFalloffMap(mapSize, mapSize, 10f, 0f, radialFalloffRadius);
                break;
            case FalloffType.FeatheredRadial:
                falloffMap = FalloffGenerator.GenerateFeatheredRadialFalloffMap(mapSize, mapSize, 10f, 0f, featheredRadialFalloffRadius1, featheredRadialFalloffRadius2);
                break;
            default:
                break;
        }
        return falloffMap;
    }

    public void DrawMap()
    {
        MapData mapData = GenerateMapData();

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawOutlines)
        {
            GetComponent<Floodfill>().Flood();
            GetComponent<Floodfill>().CreateOutline(mapData.heightMap, mapData.colourMap);
        }
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapSize, mapSize));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(GetFalloffMap()));
        }
    }
    MapData GenerateMapData() {
        noiseMap = NoiseGenerator.GenerateNoiseMap(mapSize, mapSize, seed, noiseScale, octaves, persistence, lacunarity, offset, noiseType, fractalType, cellularDistanceFunction, cellularReturnType, domainWarpType, isWarping, domainWarpAmplitude);

        return new MapData(noiseMap, GetColourMap());
    }

    Color[] GetColourMap()
    {
        Color[] colourMap = new Color[mapSize * mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }

                float currentHeight = noiseMap[x, y];

                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        return colourMap;
    }

    private void OnValidate()
    {
        if(mapSize < 1) { mapSize = 1; }

        if(mapSize < 1) {  mapSize = 1; }

        if(noiseScale < 1) { noiseScale = 1; }

        if(seed < 0) { seed = 0; }

        if(lacunarity < 1) { lacunarity = 1; }

        if(falloffSize < 0) { falloffSize = 0; }

        if(radialFalloffRadius < 0) { radialFalloffRadius = 0;}

        if(featheredRadialFalloffRadius1 < 0) { featheredRadialFalloffRadius1 = 0; }

        if(featheredRadialFalloffRadius2 < 0) { featheredRadialFalloffRadius2 = 0; }

        if(featheredRadialFalloffRadius2 < featheredRadialFalloffRadius1)
        {
            featheredRadialFalloffRadius2 = featheredRadialFalloffRadius1;
        }

        GetFalloffMap();

    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    [Range(0f, 1f)]
    public float height;
    public Color colour;
}

public struct MapData
{
    public float[,] heightMap;
    public Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}
