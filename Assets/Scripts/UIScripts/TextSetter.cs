using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;


    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetText(int value)
    {
        SetText(value.ToString("F1"));
    }   
    public void SetText(float value)
    {
        SetText(value.ToString("F1"));
    }
}
