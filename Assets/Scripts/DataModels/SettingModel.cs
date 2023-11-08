using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SettingModel
{

    /* { "_id":"643396c8c6154228bdfb65e6","balance":65303237.2,"language":"en","bgm":true,"effect":true}*/
    public string _id;
    public float balance;
    public string language;
    public bool bgm;
    public bool effect;

    public static SettingModel FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<SettingModel>(jsonString);
    }
}

