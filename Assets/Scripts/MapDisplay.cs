using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void ClearTexture()
    {
        Texture2D texture = (Texture2D)textureRenderer.sharedMaterial.mainTexture;
        Color[] blank = new Color[texture.width * texture.height];
        for (int i = 0; i < blank.Length; i++)
        {
            blank[i] = Color.white;
        }
        texture.SetPixels(blank);
        texture.Apply();
    }   
}
