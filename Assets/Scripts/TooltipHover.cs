using UnityEngine;
using UnityEngine.EventSystems;


public class TooltipHover : MonoBehaviour, IPointerEnterHandler
{
    OptionsManager optionsManager;
    MenuSliderGroup sliderGroup;

    private void Awake()
    {
        optionsManager = FindObjectOfType<OptionsManager>();
        sliderGroup = GetComponentInParent<MenuSliderGroup>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        optionsManager.tooltipBox.text = sliderGroup.tooltipText;
    }
}
