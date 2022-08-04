using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuSliderGroup : MonoBehaviour
{
    public float defaultValue;
    public Slider slider;
    public TMP_InputField inputField;
    public MapOptionType optionType;
    public bool roundValue;
    OptionsManager optionsManager;

    // Start is called before the first frame update
    void Start()
    {
        optionsManager = FindObjectOfType<OptionsManager>();
        slider.onValueChanged.AddListener(delegate { SliderValueChange(); });
        inputField.onEndEdit.AddListener (delegate { InputFieldValueChange(); });

        slider.value = defaultValue;
        inputField.text = defaultValue.ToString();

    }

    public void SliderValueChange()
    {
        if(roundValue)
        {
            inputField.text = Math.Round((decimal)slider.value, 2).ToString();
        }
        else
        {
            inputField.text = slider.value.ToString();
        }
        OnChange();
    }

    public void InputFieldValueChange()
    {
        int.TryParse(inputField.text, out int parsedValue);
        slider.value = parsedValue;
        OnChange();
    }

    void OnChange()
    {
        optionsManager.UpdateOption(optionType, slider.value);
    }
}
