using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SexyGirlControl : MonoBehaviour
{
    public SexyGirlScriptable girl;
    bool isAnimateAction = false;
    float _currTime = 0;
    bool isAppear = true;
    // Start is called before the first frame update
    void Start()
    {
        girl.spine = GetComponent<SkeletonAnimation>();
        Debug.Log(girl.girlDescription);
        girl.Idle();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimateAction();
    }
    public void PlayIdle()
    {
        if (!isAppear) return;

        girl.Idle();
        isAnimateAction = false;
    }
    public void PlayAction()
    {
        if (!isAppear) return;

        girl.Action();
        isAnimateAction = true;
        _currTime = 0;
    }
    void HandleAnimateAction()
    {
        if (!isAppear) return;

        if (!isAnimateAction) return;

        _currTime += Time.deltaTime;
        if(_currTime > girl.animateActionDuration)
        {
            PlayIdle();
        }
    }
    public void SetVisible(bool visible)
    {
        isAppear = visible;
        girl.spine.gameObject.SetActive(isAppear);
    }
}
