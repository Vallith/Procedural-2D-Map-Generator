using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourSectionController : MonoBehaviour, IDragHandler
{
    public RectTransform bar;
    public GradientUI.Node node;
    public GameObject colourSection;
    public Image img2;
    public void OnDrag(PointerEventData eventData)
    {
        RectTransform selfTransform = GetComponent<RectTransform>();
        float newY = Input.mousePosition.y;
        newY = Mathf.Clamp(newY, bar.rect.yMin + bar.position.y, bar.rect.yMax + bar.position.y);
        selfTransform.position = new Vector3(bar.position.x, newY, 0f);

        node.position = ((newY - bar.position.y) / bar.rect.height) + 0.5f;

        UpdateColourSection();
    }

    public void SetColour()
    {
        Image img1 = GetComponent<Image>();

        Color.RGBToHSV(node.color, out float h, out float s, out float v);
        v *= 0.6f;
        img2.color = node.color;
        img1.color = Color.HSVToRGB(h, s, v);

        colourSection.GetComponent<Image>().color = node.color;
    }

    void UpdateColourSection()
    {
        RectTransform rt = colourSection.GetComponent<RectTransform>();
        float change = Utils.Map(node.position, 0, 1, 0, bar.rect.height);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, change);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, change / 2f);
    }
}
