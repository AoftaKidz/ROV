using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIAutoSpinPopup : MonoBehaviour
{
    [SerializeField] GameObject selectAutoSpin;
    [SerializeField] GameObject group;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject content;
    [SerializeField] Button btnNormalMode;
    [SerializeField] Button btnTurboMode;
    [SerializeField] Color activeColor;
    [SerializeField] Color inActiveColor;

    public static UIAutoSpinPopup Instance = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }
    void Setting()
    {
        if (SlotMachine.isTurboMode)
        {
            string btnTurboFileName = "UI/Popup/Btn_Blue2_Active";
            string btnNormalFileName = "UI/Popup/Btn_Blue2";
            btnNormalMode.GetComponent<Image>().sprite = Resources.Load<Sprite>(btnNormalFileName);
            btnTurboMode.GetComponent<Image>().sprite = Resources.Load<Sprite>(btnTurboFileName);
            btnTurboMode.GetComponentInChildren<TextMeshProUGUI>().color = activeColor;
            btnNormalMode.GetComponentInChildren<TextMeshProUGUI>().color = inActiveColor;

        }
        else
        {
            string btnTurboFileName = "UI/Popup/Btn_Blue2";
            string btnNormalFileName = "UI/Popup/Btn_Blue2_Active";
            btnNormalMode.GetComponent<Image>().sprite = Resources.Load<Sprite>(btnNormalFileName);
            btnTurboMode.GetComponent<Image>().sprite = Resources.Load<Sprite>(btnTurboFileName);
            btnTurboMode.GetComponentInChildren<TextMeshProUGUI>().color = inActiveColor;
            btnNormalMode.GetComponentInChildren<TextMeshProUGUI>().color = activeColor;
        }
    }
    public void OnNormalMode()
    {
        if (!SlotMachine.isTurboMode) return;
        UIGameplay.Instance.OnClickTurbo();
        Setting();
    }
    public void OnTurboMode()
    {
        if (SlotMachine.isTurboMode) return;
        UIGameplay.Instance.OnClickTurbo();
        Setting();
    }
    public void OnClose()
    {
        Hide();
    }
    public void OnStart()
    {
        SlotMachineAutoSpin.Instance.StartAotuSpin(selectAutoSpin.GetComponent<UISelectAutoSpin>().GetValue());
        Hide();
    }
    public void Show()
    {
        Puzzle.isEnableClick = false;
        content.SetActive(true);
        Setting();
        fade.SetActive(true);
        content.transform.DOLocalMoveY(1117, 0.5f).SetEase(Ease.OutQuint);
    }
    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");

        Puzzle.isEnableClick = true;
        fade.SetActive(false);
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
        });
    }
}
