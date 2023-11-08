using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIWinBetPopup : MonoBehaviour
{
    enum WinBetState
    {
        None = 0,
        FiveOfKind,
        Win
    }
    WinBetState _state = WinBetState.None;
    public static UIWinBetPopup Instance = null;
    [SerializeField] GameObject group;
    [SerializeField] GameObject content;
    [SerializeField] TextMeshProUGUI txtReward;

    [SerializeField] float ratioBigWin = 10;
    [SerializeField] float ratioMegaWin = 20;
    [SerializeField] float ratioSuperMegaWin = 30;
    [SerializeField] float delayFiveOfKind = 2.2f;
    [SerializeField] float delayWin = 2.2f;
    [SerializeField] SkeletonGraphic spine5kind;
    [SerializeField] SkeletonGraphic spineBigwin;

    public bool isAppear = false;
    bool _isAnimate = false;
    float _time = 0;
    bool _isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //ShowFiveOfKind();
        ShowWin();
    }

    // Update is called once per frame
    void Update()
    {
        //if (SlotMachineScatterMode.Instance.isWildSpawning) return;

        HandleState();
    }
    void HandleState()
    {
        switch (_state)
        {
            case WinBetState.None:
                {
                    break;
                }
            case WinBetState.FiveOfKind:
                {
                    _time += Time.deltaTime;
                    if(_time > delayFiveOfKind)
                    {
                        _time = 0;
                        HideFiveOfKind();
                    }
                    break;
                }
            case WinBetState.Win:
                {
                    _time += Time.deltaTime;
                    if(_time > delayWin)
                    {
                        HideWin();
                    }
                    break;
                }
        }
    }
    void HideAllWin()
    {
        spine5kind.gameObject.SetActive(false);
        spineBigwin.gameObject.SetActive(false);
    }
    void SetState()
    {
        BetModel data = SlotMachine.Instance.slotData;
        HideAllWin();

        if (data.isFiveOfKind)
        {
            ShowFiveOfKind();
        }
        else
        {
            ShowWin();
        }
    }
    void ShowFiveOfKind()
    {
/*        float more_delay = 0;
        if (SlotMachine.Instance.slotData.wildSpawnIndex >= 0)
            more_delay = 0f;*/

        int skinId = 2;
        int anim = 3;

        spine5kind.gameObject.SetActive(true);
        //CHange Skin name
        if (skinId == 0)
        {
            spine5kind.Skeleton.SetSkin("Blue");
        }else if(skinId == 1)
        {
            spine5kind.Skeleton.SetSkin("Red");
        }
        else
        {
            spine5kind.Skeleton.SetSkin("Yellow");
        }

        //Change animation
        if(anim == 0)
        {
            //spine5kind.Start
            spine5kind.AnimationState.SetAnimation(0, "5_of_Kind_Meow_1", false);
            SoundManager.Instance.PlaySFX("5_of_Kind");
            _state = WinBetState.FiveOfKind;
        }
        else if(anim == 1)
        {
            spine5kind.AnimationState.SetAnimation(0, "5_of_Kind_Meow_2", false);
            SoundManager.Instance.PlaySFX("5_of_Kind");
            _state = WinBetState.FiveOfKind;
        }
        else if(anim == 2)
        {
            spine5kind.AnimationState.SetAnimation(0, "5_of_Kind_Meow_3", false);
            SoundManager.Instance.PlaySFX("5_of_Kind");
            _state = WinBetState.FiveOfKind;
        }
        else if(anim == 3)
        {
            spine5kind.AnimationState.SetAnimation(0, "5_of_Kind_Meow_4", false);
            SoundManager.Instance.PlaySFX("5_of_Kind");
            _state = WinBetState.FiveOfKind;
        }
        else if(anim == 4)
        {
            spine5kind.AnimationState.SetAnimation(0, "5_of_Kind_Meow_5", false);
            SoundManager.Instance.PlaySFX("5_of_Kind");
            _state = WinBetState.FiveOfKind;
        }
    }
    void HideFiveOfKind()
    {
        _state = WinBetState.None;
        spine5kind.gameObject.SetActive(false);
        if (SlotMachine.Instance.slotData.winRatio >= ratioBigWin)
        {
            ShowWin();
        }
        else
        {
            Hide();
        }
    }
    void ShowWin()
    {
        float more_delay = 0;
        //if (SlotMachine.Instance.slotData.wildSpawnIndex >= 0)
           // more_delay = 0f;
        //More delay 
        HideAllWin();
        _state = WinBetState.Win;
        content.SetActive(true);
        spineBigwin.gameObject.SetActive(true);

        BetModel data = new BetModel();//SlotMachine.Instance.slotData;
        data.reward = 2500;
        data.winRatio = 50;

        var formatedWallet = string.Format("WIN : {0:#,#.00}", data.reward);
        txtReward.text = formatedWallet;

        if (data.winRatio >= ratioSuperMegaWin)
        {
            SoundManager.Instance.PlaySFX("BigWin");
            spineBigwin.AnimationState.SetAnimation(0, "BigWin_Start", false);

        }
        else if (data.winRatio >= ratioMegaWin)
        {
            SoundManager.Instance.PlaySFX("MegaWin");
            spineBigwin.AnimationState.SetAnimation(0, "BigWin_Start", false);
        }
        else if (data.winRatio >= ratioBigWin)
        {
            SoundManager.Instance.PlaySFX("SuperMegaWin");
            spineBigwin.AnimationState.SetAnimation(0, "BigWin_Start", false);
        }

        txtReward.transform.localScale = Vector3.zero;
        txtReward.transform.DOScale(new Vector3(1, 1, 1), 2).SetEase(Ease.OutElastic).OnComplete(() => {
            
        });

        /* objWin.transform.DOLocalMoveY(-2778, more_delay).SetEase(Ease.OutQuint).OnComplete(() => {
             fade.SetActive(true);
             objWin.SetActive(true);
             _state = WinBetState.Win;

             var formatedWallet = string.Format("WIN : {0:#,#.00}", SlotMachine.Instance.slotData.reward);
             txtReward.text = formatedWallet;

             objWin.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuint).OnComplete(() => {
             BetModel data = SlotMachine.Instance.slotData;
             GameObject w = null;
             HideAllWin();

                 float winRate = data.reward / (float)data.v;
             if (winRate >= ratioSuperMegaWin)
             {
                 SoundManager.Instance.PlaySFX("BigWin");
                 w = imgSuperBigWin;
             }
             else if (winRate >= ratioMegaWin)
             {
                 SoundManager.Instance.PlaySFX("MegaWin");
                 w = imgMegaWin;
             }
             else if (winRate >= ratioBigWin)
             {
                 SoundManager.Instance.PlaySFX("SuperMegaWin");
                 w = imgBigWin;
             }
             if (w)
             {
                 w.SetActive(true);
                 w.transform.localScale = Vector3.zero;
                 w.transform.DOScale(1, 0.6f).SetEase(Ease.OutElastic);
             }
         });

         });*/
    }
    void HideWin()
    {
        _state = WinBetState.None;
        Hide();
    }
    public void Show()
    {
        float more_delay = 0;
        if (SlotMachine.Instance.slotData.wildSpawnIndex >= 0)
            more_delay = 4f;
        //More delay 
        SetState();
        _time = 0;
        isAppear = true;
        content.SetActive(true);
        Puzzle.isEnableClick = false;
        content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(ShowFinish);

    }
    void ShowFinish()
    {

    }
    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");

        content.transform.DOLocalMoveY(0, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            HideFinish();
        });
    }
    void HideFinish()
    {
        isAppear = false;
        content.SetActive(false);
        Puzzle.isEnableClick = true;

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
    public bool Condition()
    {
        if (SlotMachine.Instance.slotData.reward == 0) return false;
        float winRate = SlotMachine.Instance.slotData.reward / (float)SlotMachine.Instance.slotData.v;

        if (winRate >= ratioBigWin || SlotMachine.Instance.slotData.isFiveOfKind)
            return true;
        return false;
    }
}
