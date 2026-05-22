using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeParameterUIBuilder : MonoBehaviour, IParameterUIBuilder
{
    [Header("UI Parent")]
    [SerializeField] private Transform parameterUIGridParent;

    [Header("Parameter Settings Prefabs")]
    [SerializeField] private GameObject sliderPrefab;
    [SerializeField] private GameObject togglePrefab;
    [SerializeField] private GameObject dropdownPrefab;

    public void AddSlider(string labelText, float min, float max, float value, Action<float> onChanged)
    {
        GameObject sliderObj = Instantiate(sliderPrefab, parameterUIGridParent);

        var slider = sliderObj.GetComponentInChildren<Slider>();
        var label = sliderObj.GetComponentInChildren<TMP_Text>();

        label.text = labelText;

        slider.minValue = min;
        slider.maxValue = max;
        slider.value = value;

        slider.onValueChanged.AddListener(sliderValue => onChanged(sliderValue));
    }

    public void AddToggle(string labelText, bool value, Action<bool> onChanged)
    {
        GameObject toggleObj = Instantiate(togglePrefab, parameterUIGridParent);

        var toggle = toggleObj.GetComponentInChildren<Toggle>();
        var label = toggleObj.GetComponentInChildren<TMP_Text>();

        label.text = labelText;

        toggle.isOn = value;
        toggle.onValueChanged.AddListener(toggleValue => onChanged(toggleValue));
    }

    public void AddDropdown(string labelText, string[] options, int index, Action<int> onChanged)
    {
        GameObject dropdownObj = Instantiate(dropdownPrefab, parameterUIGridParent);

        var dropdown = dropdownObj.GetComponentInChildren<TMP_Dropdown>();
        var label = dropdownObj.GetComponentInChildren<TMP_Text>();

        label.text = labelText;

        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(options));
        dropdown.value = index;

        dropdown.onValueChanged.AddListener(dropdownIndex => onChanged(dropdownIndex));
    }

    public void BuildUI(VisualDelegate selectedDelegate)
    {
        ClearUI();

        if (selectedDelegate == null) return;

        //selectedDelegate.BuildUI(this);
    }

    private void ClearUI()
    {
        foreach (Transform child in parameterUIGridParent) 
        {
            Destroy(child.gameObject);
        }
    }
}