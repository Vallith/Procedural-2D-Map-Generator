using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Floodfill : MonoBehaviour
{
    public MapGenerator mapGen;
    HashSet<Vector2Int> globalSet = new HashSet<Vector2Int>();
    Dictionary<string, HashSet<Vector2Int>> landMasses = new Dictionary<string, HashSet<Vector2Int>>();

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int FindAverageOfPoints(HashSet<Vector2Int> set)
    {
        int count = set.Count;
        Vector2Int total = new Vector2Int();
        foreach (var item in set)
        {
            total += item;
        }
        return total /= count;
    }

    public void Flood()
    {
        landMasses.Clear();
        globalSet.Clear();
        HashSet<Vector2Int> samples = new HashSet<Vector2Int>();

        for (int x = 0; x < mapGen.mapSize; x += mapGen.scanStride)
        {
            for (int y = 0; y < mapGen.mapSize; y += mapGen.scanStride)
            {
                samples.Add(new Vector2Int(x, y));
            }
        }

        foreach (var sample in samples)
        {
            CalculateSets(sample);
        }
    }

    public void CalculateSets(Vector2Int sample)
    {
        if (globalSet.Contains(sample) || mapGen.noiseMap[sample.x, sample.y] < mapGen.threshold)
        {
            return;
        }
        HashSet<Vector2Int> set = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(sample);
        while (queue.Count > 0)
        {
            Vector2Int n = queue.Dequeue();
            if (set.Contains(n))
            {
                continue;
            }
            if (n.x < mapGen.mapSize && n.x >= 0 && n.y < mapGen.mapSize && n.y >= 0)
            {
                if (mapGen.noiseMap[n.x, n.y] > mapGen.threshold)
                {
                    set.Add(n);
                    globalSet.Add(n);
                    queue.Enqueue(new Vector2Int(n.x, n.y - 1));
                    queue.Enqueue(new Vector2Int(n.x, n.y + 1));
                    queue.Enqueue(new Vector2Int(n.x - 1, n.y));
                    queue.Enqueue(new Vector2Int(n.x + 1, n.y));
                }
            }
        }
        landMasses.Add(landMasses.Count.ToString(), set);
    }

    public void CreateOutline(float[,] noiseMap, Color[] colourMap = null)
    {
        Parallel.ForEach(globalSet, item =>
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
        });
    }
}
