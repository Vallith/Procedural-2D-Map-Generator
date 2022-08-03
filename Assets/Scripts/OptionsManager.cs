using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    MapGenerator mapGenerator;

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
