using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAdjustValue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtLabel;
    [SerializeField] int value;
    [SerializeField] int stepValue;
    [SerializeField] GameObject settingText;
    [SerializeField] Button btnMinus;
    [SerializeField] Button btnPlus;

    List<int> datas = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        value = 0;
        SetText();
    }
    public void OnMinus()
    {
        if (value == 0) return;

        SoundManager.Instance.PlaySFX("Click");

        value -= stepValue;
        if (value < 0)
            value = 0;

        SetText();
    }
    public void OnPlus()
    {
        SoundManager.Instance.PlaySFX("Click");

        value += stepValue;
        SetText();
    }
    void SetText()
    {
        txtLabel.text = value + "";

        if (value == 0)
        {
            settingText.SetActive(true);
            txtLabel.gameObject.SetActive(false);
            btnMinus.interactable = false;
        }
        else
        {
            settingText.SetActive(false);
            txtLabel.gameObject.SetActive(true);
            btnMinus.interactable = true;
        }
    }
    
    public int GetValue()
    {
        return value;
    }
}
