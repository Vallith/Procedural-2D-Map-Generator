using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipHover : MonoBehaviour, IPointerEnterHandler
{
    OptionsManager optionsManager;
    public string tooltipText;

    private void Awake()
    {
        optionsManager = FindObjectOfType<OptionsManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        optionsManager.tooltipBox.text = tooltipText;
    }
}
