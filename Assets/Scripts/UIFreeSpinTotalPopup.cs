using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIFreeSpinTotalPopup : MonoBehaviour
{
    public static UIFreeSpinTotalPopup Instance = null;
    [SerializeField] GameObject group;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject content;
    [SerializeField] SkeletonGraphic spine;
    [SerializeField] TextMeshProUGUI txtReward;
    [SerializeField] float delay = 4;
    [SerializeField] RewardAnimate rewardAnimate;
    [SerializeField] GameObject btnClose;

    float time = 0;
    bool isStart = false;
    bool isShowReward = false;
    bool isHideReward = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //Show();
    }
    private void Update()
    {
        if (!isStart) return;

        time += Time.deltaTime;
        if(!isShowReward && time > 1)
        {
            UpdateReward();
        }
        /*else if(!isHideReward && time > 7)
        {
            isHideReward = true;
            txtReward.transform.DOScale(0, 0.6f).SetEase(Ease.OutQuint);
        }*/
        else if(time > delay)
        {
            //Hide();
            btnClose.SetActive(true);
            isStart = false;
            spine.AnimationState.SetAnimation(0, "TotalWin_Idle", true);
        }
    }
    void UpdateReward()
    {
        isShowReward = true;
        //SlotMachine.Instance.slotData.totalReward = 1234.56f;
        //rewardAnimate.StartAnimate(1234.56f);
        rewardAnimate.StartAnimate(SlotMachine.Instance.slotData.totalReward);

        return;
        if (SlotMachine.Instance.slotData.totalReward == 0)
        {
            //txtReward.text = SpriteNumberManager.ToRed("0.00");
            rewardAnimate.StartAnimate(SlotMachine.Instance.slotData.totalReward);
        }
        else
        {
            var format = string.Format("{0:#,#.00}", SlotMachine.Instance.slotData.totalReward);
            txtReward.text = SpriteNumberManager.ToMeowWhite(format);
        }
    }
    public void Show()
    {
        UserProfile.Instance.CallUpdateUserProfile();
        btnClose.SetActive(false);
        BGMachine.Instance.Normal();
        SoundManager.Instance.PlayBGM("BGM Gameplay");

        //BGMachine.Instance.FreeSpin_CatIdle();
        time = 0;
        isStart = true;
        isShowReward = false;
        isHideReward = false;
        SoundManager.Instance.PlaySFX("WinResult");
        UIRoundRewardPopup.Instance.Hide();
        txtReward.text = "";
        spine.AnimationState.SetAnimation(0, "TotalWin_Start", false);
 
        txtReward.transform.localScale = Vector3.zero;
        txtReward.transform.DOScale(1, 1).SetEase(Ease.OutElastic);

        content.SetActive(true);
        Puzzle.isEnableClick = false;
        fade.SetActive(true);
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuart);
    }
    public void Hide()
    {
        isStart = false;
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
            fade.SetActive(false);
            Puzzle.isEnableClick = true;

            if (SlotMachine.isAutoMode)
            {
                SlotMachineAutoSpin.Instance.AutoSpin();
            }
        });
    }
}
