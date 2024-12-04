using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PercentageSlider : MonoBehaviour
{
    [SerializeField] Slider mySlider;
    [SerializeField] TextMeshProUGUI percentageText;

    public void SetPercentage() {
        percentageText.text = (mySlider.value * 100).ToString("F0") + "%";
    }
}
