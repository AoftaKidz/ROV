using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class UIGameplay : MonoBehaviour
{
    static public event Action OnUIGameplaySpinAction;
    static public event Action OnUIGameplayWildMove;

    public TextMeshProUGUI txtWallet;
    public TextMeshProUGUI txtBet;
    public TextMeshPro txtReward;
    public TextMeshPro txtScatterCount;
    public TextMeshProUGUI txtScatterMultiply;
    public TextMeshProUGUI txtAutospinNumber;

    public GameObject btnTurbo;
    [SerializeField] GameObject normalMode;
    [SerializeField] GameObject freeSpinMode;
    [SerializeField] GameObject autoSpinMode;
    [SerializeField] GameObject options;
    [SerializeField] GameObject heartTop;
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject topFX;
    [SerializeField] ParticleControl particleCounting;
    [SerializeField] GameObject btnSpin;
    [SerializeField] GameObject btnOption;
    [SerializeField] GameObject normal_bg;
    [SerializeField] GameObject normal_gachashop;
    [SerializeField] GameObject freespin_bg;
    [SerializeField] GameObject freespin_cat;
    [SerializeField] GameObject spineCoin;
    SkeletonGraphic _spineAnimation;
    [SerializeField] string coinAnimationNameIdle = "Coin_Idle";
    [SerializeField] string coinAnimationNameInsert = "Coin_Insert";
    [SerializeField] SkeletonGraphic spineFreeSpinTurboButton;
    [SerializeField] SkeletonGraphic spineFreeSpinAutoButton;
    [SerializeField] SkeletonGraphic spineFreeSpinSpinButton;


    bool _isNormalMode = true;
    bool _isOptionMode = false;
    bool _isFreeSpinMode = false;
    public List<float> betValues = new List<float>() { 3,4.5f,6,7.5f,9,12,13.5f,15,18,22.5f,24,30};
    public int betIndex = 0;
    public static UIGameplay Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    private void Start()
    {
        UpdateReward(0.00f);
        if (spineCoin)
        {
            //_spineAnimation = spineCoin.GetComponent<SkeletonGraphic>();
            CoinIdle();
        }
    }
    private void OnEnable()
    {
        SlotMachineAutoSpin.OnAutoSpin += SetAutoSpinCount;
        SlotMachineAutoSpin.OnEndterAutoSpin += AutoSpinMode;
        SlotMachineAutoSpin.OnStopAutoSpin += StopAutoSpin;
        UserProfile.OnUpdateUserProfile += UpdateWallet;
        UIBetPopup.OnBetTotal += UpdateBetTotal;
        SlotColumn.OnUpdateReward += UpdateReward;

    }
    private void OnDisable()
    {
        SlotMachineAutoSpin.OnEndterAutoSpin -= AutoSpinMode;
        SlotMachineAutoSpin.OnAutoSpin -= SetAutoSpinCount;
        SlotMachineAutoSpin.OnStopAutoSpin -= StopAutoSpin;
        UserProfile.OnUpdateUserProfile -= UpdateWallet;
        UIBetPopup.OnBetTotal -= UpdateBetTotal;
        SlotColumn.OnUpdateReward -= UpdateReward;

    }
    public void OnClickSpin()
    {
       // return;

        /*if (SlotMachine.Instance.slotData.isScatterMode == false && SlotMachine.Instance.slotData.comingFreeSpinCount > 0 && SlotMachine.isSpinning == false)
            return;*/
        float w = (float)UserProfile.Instance.wallet - UserProfile.Instance.betTotal;
        if (w < 0) return;

        if (SlotMachine.Instance.Busy()) return;

        OnUIGameplaySpinAction?.Invoke();
    }
    public void AnimateButtonSpin()
    {
        //btnSpin.transform.DORotate(new Vector3(0, 0, -360), 1, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }
    public void OnClickMinus()
    {
        SoundManager.Instance.PlaySFX("Click");
        for (int i = 0; i < betValues.Count; i++)
        {
            if (betValues[i] == UserProfile.Instance.betTotal)
            {
                if (i == 0) return;

                UserProfile.Instance.betTotal = betValues[i - 1];
                UpdateBetTotal(UserProfile.Instance.betTotal);
                return;
            }
        }
    }
    public void OnClickPlus()
    {
        //OnUIGameplayWildMove?.Invoke();
        SoundManager.Instance.PlaySFX("Click");
        
        for (int i = 0; i < betValues.Count;i++)
        {
            if(betValues[i] == UserProfile.Instance.betTotal)
            {
                if (i == betValues.Count - 1) return;

                UserProfile.Instance.betTotal = betValues[i+1];
                UpdateBetTotal(UserProfile.Instance.betTotal);
                return;
            }
        }
    }
    public void OnClickTurbo()
    {
        SoundManager.Instance.PlaySFX("Click");

        SlotMachine.isTurboMode = !SlotMachine.isTurboMode;
        if (SlotMachine.isTurboMode)
            SlotMachine.turbo = 4;
        else
            SlotMachine.turbo = 1;
        if (SlotMachine.isTurboMode)
        {
            Color c = btnTurbo.GetComponent<Image>().color;
            c.a = 1;
            btnTurbo.GetComponent<Image>().color = c;
        }
        else
        {
            Color c = btnTurbo.GetComponent<Image>().color;
            c.a = 0.3f;
            btnTurbo.GetComponent<Image>().color = c;
        }
        UpdateFreeSpinModeButtons();
    }
    public void OnClickAuto()
    {
        SoundManager.Instance.PlaySFX("Click");

        //UIAutoSpinPopup.Instance.Show();
        UIAutoSpinSettingPopup.Instance.Show();

    }
    public void OnClickFreeSpinAuto()
    {
        SoundManager.Instance.PlaySFX("Click");
        SlotMachine.isFreeSpinModeAuto = !SlotMachine.isFreeSpinModeAuto;

        UpdateFreeSpinModeButtons();
    }
    public void UpdateFreeSpinModeButtons()
    {
        //Spine button for free spin mode

        if (SlotMachine.isFreeSpinModeAuto)
        {
            Color c = spineFreeSpinAutoButton.color;
            c.a = 1;
            spineFreeSpinAutoButton.color = c;
        }
        else
        {
            Color c = spineFreeSpinAutoButton.color;
            c.a = 0.3f;
            spineFreeSpinAutoButton.color = c;
        }

        if (SlotMachine.isTurboMode)
        {
            Color c = spineFreeSpinTurboButton.color;
            c.a = 1;
            spineFreeSpinTurboButton.color = c;
        }
        else
        {
            Color c = spineFreeSpinTurboButton.color;
            c.a = 0.3f;
            spineFreeSpinTurboButton.color = c;
        }
    }
    public void OnClickHamburger()
    {
        SoundManager.Instance.PlaySFX("Click");

        OptionMode();
        //btnOption.SetActive(false);
    }
    public void OnClickCloseOption()
    {
        SoundManager.Instance.PlaySFX("Click");

        NormalMode();
       // btnOption.SetActive(true);
    }
    public void OnClickWallet()
    {
        SoundManager.Instance.PlaySFX("Click");

        UIWalletPopup.Instance.Show();
    }
    public void OnClickBet()
    {
        if (SlotMachine.isFreeSpinMode) return;
        if (SlotMachine.isAutoMode) return;

        SoundManager.Instance.PlaySFX("Click");

        UIBetPopup.Instance.Show();
    }
    void SetAutoSpinCount(int count)
    {
        //autoSpinMode.transform.Find("number").gameObject.GetComponent<UIFreeSpinNumbers>().SetNumber(count);
        txtAutospinNumber.text = SpriteNumberManager.ToMeowWhite(count + "");
        txtAutospinNumber.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        txtAutospinNumber.transform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic);
        OnClickSpin();
    }
    void StopAutoSpin()
    {
        //autoSpinMode.transform.Find("number").gameObject.GetComponent<UIFreeSpinNumbers>().SetNumber(0);
        txtAutospinNumber.text = SpriteNumberManager.ToMeowWhite("0");
        txtAutospinNumber.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        txtAutospinNumber.transform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic);
        autoSpinMode.SetActive(false);
        //_spineAnimation.startingAnimation = coinAnimationNameIdle;
    }
    public void AutoSpinMode(int count)
    {
        autoSpinMode.SetActive(true);
        //autoSpinMode.transform.Find("number").gameObject.GetComponent<UIFreeSpinNumbers>().SetNumber(count);
        txtAutospinNumber.text = SpriteNumberManager.ToMeowWhite(count + "");
        txtAutospinNumber.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        txtAutospinNumber.transform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic);
    }

    public void NormalMode()
    {
        //topFX.SetActive(false);
        btnOption.SetActive(transform);

        if (_isOptionMode)
        {
            options.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
                options.SetActive(false);
                freeSpinMode.SetActive(false);
                normalMode.SetActive(true);
                normalMode.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuint).OnComplete(() => {
                    _isNormalMode = true;
                    _isOptionMode = false;
                    _isFreeSpinMode = false;
                });
            });
        }
        else if (_isFreeSpinMode)
        {
            //normal_gachashop.transform.DOLocalMoveY(0,0.3f);
            //freespin_cat.transform.DOLocalMoveY(-13,0.3f);

            freeSpinMode.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
                freeSpinMode.SetActive(false);
                options.SetActive(false);
                normalMode.SetActive(true);
                heartTop.SetActive(false);
                normalMode.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuint).OnComplete(() => {
                    _isNormalMode = true;
                    _isOptionMode = false;
                    _isFreeSpinMode = false;
                });
            });
        }
    }
    public void FreeSpinMode()
    {
        UpdateFreeSpinModeButtons();

        btnOption.SetActive(false);

        //normal_gachashop.transform.DOLocalMoveY(-13, 0.3f);
        //freespin_cat.transform.DOLocalMoveY(-0.46f, 0.3f);

        freeSpinMode.SetActive(true);
        heartTop.SetActive(true);
        heartTop.transform.localScale = Vector3.zero;
        //particleCounting.Stop();

        //topFX.SetActive(true);

        {
            int scatterCount = 0;
            int scatterMultiply = 0;
            //Continue scatter mode
            if (SlotMachine.Instance.slotData.wildSpawnIndex < 0)
                scatterCount = SlotMachine.Instance.slotData.scatterCount;
            else
                scatterCount = SlotMachine.Instance.slotData.scatterCount + 1;

            scatterMultiply = SlotMachine.Instance.slotData.scatterMultiplier == 0 ? 2 : SlotMachine.Instance.slotData.scatterMultiplier;

            int num = 0;
            foreach (int d in SlotMachine.Instance.slotData.data)
            {
                if (d == (int)SlotMachine.SlotMachineID.Puzzle_Collectable)
                    num++;
            }

            scatterCount = scatterCount + num;
            UpdateScateMode(scatterCount, scatterMultiply);
        }

        normalMode.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            SetFreeSpinData();
            normalMode.SetActive(false);
            options.SetActive(false);
            freeSpinMode.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuint).OnComplete(() => {
                _isNormalMode = false;
                _isOptionMode = false;
                _isFreeSpinMode = true;
                if(heartTop)
                {
                    heartTop.transform.localScale = Vector3.zero;
                    heartTop.transform.DOScale(new Vector3(1, 1, 1), 0.6f).SetEase(Ease.OutElastic).OnComplete(() => {
                    });

                }
            });
        });
    }
    public void OptionMode()
    {
        normalMode.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            options.SetActive(true);
            normalMode.SetActive(false);
            options.transform.DOLocalMoveY(0, 0.6f).SetEase(Ease.OutQuint).OnComplete(() => {
                _isNormalMode = false;
                _isFreeSpinMode = false;
                _isOptionMode = true;
            });
        });
    }
    void SetFreeSpinData()
    {
        freeSpinMode.SetActive(true);
        //UIFreeSpinNumbers multiplyNumber = freeSpinMode.transform.Find("number").gameObject.GetComponent<UIFreeSpinNumbers>();
        //multiplyNumber.SetNumber(SlotMachine.Instance.slotData.scatterMultiplier);
        //heartTop.GetComponentInChildren<WhiteNumber>().SetNumber(SlotMachine.Instance.slotData.scatterCount);

        //WingNumber.SetNumber(SlotMachine.Instance.slotData.scatterMultiplier);
        //txtScatterMultiply.text = SpriteNumberManager.ToMeowWhite(SlotMachine.Instance.slotData.scatterMultiplier + "");
        //txtScatterCount.text = SpriteNumberManager.ToMeowWhite(SlotMachine.Instance.slotData.scatterCount+"");
    }
    public void SwitchGameMode()
    {
        if (_isNormalMode)
            FreeSpinMode();
        else
            NormalMode();
    }
    public void UpdateWallet(double wallet)
    {
        var formatedWallet = string.Format("{0:#,#.00}", wallet);
        txtWallet.text = formatedWallet;
    }

    public void UpdateBetTotal(float bet)
    {
        var formatedWallet = string.Format("{0:#,#.00}", bet);
        txtBet.text = formatedWallet;
    }
    //Options
    public void OnClickSetting()
    {
        SoundManager.Instance.PlaySFX("Click");

        UISettingPopup.Instance.Show();
    }
    public void OnClickRules()
    {
        SoundManager.Instance.PlaySFX("Click");

        UIRuleInfoPopup.Instance.Show();
    }
    public void OnClickPayTable()
    {
        SoundManager.Instance.PlaySFX("Click");

        UIPayInfoPopup.Instance.Show();
    }
    public void OnClickHistory()
    {
        SoundManager.Instance.PlaySFX("Click");

        UIHistoryPopUp.Instance.Show();
    }
    public void UpdateReward(float reward)
    {
/*        if (reward < 999)
            txtReward.fontSize = 10;
        else if (reward < 9999)
            txtReward.fontSize = 9;
        else if (reward < 99999)
            txtReward.fontSize = 8;
        else
            txtReward.fontSize = 7;*/
        //SlotmachineRewardAnimate.Instance.StartAnimate(reward);
        //return;
        if(reward == 0)
        {
            txtReward.text = "0.00";//SpriteNumberManager.ToMeowWhite("0.00");
        }
        else
        {
            var format = string.Format("{0:#,#.00}", reward);
            txtReward.text = format;// SpriteNumberManager.ToMeowWhite(format);
        }

    }
    public void UpdateScateMode(int count,int multiply)
    {
        txtScatterCount.text = SpriteNumberManager.ToYellow(count.ToString());
        txtScatterMultiply.text = SpriteNumberManager.ToYellow(multiply.ToString());
    }
    public void JellyHeartText()
    {
        //heartTop.GetComponentInChildren<WhiteNumber>().JellyEffect();
        //txtScatterCount.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        //txtScatterCount.transform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.OutElastic);
    }
    public void ShowSexyGirlSpin()
    {
  
    }
    public void HideSexyGirlSpin()
    {
    }
    public void CoinInsert()
    {
        //_spineAnimation.AnimationState.SetAnimation(0, coinAnimationNameInsert, false);
        //_spineAnimation.startingAnimation = coinAnimationNameInsert;
    }
    public void CoinIdle()
    {
        //_spineAnimation.AnimationState.SetAnimation(0, coinAnimationNameIdle, true);
        //_spineAnimation.startingAnimation = coinAnimationNameInsert;
    }
    public void AnimateFreespinSpinButton()
    {
        spineFreeSpinSpinButton.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        spineFreeSpinSpinButton.transform.DOScale(new Vector3(1f, 1f, 1f), 1.0f).SetEase(Ease.OutElastic);
        spineFreeSpinSpinButton.transform.DORestart();
    }
}
