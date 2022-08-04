using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    MapGenerator mapGenerator;
    public TextMeshProUGUI tooltipBox;

    private void Awake()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    public void UpdateOption<T>(MapOptionType optionType, T value)
    {
        switch (optionType)
        {
            case MapOptionType.Octaves:
                mapGenerator.octaves = (int)Convert.ChangeType(value, typeof(int));
                break;
            case MapOptionType.Persistence:
                mapGenerator.persistence = (float)Convert.ChangeType(value, typeof(float));
                break;
            case MapOptionType.Lacunarity:
                mapGenerator.lacunarity = (float)Convert.ChangeType(value, typeof(float));
                break;
            case MapOptionType.Seed:
                mapGenerator.seed = (int)Convert.ChangeType(value, typeof(int));
                break;
            case MapOptionType.Scale:
                mapGenerator.noiseScale = (float)Convert.ChangeType(value, typeof(float));
                break;
            case MapOptionType.MapSize:
                mapGenerator.mapSize = (int)Convert.ChangeType(value, typeof(int));
                break;
            case MapOptionType.NoiseType:
                mapGenerator.noiseType = (FastNoiseLite.NoiseType)Enum.Parse(typeof(FastNoiseLite.NoiseType), (string)Convert.ChangeType(value, typeof(string)));
                break;
            case MapOptionType.FractalType:
                mapGenerator.fractalType = (FastNoiseLite.FractalType)Enum.Parse(typeof(FastNoiseLite.FractalType), (string)Convert.ChangeType(value, typeof(string)));
                break;
            case MapOptionType.CellularReturnType:
                mapGenerator.cellularReturnType = (FastNoiseLite.CellularReturnType)Enum.Parse(typeof(FastNoiseLite.CellularReturnType), (string)Convert.ChangeType(value, typeof(string)));
                break;
            case MapOptionType.CellularDistanceType:
                mapGenerator.cellularDistanceFunction = (FastNoiseLite.CellularDistanceFunction)Enum.Parse(typeof(FastNoiseLite.CellularDistanceFunction), (string)Convert.ChangeType(value, typeof(string)));
                break;
            default:
                break;
        }

        mapGenerator.UpdateMap();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
