using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class AccountDataModel
{
    public string id;
    public string username;
    public float balance;
    public string access_token;
}
[Serializable]
public class AccountModel
{
    /*
    {"success":true,
    "data":{"id":"643396c8c6154228bdfb65e6",
    "username":"devbumbo000004",
    "balance":65264868.88,
    "access_token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwaWQiOjE2LCJ1c2VybmFtZSI6ImRldmJ1bWJvMDAwMDA0IiwiaWF0IjoxNjgxMTAyNTM2LCJleHAiOjE2ODExMDM0MzZ9.RtW1ZzF1HtCYdDXt5ENks1PplPquAI3r4yDTg6d0wIw","refresh_token":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwaWQiOjE2LCJ1c2VybmFtZSI6ImRldmJ1bWJvMDAwMDA0IiwiaWF0IjoxNjgxMTAyNTM2LCJleHAiOjE2ODE3MDczMzZ9.PaPOVViC6-n1WPlbhrw5nJonQwyswH2VnEPGRsJbEV8"}}
    */
    public bool success;
    public AccountDataModel data;
    public static AccountModel FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<AccountModel>(jsonString);
    }
}
