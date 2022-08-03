using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodfill : MonoBehaviour
{
    MapGenerator mapGen;
    HashSet<Vector2Int> globalSet = new HashSet<Vector2Int>();
    Dictionary<string, HashSet<Vector2Int>> landMasses = new Dictionary<string, HashSet<Vector2Int>>();
    // Start is called before the first frame update
    void Start()
    {
        mapGen = FindObjectOfType<MapGenerator>();
    }

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
        HashSet<Vector2Int> set = new HashSet<Vector2Int>();
        for (int i = 0; i < mapGen.floodfillPoints; i++)
        {
            for (int y = 0; y < mapGen.floodfillPoints; y++)
            {
                Vector2Int point = new Vector2Int(mapGen.mapSize / mapGen.floodfillPoints * i, mapGen.mapSize / mapGen.floodfillPoints * y);
                samples.Add(point);
            }
        }

        int count = 0;
        foreach (var sample in samples)
        {
            if (globalSet.Contains(sample) || mapGen.noiseMap[sample.x, sample.y] < mapGen.threshold)
            {
                continue;
            }
            set = new HashSet<Vector2Int>();
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
            count++;
            landMasses.Add(count.ToString(), set);
        }
    }

    public float[,] CreateOutline(float[,] noiseMap)
    {
        foreach (var item in globalSet)
        {
            if (noiseMap[item.x, item.y] > mapGen.threshold)
            {
                if (item.x + 1 < mapGen.mapSize && noiseMap[item.x + 1, item.y] < mapGen.threshold)
                {
                    noiseMap[item.x + 1, item.y] = 1f;
                }

                if (item.x - 1 >= 0 && noiseMap[item.x - 1, item.y] < mapGen.threshold)
                {
                    noiseMap[item.x - 1, item.y] = 1f;
                }

                if (item.y + 1 < mapGen.mapSize && noiseMap[item.x, item.y + 1] < mapGen.threshold)
                {
                    noiseMap[item.x, item.y + 1] = 1f;
                }

                if (item.y - 1 >= 0 && noiseMap[item.x, item.y - 1] < mapGen.threshold)
                {
                    noiseMap[item.x, item.y - 1] = 1f;
                }
            }
        }
        return noiseMap;
    }

}
