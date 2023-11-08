using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class SpinButtonAnimate : MonoBehaviour
{
    enum SpinButtonAnimateState
    {
        Idle = 0,
        Action,
        None
    }

    int _state = 0;
    int _idleID = 0;
    float _currTime = 0;
    float _delay = 5;
    private SkeletonGraphic _spineAnimation;
    public static bool isSpinning = false;
    public GameObject arrow;
    public GameObject arrowBlur;

    // Start is called before the first frame update
    void Start()
    {
        _spineAnimation = GetComponent<SkeletonGraphic>();
        _state = 0;
        _currTime = 0;
        RandomIdle();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case 0:
                {
                    _currTime += Time.deltaTime;

                    //arrow.transform.Rotate(0, 0, -Mathf.PI * 0.02f);
                    if (_currTime > _delay)
                    {
                        _currTime = 0;
                        RandomIdle();
                        _state = (int)SpinButtonAnimateState.None;
                    }
                    break;
                }
            case 1:
                {
                    _currTime += Time.deltaTime;
                    //arrowBlur.transform.Rotate(0, 0, -Mathf.PI * 0.2f);

                    if (_currTime > 1.0f)
                    {
                        _state = (int)SpinButtonAnimateState.Idle;
                        _currTime = 0;
                        RandomIdle();
                    }
                    break;
                }
                case 2:
                {
                    //arrow.transform.Rotate(0, 0, -Mathf.PI * 0.05f);

                    break;
                }
        }
    }
    public void Action()
    {
        if (isSpinning)
            return;
        isSpinning = true;
        _state = (int)SpinButtonAnimateState.Action;
        _currTime = 0;
        _spineAnimation.AnimationState.SetAnimation(0, "Activate", false);

        //_spineAnimation.Skeleton.Set = "Spin_Activate";
        Debug.Log("Action");
        //transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        //transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 1.0f).SetEase(Ease.OutElastic);
        //transform.DOShakeScale(1);
        //transform.DORestart();
    }
    public void StopAction()
    {
        //isSpinning = false;
        _state = (int)SpinButtonAnimateState.None;
        _currTime = 0;
        RandomIdle();
        Debug.Log("Stop Action");
    }

    void RandomIdle()
    {
        //_spineAnimation.AnimationName = "Spin_Idle";
        _spineAnimation.AnimationState.SetAnimation(0, "Idle", true);
        //arrowBlur.SetActive(false);
        //arrow.SetActive(true);
    }
    private void OnEnable()
    {
        UIGameplay.OnUIGameplaySpinAction += Action;
        SlotMachine.OnSlotColumnStopSpin += StopAction;
    }
    private void OnDisable()
    {
        UIGameplay.OnUIGameplaySpinAction -= Action;
        SlotMachine.OnSlotColumnStopSpin -= StopAction;
    }
}
