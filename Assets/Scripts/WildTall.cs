using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class WildTall : MonoBehaviour
{
    enum WildTallState
    {
        Spawn = 0,
        Idle,
        Move
    }
    int _state = 0;
    float _currTime;
    public float delay = 0.5f;
    public GameObject spine;
    public int columnID = 0;
    //Spine
    private SkeletonAnimation _spineAnimation;
    bool _isMoving = false;
    bool _isSpawning = false;
    public string animationNameSpawn = "WildCat_Expand_Active";
    public string animationNameIdle = "WildCat_Expand_Idle";
    public string animationNameMove = "WildCat_Expand_Move";

    // Start is called before the first frame update
    void Start()
    {
        // Spine
        if (spine)
        {
            _spineAnimation = spine.GetComponent<SkeletonAnimation>();
            //_spineAnimation.AnimationName = animationNameSpawn;
            _spineAnimation.AnimationState.SetAnimation(0, animationNameSpawn, false);
            Spawn();
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case (int)WildTallState.Spawn:
            {
                    _currTime += Time.deltaTime * SlotMachine.turbo;
                    if (_currTime >= 1.5f && !_isSpawning)
                    {
                        SoundManager.Instance.PlaySFX("Expand_Wild");
                        _isSpawning = true;
                        //SoundManager.Instance.PlaySFX("Expand_Wild");
                        //_state = (int)WildTallState.Idle;
                        spine.SetActive(true);
                        //_spineAnimation.AnimationName = animationNameSpawn;
                        _spineAnimation.AnimationState.SetAnimation(0, animationNameSpawn, false);

                        //pineAnimation.loop = false;
                        transform.localPosition = GetPosition(columnID);
                        
                        transform.localScale = new Vector3(1, 1, 1);
                        //transform.DOScale(new Vector3(1, 1, 1), 0.6f).SetEase(Ease.OutElastic).OnComplete(SpawnFinish);
                        Sequence s = DOTween.Sequence();
                        s.SetDelay(0);
                        s.Append(transform.DOScale(new Vector3(1, 1, 1), 0.8f).SetEase(Ease.OutElastic).OnComplete(SpawnFinish));
                        s.Play();
                        
                    }
                        break;
            }
            case (int)WildTallState.Idle:
            {
                break;
            }
            case (int)WildTallState.Move:
            {
                    if (_isMoving) return;

                    _currTime += Time.deltaTime * SlotMachine.turbo;
                    if(_currTime >= delay)
                    {
                        //Debug.Log("Wild Moving");
                        //Moving wild
                        _state = (int)WildTallState.Idle;
                        _isMoving = true;
                        //_spineAnimation.AnimationName = animationNameMove;
                        _spineAnimation.AnimationState.SetAnimation(0, animationNameMove, false);

                        //pineAnimation.loop = false;
                        columnID--;
                        GotoColumn(columnID);
                    }
                    else
                    {
                        //Debug.Log("Wild Delay");
                    }
                    break;
            }
        }
    }
    private void OnEnable()
    {
        UIGameplay.OnUIGameplayWildMove += Move;
        SlotMachine.OnSlotColumnSpin += Move;
        SlotColumn.OnActiveWildTall += OnActiveWild;
        SlotMachine.OnSlotColumnStopSpin += Stop;
    }
    private void OnDisable()
    {
        UIGameplay.OnUIGameplayWildMove -= Move;
        SlotMachine.OnSlotColumnSpin -= Move;
        SlotColumn.OnActiveWildTall -= OnActiveWild;
        SlotMachine.OnSlotColumnStopSpin -= Stop;
    }
    public void GotoColumn(int column)
    {
        //spine.transform.DOLocalMoveY(0, bounceDuration).SetEase(Ease.OutElastic).OnComplete(BounceFinish);
        //spine.transform.DORestart();
        SoundManager.Instance.PlaySFX("WildWalk");
        Vector3 p = GetPosition(column);
        Sequence s = DOTween.Sequence();
        s.SetDelay(0);
        s.Append(transform.DOLocalMove(p, 1.0f).SetEase(Ease.OutQuint).OnComplete(MoveFinish));
        s.Play();
        //DOTween.Sequence().SetDelay(1f).Append(transform.DOPath(XXX).OnComplete(XXX)).AppendInterval(1f).SetLoops(-1, LoopType.Yoyo).Play();

    }
    Vector3 GetPosition(int column)
    {
        float x = 0.02f;
        float y = 0.0f;
        switch (column)
        {
            case 0: return new Vector3(-6.0f+x, y,5 - columnID);
            case 1:return new Vector3(-1.29f + x, y, 5 - columnID);
            case 2: return new Vector3(1.45f + x, y, 5 - columnID);
            case 3: return new Vector3(4.19f + x, y, 5 - columnID);
            case 4: return new Vector3(6.97f + x, y, 5 - columnID);
            case 5: return new Vector3(9.73f + x, y, 5 - columnID);
        }
        return Vector3.zero;
    }
    void Spawn()
    {
        spine.SetActive(false);
        _state = (int)WildTallState.Spawn;
        _currTime = 0;

    }
    void SpawnFinish()
    {
        Idle();

    }
    void Idle()
    {
        _state = (int)WildTallState.Idle;
        //_spineAnimation.AnimationName = animationNameIdle;
        _spineAnimation.AnimationState.SetAnimation(0, animationNameIdle, true);

        //pineAnimation.loop = true;
        Debug.Log("Wild Idle");

    }
    public void Move()
    {
        _isMoving = false;
        _currTime = 0;
        _state = (int)WildTallState.Move;
        Debug.Log("Wild Move Delay");
    }
    public void Stop()
    {
        if (_state != (int)WildTallState.Move) return;

        _state = (int)WildTallState.Idle;
        _isMoving = true;
        //_spineAnimation.AnimationName = animationNameMove;
        _spineAnimation.AnimationState.SetAnimation(0, animationNameMove, false);

        //_spineAnimation.loop = false;
        columnID--;
        GotoColumn(columnID);
    }
    void MoveFinish()
    {
        if (columnID > 0)
            Idle();
        else
            Destroy(gameObject);
    }
    void OnActiveWild(int column)
    {
        if (column == columnID)
        {
            SlotMachine.activeWildTall = this;
        }
    }
    public bool Busy()
    {
        if (_state != (int)WildTallState.Idle) return true;
        else
            return false;
    }
}
