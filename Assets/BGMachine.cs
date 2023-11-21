using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BGMachine : MonoBehaviour
{
    public GameObject normalBG;
    public GameObject freespinBG;
    public GameObject normalMode;
    public GameObject freespinMode;
    public SkeletonAnimation spineMeowFreespin;

    public static BGMachine Instance = null;
    private void Start()
    {
        Instance = this;
        Normal();
    }
    public void Normal()
    {
        freespinBG.SetActive(false);
        normalBG.SetActive(true);
        freespinMode.SetActive(false);
    }
    public void FreeSpin()
    {
        normalBG.SetActive(false);
        freespinBG.SetActive(true);
        freespinMode.SetActive(true);

    }
    public void FreeSpin_CatIdle()
    {
    }
}
