using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIBigWinPopup : MonoBehaviour
{
    public static UIBigWinPopup Instance = null;
    [SerializeField] GameObject group;
    [SerializeField] GameObject content;
    [SerializeField] TextMeshProUGUI txtReward;
    [SerializeField] SkeletonGraphic spineBigwin;
    [SerializeField] float delay = 5;
    [SerializeField] float bigwinRatio = 10;
    [SerializeField] RewardAnimate rewardAnim;

    float time = 0;
    bool isStart = false;
    bool isShowReward = false;

    void Start()
    {
        Instance = this;
        //Show();
    }
    void Update()
    {
        if (!isStart) return;

        time += Time.deltaTime;
        if(!isShowReward && time > 1)
        {
            ShowReward();
        }
        else if(time > delay)
        {
            Hide();
        }
    }
    public void Show()
    {
        if (!Condition()) return;
        //spineBigwin.gameObject.SetActive(true);

        isStart = true;
        isShowReward = false;
        content.SetActive(true);
        SoundManager.Instance.PlaySFX("AllWin");
        spineBigwin.AnimationState.SetAnimation(0, "BigWin_Idle", false);
        spineBigwin.transform.localScale = Vector3.one / 2f;
        spineBigwin.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutElastic);
        txtReward.text = "";
        txtReward.transform.localScale = Vector3.zero;

    }
    public void Hide()
    {
        time = 0;
        isStart = false;
        isShowReward = false;
        content.SetActive(false);
        //spineBigwin.AnimationState.SetAnimation(0, "BigWin_Loop", false);
        //spineBigwin.gameObject.SetActive(false);

        //Check for Free spin mode
        SlotMachine slot = SlotMachine.Instance;
        if (slot.slotData.isScatterMode == false && slot.slotData.comingFreeSpinCount > 0)
        {
            //Starting scatter mode
            UIFreeSpinPopup.Instance.Show(slot.slotData.scatterCount);
        }
        else if (slot.slotData.isScatterMode)
        {
            //Update round reward
            //UIRoundRewardPopup.Instance.Show(0.3f);
        }
        else if (SlotMachine.isAutoMode)
        {
            SlotMachineAutoSpin.Instance.AutoSpin();
        }
    }
    void ShowReward()
    {
        //SoundManager.Instance.PlaySFX("CatMeow");
        //SlotMachine.Instance.slotData = new BetModel();
        isShowReward = true;
        BetModel data = SlotMachine.Instance.slotData;
/*        data.reward = 2500;
        data.winRatio = 50;*/

        //var formatedWallet = string.Format("WIN : {0:#,#.00}", data.reward);
        //txtReward.text = formatedWallet;
        txtReward.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        txtReward.transform.DOScale(new Vector3(1,1,1),1.0f).SetEase(Ease.OutElastic);

        rewardAnim.StartAnimate(data.reward);
    }
    public bool Condition()
    {
        return false;
        if (SlotMachine.Instance == null) return false;
        if (SlotMachine.Instance.slotData == null) return false;
        if (SlotMachine.Instance.slotData.reward == 0) return false;
        if (SlotMachine.Instance.slotData.winRatio == 0) return false;

        float rate = SlotMachine.Instance.slotData.reward / SlotMachine.Instance.slotData.v;
        if (rate >= bigwinRatio)
            return true;
        return false;
    }
    public bool Appear()
    {
        return isStart;
    }
}
