using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[CreateAssetMenu(fileName ="SexyGirl",menuName ="GirlScriptable/SexyGirl")]
public class SexyGirlScriptable : ScriptableObject
{
    public SkeletonAnimation spine;
    public bool isNake;
    public string animateIdleName;
    public string animateActionName;
    public float animateActionDuration;
    public string animateNakeIdleName;
    public string animateNakeActionName;
    [TextArea(2, 50)]
    public string girlDescription;

    private void Awake()
    {
        
    }
    public void Idle()
    {
        if(isNake)
            spine.AnimationName = animateNakeIdleName;
        else
            spine.AnimationName = animateIdleName;
    }
    public void Action()
    {
        if (isNake)
            spine.AnimationName = animateNakeActionName;
        else
            spine.AnimationName = animateActionName;
    }
    public void SetNake(bool nake)
    {
        isNake = nake;
        Idle();
    }

}
