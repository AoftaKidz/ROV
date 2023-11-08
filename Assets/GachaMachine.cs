using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class GachaMachine : MonoBehaviour
{
    public SkeletonAnimation spineGacha;
    public SkeletonAnimation spineBall;
    public static GachaMachine Instance = null;
    private void Start()
    {
        Instance = this;
        GachaBallIdle();
        BallIdle();
    }

    public void GachaBallMove()
    {
        //spineGacha.AnimationState.SetAnimation(0, "Ball_Move", false);
        //BallIdle();

    }
    public void GachaBallIdle()
    {
        //spineGacha.AnimationState.SetAnimation(0, "Ball_Idle", false);
    }
    public void BallDrop()
    {
        /*SoundManager.Instance.PlaySFX("Ball Drop");
        if(SlotMachine.Instance.slotData.ballColor == 0)
            spineBall.skeleton.SetSkin("Blue");
        else if (SlotMachine.Instance.slotData.ballColor == 1)
            spineBall.skeleton.SetSkin("Red");
        else if (SlotMachine.Instance.slotData.ballColor == 2)
            spineBall.skeleton.SetSkin("Yellow");
        spineBall.gameObject.SetActive(true);
        spineBall.AnimationState.SetAnimation(0, "Ball_Fall", false);*/
    }
    public void BallIdle()
    {
        //spineBall.gameObject.SetActive(false);
    }
}
