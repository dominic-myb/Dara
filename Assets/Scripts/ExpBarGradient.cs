using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarGradient : MonoBehaviour
{
    public Gradient gradient;
    public Slider slider;
    public Image fill;
    public void SetMaxExp(int maxExp, int exp)
    {
        slider.maxValue = maxExp;
        slider.value = exp;
        fill.color = gradient.Evaluate(exp);
    }
    public void SetExp(int exp)
    {
        slider.value = exp;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
