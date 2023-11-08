using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using System;

public class UIBetPopup : MonoBehaviour
{
    static public event Action<float> OnBetTotal;

    [SerializeField] GameObject group;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject content;
    [SerializeField] SimpleScrollSnap pickerSize;
    [SerializeField] SimpleScrollSnap pickerLevel;
    [SerializeField] SimpleScrollSnap pickerTotal;
    [SerializeField] GameObject prefabPickerNumer;
    [SerializeField] TextMeshProUGUI txtBet;
    [SerializeField] GameObject alert;
    [SerializeField] TextMeshProUGUI txtChangeBetValue;
    public float[] sizes;
    public float[] levels;
    public float[] totals;
    bool isAlert = false;
    bool _isSelectSizeOrLevel = false;
    bool _isSelectTotal = false;
    int _selectTotalCount = 0;
    public static float lastBetValue = 0;
    public static UIBetPopup Instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        UpdateData();

    }
    void UpdateData()
    {

        if (pickerSize)
        {
            for (int i = 0; i < sizes.Length; i++)
            {
                GameObject o = Instantiate(prefabPickerNumer, Vector3.zero, Quaternion.identity);
                var formatedWallet = string.Format("{0:#.00}", sizes[i]);
                if (i > 1)
                    o.GetComponentInChildren<TextMeshProUGUI>().text = "0" + formatedWallet;
                else
                    o.GetComponentInChildren<TextMeshProUGUI>().text = formatedWallet;

                pickerSize.AddToBack(o);
            }
            //pickerSize.GoToPanel(3);
        }
        if (pickerLevel)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                GameObject o = Instantiate(prefabPickerNumer, Vector3.zero, Quaternion.identity);
                var formatedWallet = string.Format("{0:#,#}", levels[i]);
                o.GetComponentInChildren<TextMeshProUGUI>().text = formatedWallet;
                //pickerSize.Add(o,i);
                pickerLevel.AddToBack(o);
            }
            //pickerLevel.GoToPanel(3);
        }
        if (pickerTotal)
        {
            for (int i = 0; i < totals.Length; i++)
            {
                GameObject o = Instantiate(prefabPickerNumer, Vector3.zero, Quaternion.identity);
                var formatedWallet = string.Format("{0:#,#.00}", totals[i]);
                o.GetComponentInChildren<TextMeshProUGUI>().text = formatedWallet;
                //pickerSize.Add(o,i);
                pickerTotal.AddToBack(o);
            }


            int index = 0;
            for (int i = 0; i < totals.Length; i++)
            {
                if (UserProfile.Instance.betTotal == totals[i])
                {
                    index = i;
                    break;
                }
            }
            pickerTotal.GoToPanel(index);

            OnBetTotal?.Invoke(totals[index]);

        }

        UpdatePickerColor();
    }
    public void OnSelectBetSize()
    {
        if (!pickerSize) return;
        if (_selectTotalCount > 0)
        {
            _selectTotalCount--;
            UpdatePickerColor();

            return;
        }
        _isSelectSizeOrLevel = true;

        int s = pickerSize.CenteredPanel;
        int l = pickerLevel.CenteredPanel;
        float val = sizes[s] * levels[l] * 15.0f;
        Debug.Log(val);
        int totalIndex = 0;
        for (int i = 0; i < totals.Length; i++)
        {
            int a = (int)totals[i];
            int b = (int)val;
            if (a == b)
                totalIndex = i;
        }

        pickerTotal.GoToPanel(totalIndex);
        UpdatePickerColor();
    }
    public void OnSelectBetLevel()
    {
        if (!pickerLevel) return;
        if (_selectTotalCount > 0)
        {
            _selectTotalCount--;
            UpdatePickerColor();

            return;
        }
        _isSelectSizeOrLevel = true;
        int s = pickerSize.CenteredPanel;
        int l = pickerLevel.CenteredPanel;
        float val = sizes[s] * levels[l] * 15.0f;
        Debug.Log(val);
        int totalIndex = 0;
        for (int i = 0; i < totals.Length; i++)
        {
            int a = (int)totals[i];
            int b = (int)val;
            if (a == b)
                totalIndex = i;
        }

        pickerTotal.GoToPanel(totalIndex);
        UpdatePickerColor();
    }
    public void OnSelectBetTotal()
    {
        if (!pickerTotal) return;

        if (!pickerTotal) return;
        if (_isSelectSizeOrLevel)
        {
            _isSelectSizeOrLevel = false;
            UpdatePickerColor();
            float bet = float.Parse(txtBet.text);
            ShowAlert(bet);
            return;
        }
        _isSelectTotal = true;
        _selectTotalCount = 2;
        //0.5,0.4,0.3,0.2
        //4,3,2,1
        int t = pickerTotal.CenteredPanel;
        float val = totals[t];

        float sl = val / 15.0f;
        int sizeIndex = 0;
        int levelIndex = 0;
        bool _done = false;
        foreach (var l in levels)
        {
            float _s = sl / l;
            sizeIndex = 0;
            foreach (var s in sizes)
            {
                if (_s == s)
                    _done = true;
                if (_done)
                    break;
                sizeIndex++;
            }
            if (_done)
                break;
            levelIndex++;
        }
        if (_done)
        {
            pickerSize.GoToPanel(sizeIndex);
            pickerLevel.GoToPanel(levelIndex);
        }

        {
            UpdatePickerColor();
            float bet = float.Parse(txtBet.text);
            ShowAlert(bet);
        }
        return;
        //0.5,0.4,0.3,0.2
        //4,3,2,1
        /*int t = pickerTotal.CenteredPanel;
        float val = totals[t];

        float sl = val / 15.0f;
        if(val == 3)
        {
            //0.2 , 1
            pickerSize.GoToPanel(3);
            pickerLevel.GoToPanel(3);
        }else if(val == 6)
        {
            //0.2 , 2
            pickerSize.GoToPanel(3);
            pickerLevel.GoToPanel(2);
        }
        else if (val == 9)
        {
            //0.2 , 3
            pickerSize.GoToPanel(3);
            pickerLevel.GoToPanel(1);
        }
        else if (val == 12)
        {
            //0.2 , 4
            pickerSize.GoToPanel(3);
            pickerLevel.GoToPanel(0);
        }
        else if (val == 4.5f)
        {
            //0.3 , 1
            pickerSize.GoToPanel(2);
            pickerLevel.GoToPanel(3);
        }
        else if (val == 13.5f)
        {
            //0.3 , 3
            pickerSize.GoToPanel(2);
            pickerLevel.GoToPanel(1);
        }
        else if (val == 18)
        {
            //0.3 , 4
            pickerSize.GoToPanel(2);
            pickerLevel.GoToPanel(0);
        }
        else if (val == 24)
        {
            //0.4 , 4
            pickerSize.GoToPanel(1);
            pickerLevel.GoToPanel(0);
        }
        else if (val == 7.5f)
        {
            //0.5 , 1
            pickerSize.GoToPanel(0);
            pickerLevel.GoToPanel(3);
        }
        else if (val == 15)
        {
            //0.5 , 2
            pickerSize.GoToPanel(0);
            pickerLevel.GoToPanel(2);
        }
        else if (val == 22.5f)
        {
            //0.5 , 3
            pickerSize.GoToPanel(0);
            pickerLevel.GoToPanel(1);
        }
        else if (val == 30)
        {
            //0.5 , 4
            pickerSize.GoToPanel(0);
            pickerLevel.GoToPanel(0);
        }*/

        //UpdatePickerColor();
    }
    void UpdatePickerColor()
    {
        txtBet.text = pickerTotal.Panels[pickerTotal.CenteredPanel].gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        {
            foreach (Transform o in pickerSize.Panels)
            {
                float c = 50.0f / 255.0f;
                o.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;// new Color(c, c, c);
            }
            pickerSize.Panels[pickerSize.CenteredPanel].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        {
            foreach (Transform o in pickerLevel.Panels)
            {
                float c = 50.0f / 255.0f;
                o.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;// new Color(c, c, c);
            }
            pickerLevel.Panels[pickerLevel.CenteredPanel].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        {
            foreach (Transform o in pickerTotal.Panels)
            {
                float c = 50.0f / 255.0f;
                o.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;//new Color(c, c, c);
            }
            pickerTotal.Panels[pickerTotal.CenteredPanel].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }
    public void Show()
    {
        if (SlotMachine.isAutoMode) return;

        content.SetActive(true);
        //ShowAlert();
        Puzzle.isEnableClick = false;
        int index = 0;
        for (int i = 0; i < totals.Length; i++)
        {
            if (UserProfile.Instance.betTotal == totals[i])
            {
                index = i;
                break;
            }
        }

        fade.SetActive(true);
        content.transform.DOLocalMoveY(1117, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => {
            pickerTotal.GoToPanel(index);
            UpdatePickerColor();
            float bet = float.Parse(txtBet.text);
            ShowAlert(bet);
        });
    }
    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");

        Puzzle.isEnableClick = true;
        fade.SetActive(false);
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
            HideAlert();
        });
    }
    public void OnConfirm()
    {
        Hide();
        float bet = float.Parse(txtBet.text);
        OnBetTotal?.Invoke(bet);
        UserProfile.Instance.betTotal = bet;
    }
    public void OnClose()
    {
        Hide();
    }
    public void OnMaxBet()
    {
    }
    void ShowAlert(float value = 0)
    {
        return;

        if (!SlotMachine.Instance.CheckWildActive() || lastBetValue == value)
        {
            HideAlert();
            return;
        }

        if (!isAlert)
        {
            isAlert = true;
            alert.transform.localScale = new Vector2(0.8f, 0.8f);
            alert.transform.DOScale(new Vector2(1, 1), 1).SetEase(Ease.OutElastic);
            alert.SetActive(true);
        }

        if (lastBetValue == 0)
            lastBetValue = UserProfile.Instance.betTotal;

        var formatedWallet = string.Format("{0:#,#.00}", lastBetValue);
        txtChangeBetValue.text = formatedWallet;
    }
    void HideAlert()
    {
        isAlert = false;
        alert.SetActive(false);
        alert.transform.localScale = new Vector2(0.5f, 0.5f);
    }
    public void ChangeBetValue(float value = 0)
    {
        if (lastBetValue == 0)
            lastBetValue = UserProfile.Instance.betTotal;

        UserProfile.Instance.betTotal = value;
    }
    public List<float> GetBetMultiplies(float value)
    {
        List<float> result = new List<float>();
        foreach (var b1 in sizes)
        {
            foreach (var b2 in levels)
            {
                var t = b1 * b2 * 15f;
                if (t == value)
                {
                    result.Add(b1);
                    result.Add(b2);
                    return result;
                }
            }
        }

        return result;
    }
}
