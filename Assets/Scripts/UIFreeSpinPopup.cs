using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;

public class UIFreeSpinPopup : MonoBehaviour
{
    public static UIFreeSpinPopup Instance = null;
    [SerializeField] GameObject group;
    [SerializeField] GameObject content;
    [SerializeField] GameObject fade;
    [SerializeField] UIFreeSpinNumbers freeSpinNumbers;
    [SerializeField] GameObject number;
    [SerializeField] GameObject btnStart;
    [SerializeField] SkeletonGraphic spine;
    [SerializeField] float transformDelay = 1;
    bool isTransform = false;
    bool isShow = false;
    bool isHide = false;
    bool isReady = false;
    int _scatterCount = 0;
    float _time = 0;
    float delay = 4;
    public float startDelay = 1;
    bool isDelay = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //Show(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShow) return;
        if (isDelay)
        {
            _time += Time.deltaTime;
            if(_time > startDelay)
            {
                _time = 0;
                isDelay = false;
                group.SetActive(true);
                fade.SetActive(true);
                //content.SetActive(true);
                spine.AnimationState.SetAnimation(0, "FreeSpin_Start", false);
                spine.transform.localScale = Vector3.one / 2f;
                spine.transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutElastic);
            }
        }
        else
        {
            _time += Time.deltaTime;
            if (_time > transformDelay && !isTransform)
            {
                BGMachine.Instance.FreeSpin();
                isTransform = true;
            }

            if (_time > delay)
            {
                _time = 0;
                OnStart();
            }
        }
    }
    private void OnEnable()
    {
        SlotColumn.OnScatterMode += Show;
    }
    private void OnDisable()
    {
        SlotColumn.OnScatterMode -= Show;
    }
    public void Show(int count)
    {
        isDelay = true;
        isTransform = false;
        Puzzle.isEnableClick = false;
        _time = 0;
        _scatterCount = count;
        isShow = true;
        isReady = false;
        SoundManager.Instance.PlaySFX("AllWin");
        //spine.AnimationState.SetAnimation(0, "FreeSpin_Start", false);
        //fade.SetActive(true);
        //group.SetActive(true);

        /* content.transform.DOScale(new Vector3(1, 1, 1), 1).OnComplete(()=>{
             SoundManager.Instance.PlaySFX("FreeSpin");
             _scatterCount = count;
             fade.SetActive(true);
             isShow = true;
             isReady = false;
             content.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(ShowFinish);
         });*/
    }
    void ShowFinish()
    {
        isReady = true;
        btnStart.SetActive(true);
    }
    public void Hide()
    {
        if (!isShow) return;
        isHide = true;
        isReady = false;

        /*
        spine.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutQuint);

        Sequence s = DOTween.Sequence();
        s.SetDelay(0.0f);
        //spine.transform.localScale = Vector3.zero;
        s.Append(number.transform.DOScale(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutQuint).OnComplete(HideFinish));
        s.Play();
        */
        HideFinish();
/*
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            HideFinish();
        });*/
    }
    void HideFinish()
    {
        group.SetActive(false);
        Puzzle.isEnableClick = true;
        isHide = false;
        isShow = false;
        fade.SetActive(false);
        btnStart.SetActive(false);
        //UIGameplay.Instance.FreeSpinMode();
        //SlotMachineScatterMode.Instance.StartScatterMode();
    }
    public void OnStart()
    {
        //if (!isReady) return;
        //SoundManager.Instance.PlaySFX("Close");
        Hide();
        //SoundManager.Instance.PlaySFX("Click");
        UIGameplay.Instance.FreeSpinMode();

        //SlotMachineScatterMode.Instance.StartScatterMode();
        SoundManager.Instance.PlayBGM("BGM Scatter");

    }
}
