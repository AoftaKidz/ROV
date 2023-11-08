using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISelectAutoSpin : MonoBehaviour
{
    [SerializeField] Button []buttons;
    [SerializeField] int[] labels;
    int _activeButton = 0;
    [SerializeField] Color activeColor;
    [SerializeField] Color inActiveColor;

    // Start is called before the first frame update
    void Start()
    {
        SetButtons();
    }
    public void SetButtons()
    {
        for (int i = 0; i < 5;i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = labels[i] + "";
        }
        OnSelect(0);
    }
    public void OnSelect(int tag)
    {
        SoundManager.Instance.PlaySFX("Click");
        DisSelectAll();
        Button btn = buttons[tag];
        buttons[tag].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/button_pink_active");
        buttons[tag].GetComponentInChildren<TextMeshProUGUI>().color = activeColor;
        _activeButton = tag;
    }

    void DisSelectAll()
    {
        for (int i = 0; i < 5; i++)
        {
            buttons[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/button_pink_inactive");
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().color = inActiveColor;
        }
    }
    public int GetValue()
    {
        return labels[_activeButton];
    }
}
