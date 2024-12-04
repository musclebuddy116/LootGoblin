using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        int currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", resolutions.Length - 1);
        for(int i = 0; i < resolutions.Length; i++) {
            string resolutionString = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutionString));
        }
        currentResolutionIndex = Math.Min(currentResolutionIndex, resolutions.Length-1);
        resolutionDropdown.value = currentResolutionIndex;
        SetResolution();
    }

    public void SetResolution() {
        int resIndex = resolutionDropdown.value;
        Screen.SetResolution(resolutions[resIndex].width, resolutions[resIndex].height, true);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
    }
}
