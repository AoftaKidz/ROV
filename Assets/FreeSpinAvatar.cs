using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpinAvatar : MonoBehaviour
{
    public static FreeSpinAvatar Instance;
    bool isAction = false;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        PlayIdle();
        Debug.Log("Avatar Idle.");

    }
    private void Awake()
    {
        Instance = this;
        PlayIdle();
        Debug.Log("Avatar Awake.");
    }
    // Update is called once per frame
    void Update()
    {
        if (!isAction) return;
        time += Time.deltaTime;
        if(time > 3)
        {
            PlayIdle();
        }
    }
    public void PlayIdle()
    {
        isAction = false;
        GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Creepy_Idle", true);
    }
    public void PlayWin()
    {
        time = 0;
        isAction = true;
        GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0, "Creepy_Win", false);
    }
}
