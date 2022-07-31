using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                UpdateMap(mapGen);
            }
        }
        if (GUILayout.Button("Generate Map"))
        {
            UpdateMap(mapGen);
        }
    }

    void UpdateMap(MapGenerator mapGen)
    {
        float w = mapGen.mapWidth;
        float h = mapGen.mapHeight;
        float x = w * 0.5f - 0.5f;
        float y = h * 0.5f - 0.5f;
        Camera.main.orthographicSize = (((w > h * Camera.main.aspect) ? (float)w / (float)Camera.main.pixelWidth * Camera.main.pixelHeight : h) / 2 + 2) * 10f;
        mapGen.DrawMapInEditor();
    }
}
