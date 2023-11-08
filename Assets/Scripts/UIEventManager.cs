using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIEventManager : MonoBehaviour
{
    static public event Action OnSlotMachineSpin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonSpinClick()
    {
        OnSlotMachineSpin?.Invoke();
    }
}
