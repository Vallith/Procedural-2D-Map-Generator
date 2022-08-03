using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MenuInputGroup : MonoBehaviour
{
    public float defaultValue;
    public MapOptionType optionType;
    public bool roundValue;
    OptionsManager optionsManager;
    TMP_InputField inputField;
    
    // Start is called before the first frame update
    void Start()
    {
        optionsManager = FindObjectOfType<OptionsManager>();
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(delegate { InputFieldValueChange(); });

        inputField.text = defaultValue.ToString();
    }

    private void InputFieldValueChange()
    {
        int.TryParse(inputField.text, out int parsedValue);
        optionsManager.UpdateOption(optionType, parsedValue);
    }

}
