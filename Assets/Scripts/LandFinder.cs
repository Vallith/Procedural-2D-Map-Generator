using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;

public class LandFinder : MonoBehaviour
{
    public MapGenerator mapGen;
    ConcurrentBag<Vector2Int> globalSet = new ConcurrentBag<Vector2Int>();

    public void GetLand()
    {
        globalSet = new ConcurrentBag<Vector2Int>();
        BruteForce();
    }

    public bool Inside(Vector2Int point)
    {
        return Inside(point.x, point.y);
    }

    public bool Inside(int x, int y)
    {
        if (x < mapGen.mapSize && x >= 0 && y < mapGen.mapSize && y >= 0)
        {
            return mapGen.noiseMap[x, y] > mapGen.threshold;
        }
        return false;
    }

    public void BruteForce()
    {
        Parallel.For(0, mapGen.mapSize, x =>
        {
            Parallel.For(0, mapGen.mapSize, y =>
            {
                if (Inside(x, y))
                {
                    globalSet.Add(new Vector2Int(x, y));
                }
            });
        });
    }

    public void CreateOutline(float[,] noiseMap, Color[] colourMap = null)
    {
        Parallel.ForEach(globalSet, item =>
        {
            CheckEdge(item, noiseMap, colourMap);
        });
    }

    void CheckEdge(Vector2Int item, float[,] noiseMap, Color[] colourMap)
    {
        if (noiseMap[item.x, item.y] >= mapGen.threshold)
        {
            if (item.x + 1 < mapGen.mapSize && noiseMap[item.x + 1, item.y] <= mapGen.threshold)
            {
                noiseMap[item.x + 1, item.y] = 1f;
                if (colourMap != null)
                {
                    colourMap[item.y * mapGen.mapSize + (item.x + 1)] = mapGen.outlineColour;
                }
            }

            if (item.x - 1 >= 0 && noiseMap[item.x - 1, item.y] <= mapGen.threshold)
            {
                noiseMap[item.x - 1, item.y] = 1f;
                if (colourMap != null)
                {
                    colourMap[item.y * mapGen.mapSize + (item.x - 1)] = mapGen.outlineColour;
                }
            }

            if (item.y + 1 < mapGen.mapSize && noiseMap[item.x, item.y + 1] <= mapGen.threshold)
            {
                noiseMap[item.x, item.y + 1] = 1f;
                if (colourMap != null)
                {
                    colourMap[(item.y + 1) * mapGen.mapSize + item.x] = mapGen.outlineColour;
                }
            }

            if (item.y - 1 >= 0 && noiseMap[item.x, item.y - 1] <= mapGen.threshold)
            {
                noiseMap[item.x, item.y - 1] = 1f;
                if (colourMap != null)
                {
                    colourMap[(item.y - 1) * mapGen.mapSize + item.x] = mapGen.outlineColour;
                }
            }
        }
    }
}
