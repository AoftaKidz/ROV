using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlotMachineAutoSpin : MonoBehaviour
{
    static public event Action<int> OnAutoSpin;
    static public event Action OnStopAutoSpin;
    static public event Action<int> OnEndterAutoSpin;

    int _count = 0;
    int _current = 0;
    bool _isStart = false;
    float _currTime = 0;
    float _delay = 2.5f;
    bool isFirstTime = false;
    public static SlotMachineAutoSpin Instance { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (!SlotMachine.isAutoMode) return;
        if (!_isStart) return;
        //if(!UIWinBetPopup.Instance.isAppear)
        _currTime += Time.deltaTime;
        if(_currTime > _delay)
        {
            _current++;
            OnAutoSpin?.Invoke(_count - _current);
            _isStart = false;
        }
    }
    public void StartAotuSpin(int count)
    {
        SlotMachine.isAutoMode = true;
        _count = count;
        _current = 0;
        isFirstTime = true;
        _currTime = _delay;
        OnEndterAutoSpin?.Invoke(count);
        AutoSpin();
    }
    public void AutoSpin()
    {
        if (!SlotMachine.isAutoMode) return;
        if(_current >= _count)
        {
            //End
            SlotMachine.isAutoMode = false;
            StopAutoSpin();
        }
        else
        {
            _isStart = true;
            if (isFirstTime)
            {
                _currTime = _delay * 0.9f;
                isFirstTime = false;
            }
            else
            {
                _currTime = 0;
            }
        }
    }
    public void StopAutoSpin()
    {
        _isStart = false;
        _count = 0;
        _current = 0;
        SlotMachine.isAutoMode = false;
        OnStopAutoSpin?.Invoke();
    }
}
