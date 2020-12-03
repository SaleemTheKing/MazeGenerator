using System;
using UnityEngine;
using UnityEngine.UI;

public class ValueSlider : MonoBehaviour
{
    private Slider _slider;
    public Text amountText;
    
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        UpdateText();
    }

    public void UpdateText()
    {
        amountText.text = String.Format("{0} walls", _slider.value);
    }
}
