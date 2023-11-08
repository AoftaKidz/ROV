using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIAutoSpinSettingPopup : MonoBehaviour
{
    public static UIAutoSpinSettingPopup Instance = null;
    [SerializeField] GameObject selectAutoSpin;
    [SerializeField] GameObject group;
    [SerializeField] GameObject content;
    [SerializeField] GameObject fade;
    [SerializeField] Button btnNormalMode;
    [SerializeField] Button btnTurboMode;
    [SerializeField] Color activeColor;
    [SerializeField] Color inActiveColor;
    [SerializeField] UIAdjustValue adjustDecBalanceStop;
    [SerializeField] UIAdjustValue adjustincBalanceStop;
    [SerializeField] UIAdjustValue adjustRewardStop;

    public double startBalance = 0;
    public int decreaseBalanceStop = 0;
    public int increaseBalanceStop = 0;
    public int rewardStop = 0;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;

    }
    // Update is called once per frame
    void Update()
    {
        
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
    public void OnStart()
    {
      /*  startBalance = UserProfile.Instance.wallet;
        decreaseBalanceStop = adjustDecBalanceStop.GetValue();
        increaseBalanceStop = adjustincBalanceStop.GetValue();
        rewardStop = adjustRewardStop.GetValue();
        SlotMachineAutoSpin.Instance.StartAotuSpin(selectAutoSpin.GetComponent<UISelectAutoSpin>().GetValue());*/

        Hide();
    }
    public void OnClose()
    {
        Hide();
    }
    public void Show()
    {
        Puzzle.isEnableClick = false;
        content.SetActive(true);
        fade.SetActive(true);
        content.transform.DOLocalMoveY(1117, 0.5f).SetEase(Ease.OutQuint);
    }
    public void Hide()
    {
        //SoundManager.Instance.PlaySFX("Close");
        Setting();
        Puzzle.isEnableClick = true;
        fade.SetActive(false);
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
        });
    }
}
