using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimateButton : MonoBehaviour
{
    [SerializeField] SkeletonGraphic spine;
    float time = 0;
    [SerializeField] string idleName = "";
    [SerializeField] string actionName = "";
    [SerializeField] float actionDuration = 1;
    bool isAction = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayIdle();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAction) return;

        time += Time.deltaTime;
        if(time < actionDuration)
        {
            isAction = false;
            PlayIdle();
        }
    }
    public void PlayAction()
    {
        spine.AnimationState.SetAnimation(0, actionName, false);
        isAction = true;
        time = 0;
    }
    public void PlayIdle()
    {
        spine.AnimationState.SetAnimation(0, idleName, true);
        isAction = false;
        time = 0;
    }
}
