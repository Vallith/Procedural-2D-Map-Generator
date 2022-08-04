using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuDropdownGroup : MonoBehaviour
{
    OptionsManager optionsManager;
    public MapOptionType optionType;
    public TMP_Dropdown dropdown;
    public int defaultIndex;

    public GameObject conditionalObject;
    public string conditionalOptionName;

    // Start is called before the first frame update
    void Start()
    {
        optionsManager = FindObjectOfType<OptionsManager>();
        dropdown.onValueChanged.AddListener(delegate { OnChange(); });
        GetOptions();
    }

    void GetOptions()
    {
        dropdown.options.Clear();
        string[] options = null;
        switch (optionType)
        {
            case MapOptionType.NoiseType:
                options = new string[]{ "OpenSimplex2S", "Perlin", "Cellular", "ValueCubic" };

                break;
            case MapOptionType.FractalType:
                options = Enum.GetNames(typeof(FastNoiseLite.FractalType));

                break;

            case MapOptionType.CellularReturnType:
                options = Enum.GetNames(typeof(FastNoiseLite.CellularReturnType));

                break;

            case MapOptionType.CellularDistanceType:
                options = Enum.GetNames(typeof(FastNoiseLite.CellularDistanceFunction));

                break;
            default:
                break;
        }

        dropdown.AddOptions(options.Select(x =>
        {
            return new TMP_Dropdown.OptionData(x);
        }).ToList());

        dropdown.value = defaultIndex;
        dropdown.captionText.text = dropdown.options[dropdown.value].text;
    }

    void OnChange()
    {
        optionsManager.UpdateOption(optionType, dropdown.options[dropdown.value].text);

        if (conditionalObject != null && conditionalOptionName != null && dropdown.options[dropdown.value].text == conditionalOptionName)
        { conditionalObject.SetActive(true); }
        else if(conditionalObject != null)
        { conditionalObject.SetActive(false); }
    }
}
