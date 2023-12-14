using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotColumn : MonoBehaviour
{
    enum SlotColumnState
    {
        None = 0,
        PreSpin,
        Spin,
    }
    static public event Action<int> OnSpinFinish;
    static public event Action<int> OnActiveWildTall;
    static public event Action<int> OnScatterMode;
    static public event Action<float> OnUpdateReward;
    static public int scatterCount = 0;

    public int maxScatterCount = 2;
    public GameObject prefabWildTall;
    public float duration = 2;
    public float delay = 0;
    public GameObject sprite;
    public int columnID = 0;
    public float speed = 1;
    int _state = 0;
    float _currTime = 0;
    float _currDelayTime = 0;
    Vector2 _offset = new Vector2(0, 0);
    public bool isSlowMotion = false;
    public float slowMotionFactor = 0.3f;
    public float slowMotionDuration = 1.5f;
    public GameObject scatterFX;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer rend = sprite.GetComponent<SpriteRenderer>();
        Material mat = Instantiate(rend.material);
        rend.material = mat;
        //SetMaterial();
        sprite.SetActive(false);
        if (scatterFX.activeSelf)
            scatterFX.SetActive(false);
    }
    private void OnEnable()
    {
        //Subscribe event
        SlotMachine.OnSlotColumnSpin += Spin;
        SlotMachine.OnSlotColumnStopSpin += Stop;
        SlotMachine.OnSlotColumnPreSpin += PreSpin;

    }

    private void OnDisable()
    {
        //Unsubscribe event
        SlotMachine.OnSlotColumnSpin -= Spin;
        SlotMachine.OnSlotColumnStopSpin -= Stop;
        SlotMachine.OnSlotColumnPreSpin -= PreSpin;

    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    _currDelayTime += Time.deltaTime * SlotMachine.turbo;
                    if (_currDelayTime < delay) return;

                    //Spinning
                    _offset.y += speed * Time.deltaTime;
                    SetMaterial();
                    break;
                }
            case 2:
                {

                    //SetMaterial();
                    float _d = duration;

                    if (scatterCount >= maxScatterCount)
                    {
                        //Slow motion mode
                        _currTime += Time.deltaTime;
                        _d = duration + slowMotionDuration;
                        if (!scatterFX.activeSelf)
                            scatterFX.SetActive(true);
                    }
                    else
                    {
                        _currTime += Time.deltaTime * SlotMachine.turbo;
                        if (scatterFX.activeSelf)
                            scatterFX.SetActive(false);
                    }

                    //SetMaterial();
                    //_currTime += Time.deltaTime * SlotMachine.turbo;
                    //if (_currTime >= duration)
                    if (_currTime >= _d)
                    {
                        scatterFX.SetActive(false);

                        //Spin finish
                        CountTheScatter();

                        GachaMachine.Instance.BallDrop();
                        UIGameplay.Instance.CoinIdle();
                        _state = (int)SlotColumnState.None;
                        sprite.SetActive(false);

                        OnSpinFinish?.Invoke(columnID);
                        SoundManager.Instance.PlaySFX("SpinStop");

                        if (columnID == 5)
                        {
                            SoundManager.Instance.StopSFX("SlotSpin");

                            SpinButtonAnimate.isSpinning = false;
                            LineManager.Instance.CreateLine();
                            SlotMachine.isSpinning = false;

                            SlotMachine slot = SlotMachine.Instance;
                            if (UIKindOfMeowPopup.Instance.Condition())
                            {
                                //Show UIWinBetPopup
                                UIKindOfMeowPopup.Instance.Show();
                            }
                            else if (UIBigWinPopup.Instance.Condition())
                            {
                                //Show UIWinBetPopup
                                UIBigWinPopup.Instance.Show();
                            }
                            //Check for Free spin mode
                            else if (slot.slotData.isScatterMode == false && slot.slotData.comingFreeSpinCount > 0)
                            {
                                //Start scatter
                                ScatterModeInvoke(slot.slotData.comingFreeSpinCount);
                            }
                            else if (slot.slotData.isScatterMode)
                            {
                                //Update round reward
                                if (!SlotMachine.isFreeSpinMode)
                                {
                                    UIGameplay.Instance.FreeSpinMode();
                                }

                                float more_delay = 0.5f;
                                if (SlotMachine.Instance.slotData.wildSpawnIndex >= 0)
                                    more_delay = 5f;
                                UIRoundRewardPopup.Instance.Show(more_delay);
                            }
                            else if (SlotMachine.isAutoMode)
                            {
                                bool check = true;
                                //Check increase Balance Stop
                                if (UIAutoSpinSettingPopup.Instance.decreaseBalanceStop > 0)
                                {
                                    double balance_diff = UIAutoSpinSettingPopup.Instance.startBalance - SlotMachine.Instance.slotData.userBalance;
                                    //Debug.Log("Decrease Balance stop : " + UIAutoSpinSettingPopup.Instance.startBalance + ", balance : " + SlotMachine.Instance.slotData.userBalance);
                                    if (balance_diff > 0 && balance_diff >= UIAutoSpinSettingPopup.Instance.decreaseBalanceStop)
                                    {
                                        //Debug.Log("Stop auto " + balance_diff);
                                        SlotMachineAutoSpin.Instance.StopAutoSpin();
                                        check = false;
                                    }
                                }

                                //Check increase Balance Stop
                                if (UIAutoSpinSettingPopup.Instance.increaseBalanceStop > 0)
                                {
                                    double balance_diff = SlotMachine.Instance.slotData.userBalance - UIAutoSpinSettingPopup.Instance.startBalance;
                                    //Debug.Log("Increase Balance stop : " + UIAutoSpinSettingPopup.Instance.startBalance + ", balance : " + SlotMachine.Instance.slotData.userBalance);

                                    if (balance_diff > 0 && balance_diff >= UIAutoSpinSettingPopup.Instance.increaseBalanceStop)
                                    {
                                        //Debug.Log("Stop auto " + balance_diff);
                                        SlotMachineAutoSpin.Instance.StopAutoSpin();
                                        check = false;
                                    }
                                }

                                //Check Reward Stop
                                if (UIAutoSpinSettingPopup.Instance.rewardStop > 0)
                                {
                                    //Debug.Log("increase Balance stop : " + SlotMachine.Instance.slotData.reward);
                                    if (SlotMachine.Instance.slotData.reward >= UIAutoSpinSettingPopup.Instance.rewardStop)
                                    {
                                        //Debug.Log("Stop auto " + SlotMachine.Instance.slotData.reward + " >= " + UIAutoSpinSettingPopup.Instance.rewardStop);
                                        SlotMachineAutoSpin.Instance.StopAutoSpin();
                                        check = false;
                                    }
                                }

                                if (check)
                                {
                                    SlotMachineAutoSpin.Instance.AutoSpin();
                                }
                            }

                            //Update score
                            //OnUpdateReward?.Invoke(slot.slotData.totalReward);
                            /* if(SlotMachine.isFreeSpinMode)
                                 UIGameplay.Instance.UpdateReward(slot.slotData.totalReward);
                             else
                                 UIGameplay.Instance.UpdateReward(slot.slotData.reward);
                             UserProfile.Instance.wallet = slot.slotData.userBalance;
                             UserProfile.Instance.CallUpdateUserProfile();*/
                            UserProfile.Instance.wallet = slot.slotData.userBalance;

                            if (SlotMachine.isFreeSpinMode)
                                UIGameplay.Instance.UpdateReward(slot.slotData.totalReward);
                            else
                            {
                                UIGameplay.Instance.UpdateReward(slot.slotData.reward);
                                //Update wallet for non free spin mode only
                                UserProfile.Instance.CallUpdateUserProfile();
                            }

                            if (slot.slotData.totalReward > 0)
                            {
                                //SoundManager.Instance.PlaySFX("GirlRedHair_Win");
                                //SoundManager.Instance.PlaySFX("GirlGoldHair_Win");
                            }

                            //UIGameplay.Instance.HideSexyGirlSpin();
                            //SoundManager.Instance.StopSFX("SlotSpin");

                        }

                        CreateWildTall();
                    }
                    else
                    {
                        //Spinning
                        //Spinning
                        if (scatterCount >= maxScatterCount)
                            _offset.y += speed * Time.deltaTime * slowMotionFactor;
                        else
                            _offset.y += speed * Time.deltaTime;
                        //_offset.y += speed * Time.deltaTime;
                        SetMaterial();
                    }
                    break;
                }
        }
    }
    public void PreSpin()
    {
        scatterCount = 0;
        sprite.SetActive(true);
        _state = (int)SlotColumnState.PreSpin;
        _currTime = 0;
        _currDelayTime = 0;
        _offset = Vector2.zero;
        _offset.y = (float)UnityEngine.Random.Range(0, 100) / 100.0f;
        //Debug.Log(_offset);
        SetMaterial();
    }
    public void Spin()
    {
        _state = (int)SlotColumnState.Spin;
        _currTime = 0;
        _currDelayTime = 0;
        //_offset = Vector2.zero;
        //_offset.y = (float)UnityEngine.Random.Range(0, 100) /100.0f;
        //Debug.Log(_offset);
        //SetMaterial();
    }
    public void Stop()
    {
        _currDelayTime = delay;
        _currTime = duration;
    }
    void SetMaterial()
    {
        SpriteRenderer rend = sprite.GetComponent<SpriteRenderer>();
        Material _mat = rend.material;
        _mat.SetVector("_Offset", _offset);
    }
    public void CreateWildTall(bool isForceCreate = false)
    {
        //Sending message to all wild tall
        SlotMachine.activeWildTall = null;
        OnActiveWildTall?.Invoke(columnID);

        //Check reponse from wild tall
        if (SlotMachine.activeWildTall == null)
        {
            SlotMachine slot = SlotMachine.Instance;
            //Check wild in this columnID
            int startIndex = (columnID - 1) * 3;
            bool isWald = false;
            if (slot.slotData.data[startIndex] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
                slot.slotData.data[startIndex + 1] == (int)SlotMachine.SlotMachineID.Puzzle_Wild ||
                slot.slotData.data[startIndex + 2] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                isWald = true;

            if (isWald || isForceCreate)
            {
                //Create wild tall prefab
                GameObject o = Instantiate(prefabWildTall, Vector3.zero, Quaternion.identity);
                WildTall wild = o.GetComponent<WildTall>();
                wild.columnID = columnID;
                o.transform.parent = slot.transform;

            }
        }
    }
    public float GetSpinTime()
    {
        return _currTime;
    }
    public void ScatterModeInvoke(int count)
    {
        Puzzle.isEnableClick = false;
        OnScatterMode?.Invoke(count);
    }
    void CountTheScatter()
    {
        int col = columnID - 1;
        int a = col * 3;
        int b = col * 3 + 1;
        int c = col * 3 + 2;

        if (SlotMachine.Instance.slotData.data[a] == (int)SlotMachine.SlotMachineID.Puzzle_Scatter)
        {
            scatterCount++;
        }
        else if (SlotMachine.Instance.slotData.data[b] == (int)SlotMachine.SlotMachineID.Puzzle_Scatter)
        {
            scatterCount++;
        }
        else if (SlotMachine.Instance.slotData.data[c] == (int)SlotMachine.SlotMachineID.Puzzle_Scatter)
        {
            scatterCount++;
        }

    }
    static public SlotColumn GetSlotColumn(int column)
    {
        var all = GameObject.FindGameObjectsWithTag("SlotColumn");
        foreach (var c in all)
        {
            if (c.GetComponent<SlotColumn>().columnID == column) return c.GetComponent<SlotColumn>();
        }

        return null;
    }
}
