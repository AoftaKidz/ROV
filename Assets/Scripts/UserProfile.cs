using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UserProfile : MonoBehaviour
{
    public static event Action<double> OnUpdateUserProfile;
    public double wallet;
    public float betTotal;
    public bool isOnSpeaker = true;
    public bool isOnEffect = true;
    public string language = "th";
    public string username = "";
    public string token = "";
    public string settingID = "";
    public static UserProfile Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        betTotal = 9;

    }
    private void Start()
    {

        //CallUpdateUserProfile();

    }
    public void CallUpdateUserProfile()
    {
        //wallet = Random.Range(0, 10000);
        OnUpdateUserProfile?.Invoke(wallet);
    }

}
